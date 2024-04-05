﻿using EFCore.BulkExtensions;
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
using Castle.DynamicProxy;


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
            using PostgresContext context = new PostgresContext();
            List<Stt> stts = context.Stts.ToList();
            List<SttAggregateMetric> aggregates = new List<SttAggregateMetric>();
            List<SttAggregateMetric> STTScenarioAggregates = new List<SttAggregateMetric>();
            int sttCount = 0; //do a query here to get the total number of stts in the database


            

            for (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                if (request.ScenarioList?[i].Id != null)
                {
                    //List<SttAggregateMetric> sttAggregates = new List<SttAggregateMetric>();
                    foreach( Stt stt in stts)
                    {
                        STTScenarioAggregates = context.SttAggregateMetrics.Where(s => s.ScenarioId == i)
                                                                           .Where(s => s.SttId == stt.Id)
                                                                           .ToList();
                        // List<SttAggregateMetric> singleSttAggregateList = new List<SttAggregateMetric>();
                        
                        //first collect all agregate metrics of the given scenario id and stt id with a sql query
                        //singleSttAggregateList.Add(query in here)
                        SttAggregateMetric aggregateAvg = new SttAggregateMetric(); //cumulative by scenario
                        double? avgWer = 0;
                        double? avgMer = 0;
                        double? avgWil = 0;
                        double? avgSim = 0;
                        double? avgDist = 0;
                        TimeSpan? avgSpeed = new TimeSpan?();
                        foreach (SttAggregateMetric aggregate in STTScenarioAggregates)
                        {
                            avgWer += aggregate.Wer;
                            avgMer += aggregate.Mer;
                            avgWil += aggregate.Wil;
                            avgSim += aggregate.Sim;
                            avgDist += aggregate.Dist;
                            avgSpeed += aggregate.Rawtime;
                        }

                        int total = STTScenarioAggregates.Count;
                        avgWer   /= total;
                        avgMer   /= total;
                        avgWil   /= total;
                        avgSim   /= total;
                        avgDist  /= total;
                        avgSpeed /= total;
                        
                        aggregateAvg.SttId = stt.Id;
                        aggregateAvg.ScenarioId = i;
                        aggregateAvg.Rawtime = avgSpeed / total;
                        aggregateAvg.Wer     = avgWer / total;
                        aggregateAvg.Mer     = avgMer / total;
                        aggregateAvg.Wil     = avgWil / total;
                        aggregateAvg.Sim     = avgSim / total;
                        aggregateAvg.Dist    = avgDist / total;
                        //sttAggregates.Add(sttAggregate);
                        STTScenarioAggregates.Add(aggregateAvg);

                        
                    }
                }
            }
            for (int i = 0; i < sttCount; i++)
            {
                double? wer = 0;
                double? mer = 0;
                double? wil = 0;
                double? sim = 0;
                double? dist = 0;
                TimeSpan? speed = new TimeSpan?();
                SttAggregateMetric aggregateAvg = new SttAggregateMetric();
                foreach (SttAggregateMetric aggregate in STTScenarioAggregates)
                {
                    wer += aggregate.Wer; //* scenario weight (using aggregate.scenarioId)
                    mer += aggregate.Mer;
                    wil += aggregate.Wil;
                    sim += aggregate.Sim;
                    dist += aggregate.Dist;
                    speed += aggregate.Rawtime;
                }
                aggregateAvg.SttId = i;
                aggregateAvg.Rawtime = speed / STTScenarioAggregates.Count;
                aggregateAvg.Wer = wer / STTScenarioAggregates.Count;
                aggregateAvg.Mer = mer / STTScenarioAggregates.Count;
                aggregateAvg.Wil = wil / STTScenarioAggregates.Count;
                aggregateAvg.Sim = sim / STTScenarioAggregates.Count;
                aggregateAvg.Dist = dist / STTScenarioAggregates.Count;
                aggregates.Add(aggregateAvg);
            }

            return Enumerable.Range(0, aggregates.Count).Select(index => new RankingsResponseModel 
            {
                SttName = context.Stts.Find(aggregates[index].SttId).Name, // query for name using aggregates[index].SttId
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
