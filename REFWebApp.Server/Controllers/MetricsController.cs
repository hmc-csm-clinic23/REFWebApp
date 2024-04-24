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
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static REFWebApp.Server.Controllers.RankingsController;
using System.Net.NetworkInformation;


namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetricsController : ControllerBase
    {

        private readonly ILogger<MetricsController> _logger;

        public MetricsController(ILogger<MetricsController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostMetrics")]
        public IEnumerable<MetricResponseModel> Post([FromBody] MetricsRequestModel request)
        {
            List<IEnumerable<runMetricsObject>> refData = new List<IEnumerable<runMetricsObject>>();
            for (int i = 0; i < request.SttList?.Length; i++)
            {
                //Choose stts
                switch (request.SttList?[i].Name)
                {
                    //case "Amazon Transcribe":
                    //    ISTT AmazonTranscribe = new AmazonTranscribe();
                    //    refData.Add(runMetrics(request, AmazonTranscribe, request.SttList?[i].Id));
                    //    break;
                    case "Google Cloud":
                        ISTT GoogleCloud = new GoogleCloud();
                        refData.Add(runMetrics(request, GoogleCloud, request.SttList?[i].Id));
                        break;
                    case "Deepgram":
                        ISTT DeepGram = new DeepGram();
                        refData.Add(runMetrics(request, DeepGram, request.SttList?[i].Id));
                        break;
                    case "Whisper":
                        ISTT Whisper = new Whisper();
                        refData.Add(runMetrics(request, Whisper, request.SttList?[i].Id));
                        break;
                    case "Gladia":
                        ISTT Gladia = new Gladia();
                        refData.Add(runMetrics(request, Gladia, request.SttList?[i].Id));
                        break;
                    default:
                        break;
                }
            }
            return Enumerable.Range(0, (int)request.SttList?.Length).Select(index => new MetricResponseModel
            {
                SttName = request.SttList?[index].Name,
                RefData = refData[index]
            })
            .ToArray();
        }

        [NonAction]
        public void AddTranscriptionsToDB(List<Transcription> transcriptions) { 
            using (var context = new PostgresContext())
            {
                context.BulkInsert(transcriptions);
            }
        }

        [NonAction]
        public IEnumerable<runMetricsObject> runMetrics(MetricsRequestModel request, ISTT STT, int? sttId)
        {
            List<string?> scenarioNames = new List<string?>();
            List<List<string?>> paths = new List<List<string?>>();
            List<List<string?>> groundTruths = new List<List<string?>>();
            List<List<string?>> transcriptions = new List<List<string?>>();
            List<MetricObject> metrics = new List<MetricObject>();
            List<SttAggregateMetric> weightedMetrics = new List<SttAggregateMetric>();
            List<Transcription> transcription_objects = new List<Transcription>();
            List<SttAggregateMetric> aggregate = new List<SttAggregateMetric>();
            DateTime timestamp = DateTime.Now;
            float weightsAverage = request.WeightList.Sum();

            for (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                if (request.ScenarioList?[i].Name != null)
                {
                    scenarioNames.Add(request.ScenarioList?[i].Name);
                    List<string?> path = new List<string?>();
                    List<string?> groundTruth = new List<string?>();
                    List<string?> transcription = new List<string?>();
                    List<TimeSpan> elapsedTimes = new List<TimeSpan>();
                    List<double> wer = new List<double>();
                    List<double> mer = new List<double>();
                    List<double> wil = new List<double>();
                    List<double> sim = new List<double>();
                    List<double> dist = new List<double>();


                    for (int j = 0; j < 5; j++) // (int j = 0; j < request.ScenarioList?[i].Audios?.Count; j++)
                    {
                        if (request.ScenarioList?[i].Audios?[j].Path != null && request.ScenarioList?[i].Audios?[j].GroundTruth != null)
                        {
                            path.Add(request.ScenarioList?[i].Audios?[j].Path);
                            groundTruth.Add(request.ScenarioList?[i].Audios?[j].GroundTruth);

                            var stopwatch = new Stopwatch();
                            stopwatch.Start();

                            string runResult = STT.Run(request.ScenarioList?[i].Audios?[j].Path);

                            stopwatch.Stop();
                            TimeSpan elapsedTime = stopwatch.Elapsed;
                            string timePrint = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds / 10);
                            elapsedTimes.Add(elapsedTime);
                            Console.WriteLine("time taken: " + timePrint);

                            List<float> metricResult = STT.Metrics(runResult, request.ScenarioList?[i].Audios?[j].GroundTruth);

                            transcription.Add(runResult);

                            wer.Add(metricResult[0]);
                            mer.Add(metricResult[1]);
                            wil.Add(metricResult[2]);
                            sim.Add(metricResult[3]);
                            dist.Add(metricResult[4]);

                            transcription_objects.Add(new Transcription
                            {
                                Timestamp = timestamp,
                                Transcript = string.Join("", runResult),
                                AudioId = request.ScenarioList?[i].Audios?[j].Id,
                                SttId = sttId,
                                Wer = metricResult[0],
                                Mer = metricResult[1],
                                Wil = metricResult[2],
                                Sim = metricResult[3],
                                Dist = metricResult[4],
                                Rawtime = elapsedTime,
                            });
                        };
                    }
                    if (groundTruth.Count == 0)
                    {
                        continue;
                    }

                    paths.Add(path);
                    groundTruths.Add(groundTruth);
                    transcriptions.Add(transcription);

                    aggregate.Add(new SttAggregateMetric
                    {
                        ScenarioId = request.ScenarioList?[i].Id,
                        SttId = sttId,
                        Timestamp = timestamp,
                        Wer = wer.Sum() / wer.Count,
                        Mer = mer.Sum() / mer.Count,
                        Wil = wil.Sum() / wil.Count,
                        Sim = sim.Sum() / sim.Count,
                        Dist = dist.Sum() / dist.Count,
                        Rawtime = elapsedTimes.Aggregate(new TimeSpan(0, 0, 0, 0), (x, y) => x + y),
                    });

                    MetricObject metric = new MetricObject
                    {
                        Wer = wer,
                        Mer = mer,
                        Wil = wil,
                        Sim = sim,
                        Dist = dist,
                        Rawtime = elapsedTimes,
                    };
                    metrics.Add(metric);
                    SttAggregateMetric weightedMetric = new SttAggregateMetric
                    {
                        Wer = wer.Select(val => val * request.WeightList[i] / weightsAverage).Sum() / wer.Count,
                        Mer = mer.Select(val => val * request.WeightList[i] / weightsAverage).Sum() / mer.Count,
                        Wil = wil.Select(val => val * request.WeightList[i] / weightsAverage).Sum() / wil.Count,
                        Sim = sim.Select(val => val * request.WeightList[i] / weightsAverage).Sum() / sim.Count,
                        Dist = dist.Select(val => val * request.WeightList[i] / weightsAverage).Sum() / dist.Count,
                        Rawtime = elapsedTimes.Select(val => val * request.WeightList[i] / weightsAverage).Aggregate(new TimeSpan(0, 0, 0, 0), (x, y) => x + y),
                    };
                    weightedMetrics.Add(weightedMetric);
                }
            }
            
            using (var context = new PostgresContext())
            {
                context.BulkInsert(transcription_objects);
                context.BulkInsert(aggregate);
            }
            using PostgresContext newContext = new PostgresContext();
            //return new MetricObject
            //{
            //    Metrics = metrics,
            //    Transcriptions = transcriptions,
            //    GroundTruths = groundTruths
            //};

            return Enumerable.Range(0, metrics.Count).Select(index => new runMetricsObject
            {
                TotalScore = FinalScore(FinalAccuracy(weightedMetrics[index].Wer * request.ScenarioList?.Length, weightedMetrics[index].Mer * request.ScenarioList?.Length, weightedMetrics[index].Wil * request.ScenarioList?.Length, weightedMetrics[index].Sim * request.ScenarioList?.Length, weightedMetrics[index].Dist * request.ScenarioList?.Length), TimeSpan.FromSeconds((Math.Round(((TimeSpan)weightedMetrics[index].Rawtime).TotalSeconds) * (double)(request.ScenarioList?.Length))), newContext.Stts.Find(sttId).Usability),
                Accuracy = FinalAccuracy(weightedMetrics[index].Wer * request.ScenarioList?.Length, weightedMetrics[index].Mer * request.ScenarioList?.Length, weightedMetrics[index].Wil * request.ScenarioList?.Length, weightedMetrics[index].Sim * request.ScenarioList?.Length, weightedMetrics[index].Dist * request.ScenarioList?.Length),
                Speed = Math.Round(((TimeSpan)weightedMetrics[index].Rawtime).TotalSeconds) * (double)(request.ScenarioList?.Length),
                Wer = metrics[index].Wer,
                Mer = metrics[index].Mer,
                Wil = metrics[index].Wil,
                Sim = metrics[index].Sim,
                Dist = metrics[index].Dist,
                Transcriptions = transcriptions[index],
                GroundTruths = groundTruths[index]
            })
            .ToArray();

        }

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

        public class MetricObject
        {
            public double? Weight { get; set; }
            public List<double>? Wer { get; set; }
            public List<double>? Mer { get; set; }
            public List<double>? Wil { get; set; }
            public List<double>? Sim { get; set; }
            public List<double>? Dist { get; set; }
            public List<TimeSpan>? Rawtime { get; set; }
        }

        public class runMetricsObject
        {
            public double? TotalScore { get; set; }
            public double? Accuracy { get; set; }
            public double? Speed { get; set; }
            public List<double>? Wer { get; set; }
            public List<double>? Mer { get; set; }
            public List<double>? Wil { get; set; }
            public List<double>? Sim { get; set; }
            public List<double>? Dist { get; set; }
            public List<string>? Transcriptions { get; set; }
            public List<string>? GroundTruths { get; set; }
        }

        public class MetricsRequestModel
        {
            public IndividualStt[]? SttList { get; set; }
            public IndividualScenarioRequest[]? ScenarioList { get; set; }
            public int[]? WeightList { get; set; }
        }

        public class MetricResponseModel
        {
            public string? SttName { get; set; }
            public IEnumerable<runMetricsObject>? RefData { get; set; }
        }
    }
}
