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
using Castle.DynamicProxy;
using System.ComponentModel;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;


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

        [HttpPost(Name = "PostRankings")]
        public IEnumerable<RankingsResponseModel> Post([FromBody] RankingsRequestModel request)
        {
            using PostgresContext context = new PostgresContext();
            List<Stt> stts = context.Stts.ToList();
            List<SttAggregateMetric> aggregates = new List<SttAggregateMetric>();
            List<SttAggregateMetric> initialAggregates = new List<SttAggregateMetric>();
            List<int> sttIds = new List<int>();
            Dictionary<int?, int?> scenarioWeightMap = new Dictionary<int?, int?>();
            int weightsIndex = 0;
            float weightsAverage = request.WeightList.Sum();

            foreach (IndividualScenarioRequest? scenario in request.ScenarioList)
            {
                scenarioWeightMap.Add(scenario.Id, request.WeightList[weightsIndex]);
                weightsIndex += 1;
                    foreach(Stt stt in stts)
                    {
                        List<SttAggregateMetric> STTScenarioAggregates = new List<SttAggregateMetric>();
                        STTScenarioAggregates = context.SttAggregateMetrics.Where(s => s.ScenarioId == scenario.Id)
                                                                           .Where(s => s.SttId == stt.Id)
                                                                           .ToList();
                        if (STTScenarioAggregates.Count != 0) {
                            if (!sttIds.Contains(stt.Id)) 
                            {
                                sttIds.Add(stt.Id);
                            }
                            SttAggregateMetric aggregateAvg = new SttAggregateMetric();
                            double? avgWer = 0;
                            double? avgMer = 0;
                            double? avgWil = 0;
                            double? avgSim = 0;
                            double? avgDist = 0;
                            TimeSpan? avgSpeed = new TimeSpan(0, 0, 0, 0);
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
                        
                            aggregateAvg.SttId = stt.Id;
                            aggregateAvg.ScenarioId = scenario.Id;
                            aggregateAvg.Rawtime = avgSpeed / total;
                            aggregateAvg.Wer     = avgWer / total;
                            aggregateAvg.Mer     = avgMer / total;
                            aggregateAvg.Wil     = avgWil / total;
                            aggregateAvg.Sim     = avgSim / total;
                            aggregateAvg.Dist    = avgDist / total;
                            initialAggregates.Add(aggregateAvg);  
                        }
                    }
            }
            foreach (int i in sttIds)
            {
                double? wer = 0;
                double? mer = 0;
                double? wil = 0;
                double? sim = 0;
                double? dist = 0;
                double? multiplier = 1;
                int count = 0;
                TimeSpan? speed = new TimeSpan(0, 0, 0, 0);
                SttAggregateMetric aggregateAvg = new SttAggregateMetric();

                foreach (SttAggregateMetric aggregate in initialAggregates)
                {
                    if (aggregate.SttId == i)
                    {
                        multiplier = scenarioWeightMap[aggregate.ScenarioId] / weightsAverage;
                        count += 1;
                        wer += aggregate.Wer * multiplier; //* scenario weight (using aggregate.scenarioId)
                        mer += aggregate.Mer * multiplier;
                        wil += aggregate.Wil * multiplier;
                        sim += aggregate.Sim * multiplier;
                        dist += aggregate.Dist * multiplier;
                        speed += aggregate.Rawtime * multiplier;
                    }
                }
                aggregateAvg.SttId = i;
                aggregateAvg.Rawtime = speed;// / count;
                aggregateAvg.Wer = wer;// / count;
                aggregateAvg.Mer = mer;// / count;
                aggregateAvg.Wil = wil;// / count;
                aggregateAvg.Sim = sim;// / count;
                aggregateAvg.Dist = dist;// / count;
                aggregates.Add(aggregateAvg);
            }
            return Enumerable.Range(0, aggregates.Count).Select(index => new RankingsResponseModel 
            {
                SttName = context.Stts.Find(aggregates[index].SttId).Name,
                TotalScore = FinalScore(FinalAccuracy(aggregates[index].Wer, aggregates[index].Mer, aggregates[index].Wil, aggregates[index].Sim, aggregates[index].Dist), TimeSpan.FromSeconds(Math.Round(((TimeSpan)aggregates[index].Rawtime).TotalSeconds)), context.Stts.Find(aggregates[index].SttId).Usability),
                Accuracy = FinalAccuracy(aggregates[index].Wer, aggregates[index].Mer, aggregates[index].Wil, aggregates[index].Sim, aggregates[index].Dist),
                Speed = TimeSpan.FromSeconds(Math.Round(((TimeSpan)aggregates[index].Rawtime).TotalSeconds)),
                Wer = aggregates[index].Wer,
                Mer = aggregates[index].Mer,
                Wil = aggregates[index].Wil,
                Sim = aggregates[index].Sim,
                Dist = aggregates[index].Dist,

            })
            .ToArray();
        }

        // If using weights, use this function. TODO: Add usability
        // public double? Weight(List<double?> weights, List<double?> metrics, TimeSpan speed)
        // {   
        //     double finalSpeed = Math.Min(1, 1/speed.TotalSeconds);
        //     List<double?> weighted = new List<double?>();
        //     for (int i = 0;  i < weights.Count; i++)
        //     {
        //         weighted.Add(weights[i]*metrics[i]);
        //     }
        //     return (weighted.Sum(x => x) + finalSpeed)/2;
        // }

        [NonAction]
        public double? FinalScore(double? accuracy, TimeSpan speed, double? usability)
        {
            double finalSpeed = Math.Min(100, (1 / speed.TotalSeconds * 800));
            return (accuracy + finalSpeed + (usability * 100)) / 3;
        }

        [NonAction]
        public double? FinalAccuracy(double? wer, double? mer, double? wil, double? sim, double? dist)
        {
            var newAccuracy = (4 - (wer + mer + wil + dist) + sim) / 5;
            return newAccuracy * 100;
        }

        public class RankingsRequestModel
        {
            public IndividualScenarioRequest[]? ScenarioList { get; set; }
            public int[]? WeightList { get; set; }
        }

        public class RankingsResponseModel
        {
            public string? SttName { get; set; }
            public double? TotalScore { get; set; }
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
