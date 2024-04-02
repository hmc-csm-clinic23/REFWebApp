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
            List<MetricObject> refData = new List<MetricObject>();
            for (int i = 0; i < request.SttList?.Length; i++)
            {
                //Choose stts
                switch (request.SttList?[i].Name)
                {
                    //case "Amazon Transcribe":
                    //    ISTT AmazonTranscribe = new AmazonTranscribe();
                    //    refData.Add(runMetrics(request, AmazonTranscribe, 1));
                    //    break;
                    //case "Google Cloud":
                    //    ISTT GoogleCloud = new GoogleCloud();
                    //    refData.Add(runMetrics(request, GoogleCloud, 2));
                    //    break;
                    //case "Deepgram":
                    //    ISTT DeepGram = new DeepGram();
                    //    refData.Add(runMetrics(request, DeepGram, 3));
                    //    break;
                    //case "Whisper":
                    //    ISTT Whisper = new Whisper();
                    //    refData.Add(runMetrics(request, Whisper, 4));
                    //    break;
                    case "Gladia":
                        ISTT Gladia = new Gladia();
                        refData.Add(runMetrics(request, Gladia, 5));
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
        public MetricObject runMetrics(MetricsRequestModel request, ISTT STT, int sttId)
        {
            List<string?> scenarioNames = new List<string?>();
            List<List<string?>> paths = new List<List<string?>>();
            List<List<string?>> groundTruths = new List<List<string?>>();
            List<List<string?>> transcriptions = new List<List<string?>>();
            List<List<List<float>>> metrics = new List<List<List<float>>>();
            DateTime timestamp = DateTime.Now;

            for (int i = 0; i < request.ScenarioList?.Length; i++) // (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                if (request.ScenarioList?[i].Name != null)
                {
                    scenarioNames.Add(request.ScenarioList?[i].Name);
                    List<string?> path = new List<string?>();
                    List<string?> groundTruth = new List<string?>();
                    List<string?> transcription = new List<string?>();
                    List<List<float>> metric = new List<List<float>>();
                    List<string> elapsed_times = new List<string>();

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
                            TimeSpan ts = stopwatch.Elapsed;
                            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                            elapsed_times.Add(elapsedTime);
                            Console.WriteLine("time taken: " + elapsedTime);

                            List<float> metricResult = STT.Metrics(runResult, request.ScenarioList?[i].Audios?[j].GroundTruth);

                            metric.Add(metricResult);
                            transcription.Add(runResult);
                            
                            /*List<Transcription> transcription_objects = new List<Transcription>();
                            transcription_objects.Add(new Transcription
                            {
                                Timestamp = timestamp,
                                Transcript = string.Join("", runResult),
                                AudioId = request.ScenarioList?[i].Audios?[j].Id,
                                SttId = sttId,
                                //wer,
                                //mer,
                                //wil,
                                //wip,
                                //cer,
                                //Rawtime = elapsedTime,
                            });*/
                        };
                    }

                    paths.Add(path);
                    groundTruths.Add(groundTruth);
                    transcriptions.Add(transcription);
                    metrics.Add(metric);

                    /*List<SttAggregateMetric> aggregate = new List<SttAggregateMetric>();
                    aggregate.Add(new SttAggregateMetric
                    {
                        ScenarioId = 0,
                        SttId = sttId,
                        Timestamp = timestamp,
                        //wer = sum(metric) / Metrics.length,
                        //mer,
                        //wil,
                        //wip,
                        //cer,
                        //Rawtime = 0,
                    });*/
                }
            }
            // Adding to the Database???
            //using (var context = new PostgresContext())
            //{
            //    context.BulkInsert(transcription_objects);
            //}
            // ground truths should be a list from the database for the specific audio files
            //List<string> groundtruths = ["The colorful autumn leaveks rustled in the gentle breeze as I took a leisurely stroll through the serene forest."];

            return new MetricObject
            {
                Metrics = metrics,
                Transcriptions = transcriptions,
                GroundTruths = groundTruths
            };

        }

        public class MetricObject
        {
            public List<List<List<float>>>? Metrics { get; set; }
            public List<List<string>>? Transcriptions { get; set; }
            public List<List<string>>? GroundTruths { get; set; }
        }

        public class MetricsRequestModel
        {
            public IndividualStt[]? SttList { get; set; }
            public IndividualScenarioRequest[]? ScenarioList { get; set; }
        }

        public class MetricResponseModel
        {
            public string? SttName { get; set; }
            public MetricObject? RefData { get; set; }
        }
    }
}
