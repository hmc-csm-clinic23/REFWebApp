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
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static REFWebApp.Server.Controllers.RankingsController;


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
                        Rawtime = elapsedTimes.Aggregate(new TimeSpan(0, 0, 0, 0), (x,y) => x+y),
                    });

                    MetricObject metric = new MetricObject
                    {
                        Weight = request.WeightList[i] / weightsAverage,
                        Wer = wer,
                        Mer = mer,
                        Wil = wil,
                        Sim = sim,
                        Dist = dist,
                        Rawtime = elapsedTimes,
                    };
                    metrics.Add(metric);
                }
            }
            
            using (var context = new PostgresContext())
            {
                context.BulkInsert(transcription_objects);
                context.BulkInsert(aggregate);
            }

            //return new MetricObject
            //{
            //    Metrics = metrics,
            //    Transcriptions = transcriptions,
            //    GroundTruths = groundTruths
            //};

            return Enumerable.Range(0, metrics.Count).Select(index => new runMetricsObject
            {
                TotalScore = metrics[index].Wer, //this should be a complex line or function that combines our metrics, speed, usability, etc.
                Accuracy = metrics[index].Wer,
                Speed = metrics[index].Rawtime,
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
            public List<double>? TotalScore { get; set; }
            public List<double>? Accuracy { get; set; }
            public List<TimeSpan>? Speed { get; set; }
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
