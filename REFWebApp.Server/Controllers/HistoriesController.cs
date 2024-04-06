using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Model.STTs;
using REFWebApp.Server.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Text.Json;
using System.IO;
using System;
using Microsoft.Identity.Client;
using REFWebApp.Server.Model;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.ConstrainedExecution;
using System.Collections.Generic;


namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoriesController : ControllerBase
    {

        private readonly ILogger<HistoriesController> _logger;

        public HistoriesController(ILogger<HistoriesController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostHistories")]
        public IEnumerable<HistoriesResponseModel> Post([FromBody] HistoriesRequestModel request)
        {
            using PostgresContext context = new PostgresContext();
            List<Scenario> scenarios = context.Scenarios.ToList();
            List<SttAggregateMetric> aggregates = new List<SttAggregateMetric>();
            List<Stt> stts = context.Stts.Where(s => s.Name == request.SttName).ToList();
            Stt stt = stts[0];
            List<int> scenarioIds = new List<int>();

            foreach (Scenario scenario in scenarios)
            {
                List<SttAggregateMetric> scenarioAggregates = new List<SttAggregateMetric>();
                scenarioAggregates = context.SttAggregateMetrics.Where(s => s.ScenarioId == scenario.Id)
                                                                   .Where(s => s.SttId == stt.Id)
                                                                   .ToList();
                if (scenarioAggregates.Count != 0)
                {
                    SttAggregateMetric aggregate = new SttAggregateMetric();
                    double? wer = 0;
                    double? mer = 0;
                    double? wil = 0;
                    double? sim = 0;
                    double? dist = 0;
                    TimeSpan? speed = new TimeSpan(0, 0, 0, 0);
                    foreach (SttAggregateMetric scenarioAggregate in scenarioAggregates)
                    {
                        wer += scenarioAggregate.Wer;
                        mer += scenarioAggregate.Mer;
                        wil += scenarioAggregate.Wil;
                        sim += scenarioAggregate.Sim;
                        dist += scenarioAggregate.Dist;
                        speed += scenarioAggregate.Rawtime;
                    }
                    int total = scenarioAggregates.Count;
                    aggregate.ScenarioId = scenario.Id;
                    aggregate.Rawtime = speed / total;
                    aggregate.Wer = wer / total;
                    aggregate.Mer = mer / total;
                    aggregate.Wil = wil / total;
                    aggregate.Sim = sim / total;
                    aggregate.Dist = dist / total;
                    aggregates.Add(aggregate);
                }
            }
            //when you do the eval controller do a sql count* for a given scenario and then when they click on the number/index of an eval do the full query of the transcriptions table
            return Enumerable.Range(0, aggregates.Count).Select(index => new HistoriesResponseModel
            {
                ScenarioName = context.Scenarios.Find(aggregates[index].ScenarioId).Name, // query for name using aggregates[index].SttId
                Accuracy = 0,
                Speed = TimeSpan.FromSeconds(Math.Round(((TimeSpan)aggregates[index].Rawtime).TotalSeconds)),
                Wer = aggregates[index].Wer,
                Mer = aggregates[index].Mer,
                Wil = aggregates[index].Wil,
                Sim = aggregates[index].Sim,
                Dist = aggregates[index].Dist
            })
            .ToArray();
        }

        [NonAction]
        public void AddTranscriptionsToDB(List<Transcription> transcriptions)
        {
            using (var context = new PostgresContext())
            {
                context.BulkInsert(transcriptions);
            }
        }

        public class MetricObject
        {
            public List<List<List<float>>>? Metrics { get; set; }
            public List<List<string>>? Transcriptions { get; set; }
            public List<List<string>>? GroundTruths { get; set; }
        }

        public class HistoriesRequestModel
        {
            public string? SttName { get; set; }
        }

        public class HistoriesResponseModel
        {
            public string? ScenarioName { get; set; }
            public double? Accuracy { get; set; }
            public TimeSpan? Speed { get; set; }
            public double? Wer { get; set; }
            public double? Mer { get; set; }
            public double? Wil { get; set; }
            public double? Sim { get; set; }
            public double? Dist { get; set; }
        }
    }
}
