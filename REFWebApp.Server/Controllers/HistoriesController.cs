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

        [HttpPost(Name = "PostMetrics")]
        public IEnumerable<HistoriesResponseModel> Post([FromBody] HistoriesRequestModel request)
        {
            List<SttAggregateMetric> aggregates = new List<SttAggregateMetric>();
            int scenarioCount = 0; //do a query here to get the total number of scenarios in the database
            for (int j = 0; j < scenarioCount; j++)
            {
                List<SttAggregateMetric> scenarioAggregateList = new List<SttAggregateMetric>();
                //first collect all agregate metrics of the given scenario id and stt id with a sql query
                //scenarioAggregateList.Add(query in here)
                SttAggregateMetric aggregate = new SttAggregateMetric();
                double? wer = 0;
                double? mer = 0;
                double? wil = 0;
                double? sim = 0;
                double? dist = 0;
                TimeSpan? speed = new TimeSpan?();
                foreach (SttAggregateMetric scenarioAggregate in scenarioAggregateList)
                {
                    wer += scenarioAggregate.Wer;
                    mer += scenarioAggregate.Mer;
                    wil += scenarioAggregate.Wil;
                    sim += scenarioAggregate.Sim;
                    dist += scenarioAggregate.Dist;
                    speed += scenarioAggregate.Rawtime;
                }

                aggregate.SttId = j;
                aggregate.Rawtime = speed / scenarioAggregateList.Count;
                aggregate.Wer = wer / scenarioAggregateList.Count;
                aggregate.Mer = mer / scenarioAggregateList.Count;
                aggregate.Wil = wil / scenarioAggregateList.Count;
                aggregate.Sim = sim / scenarioAggregateList.Count;
                aggregate.Dist = dist / scenarioAggregateList.Count;
                aggregates.Add(aggregate);
            }
            //when you do the eval controller do a sql count* for a given scenario and then when they click on the number/index of an eval do the full query of the transcriptions table
            return Enumerable.Range(0, aggregates.Count).Select(index => new HistoriesResponseModel
            {
                ScenarioName = "hi", // query for name using aggregates[index].SttId
                TotalScore = 0, //this should be a complex line or function that combines our metrics, speed, usability, etc.
                Speed = aggregates[index].Rawtime,
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
            public IndividualStt[]? SttList { get; set; }
        }

        public class HistoriesResponseModel
        {
            public string? ScenarioName { get; set; }
            public float? TotalScore { get; set; }
            public TimeSpan? Speed { get; set; }
            public double? Wer { get; set; }
            public double? Mer { get; set; }
            public double? Wil { get; set; }
            public double? Sim { get; set; }
            public double? Dist { get; set; }
        }
    }
}
