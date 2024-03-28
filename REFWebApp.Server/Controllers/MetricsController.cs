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
                    case "Google Cloud":
                        ISTT GoogleCloud = new GoogleCloud();
                        refData.Add(runMetrics(request, GoogleCloud));
                        break;
                    case "Deepgram":
                        ISTT DeepGram = new DeepGram();
                        refData.Add(runMetrics(request, DeepGram));
                        break;
                    case "Amazon Transcribe":
                        ISTT AmazonTranscribe = new AmazonTranscribe();
                        refData.Add(runMetrics(request, AmazonTranscribe));
                        break;
                    case "Gladia":
                        ISTT Gladia = new Gladia();
                        refData.Add(runMetrics(request, Gladia));
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
        public MetricObject runMetrics(MetricsRequestModel request, ISTT STT)
        {
            List<string> scenarioNames = new List<string>();
            List<List<string>> paths = new List<List<string>>();
            List<List<string>> groundTruths = new List<List<string>>();

            for (int i = 0; i < request.ScenarioList?.Length; i++) // (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                if (request.ScenarioList?[i].Name != null)
                {
                    scenarioNames.Add(request.ScenarioList?[i].Name);
                    List<string> path = new List<string>();
                    List<string> groundTruth = new List<string>();

                    for (int j = 0; j < 5; j++) // (int j = 0; j < request.ScenarioList?[i].Audios?.Count; j++)
                    {
                        if (request.ScenarioList?[i].Audios?[j].Path != null && request.ScenarioList?[i].Audios?[j].GroundTruth != null)
                        {
                            path.Add(request.ScenarioList?[i].Audios?[j].Path);
                            groundTruth.Add(request.ScenarioList?[i].Audios?[j].GroundTruth);
                        };
                    }

                    paths.Add(path);
                    groundTruths.Add(groundTruth);
                };
            }

            List<List<string>> transcriptions = new List<List<string>>();

            for (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                // for timing
                var stopwatch = new Stopwatch();
                var starting_time = new DateTime(Stopwatch.GetTimestamp());
                stopwatch.Start();

                //string[] audiofiles = ["C:\\Users\\micro\\source\\repos\\REFWebApp\\REFWebApp.Server\\Evaluation\\test.wav"];
                // Run the STT with audio files
                transcriptions.Add(STT.Run(paths[i].ToArray()));

                // get time for running the STT
                stopwatch.Stop();
                var elapsed_time = stopwatch.Elapsed.Seconds;
                Console.WriteLine("time taken: " + elapsed_time.ToString());
            }

            /* use time now for this one and start and stop for the invidual scenario speeds above
            // put transcription info into Json format
            List<Transcription> transcription_objects = new List<Transcription>();
            for (int i = 0; i < transcriptions.Count; i++)
            {
                transcription_objects.Add(new Transcription
                {
                    Timestamp = starting_time,
                    Transcript = transcriptions[i],
                    AudioId = i,
                    SttId = null,

                });
            }*/

            Console.WriteLine("from metrics controller: " + transcriptions[0][0]);
            // ground truths should be a list from the database for the specific audio files
            //List<string> groundtruths = ["The colorful autumn leaveks rustled in the gentle breeze as I took a leisurely stroll through the serene forest."];
            List<List<List<float>>> metrics = new List<List<List<float>>>();

            for (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                metrics.Add(STT.Metrics(transcriptions[i], groundTruths[i]));
            }

            Console.WriteLine("metrics: " + metrics);

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
