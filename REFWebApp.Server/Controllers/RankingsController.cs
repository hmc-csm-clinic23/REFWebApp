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
    public class RankingsController : ControllerBase
    {

        private readonly ILogger<RankingsController> _logger;

        public RankingsController(ILogger<RankingsController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostMetrics")]
        public IEnumerable<RankingsResponseModel> Post([FromBody] RankingsRequestModel request)
        {
            List<SttAggregateMetric> aggregates = new List<SttAggregateMetric>();
            List<List<SttAggregateMetric>> scenarioAggregates = new List<List<SttAggregateMetric>>();
            int sttCount = 0; //do a query here to get the total number of stts in the database

            for (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                if (request.ScenarioList?[i].Id != null)
                {
                    List<SttAggregateMetric> sttAggregates = new List<SttAggregateMetric>();
                    for (int j = 0; j < sttCount; j++)
                    {
                        List<SttAggregateMetric> singleSttAggregateList = new List<SttAggregateMetric>();
                        //first collect all agregate metrics of the given scenario id and stt id with a sql query
                        //singleSttAggregateList.Add(query in here)
                        SttAggregateMetric sttAggregate = new SttAggregateMetric();
                        double? werVal = 0;
                        double? merVal = 0;
                        double? wilVal = 0;
                        double? simVal = 0;
                        double? distVal = 0;
                        TimeSpan? speedVal = new TimeSpan?();
                        foreach (SttAggregateMetric singleSttAggregate in singleSttAggregateList)
                        {
                            werVal += singleSttAggregate.Wer;
                            merVal += singleSttAggregate.Mer;
                            wilVal += singleSttAggregate.Wil;
                            simVal += singleSttAggregate.Sim;
                            distVal += singleSttAggregate.Dist;
                            speedVal += singleSttAggregate.Rawtime;
                        }

                        sttAggregate.SttId = j;
                        sttAggregate.Rawtime = speedVal / singleSttAggregateList.Count;
                        sttAggregate.Wer = werVal / singleSttAggregateList.Count;
                        sttAggregate.Mer = merVal / singleSttAggregateList.Count;
                        sttAggregate.Wil = wilVal / singleSttAggregateList.Count;
                        sttAggregate.Sim = simVal / singleSttAggregateList.Count;
                        sttAggregate.Dist = distVal / singleSttAggregateList.Count;
                        sttAggregates.Add(sttAggregate);
                    }
                    scenarioAggregates.Add(sttAggregates);
                };
            }
            for (int i = 0; i < sttCount; i++)
            {
                double? wer = 0;
                double? mer = 0;
                double? wil = 0;
                double? sim = 0;
                double? dist = 0;
                TimeSpan? speed = new TimeSpan?();
                SttAggregateMetric aggregate = new SttAggregateMetric();
                foreach (List<SttAggregateMetric> scenarioAggregate in scenarioAggregates)
                {
                    wer += scenarioAggregate[i].Wer;
                    mer += scenarioAggregate[i].Mer;
                    wil += scenarioAggregate[i].Wil;
                    sim += scenarioAggregate[i].Sim;
                    dist += scenarioAggregate[i].Dist;
                    speed += scenarioAggregate[i].Rawtime;
                }
                aggregate.SttId = i;
                aggregate.Rawtime = speed / scenarioAggregates.Count;
                aggregate.Wer = wer / scenarioAggregates.Count;
                aggregate.Mer = mer / scenarioAggregates.Count;
                aggregate.Wil = wil / scenarioAggregates.Count;
                aggregate.Sim = sim / scenarioAggregates.Count;
                aggregate.Dist = dist / scenarioAggregates.Count;
                aggregates.Add(aggregate);
            }

            return Enumerable.Range(0, aggregates.Count).Select(index => new RankingsResponseModel
            {
                SttName = "hi", // query for name using aggregates[index].SttId
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

        public class RankingsRequestModel
        {
            public IndividualScenarioRequest[]? ScenarioList { get; set; }
        }

        public class RankingsResponseModel
        {
            public string? SttName { get; set; }
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
