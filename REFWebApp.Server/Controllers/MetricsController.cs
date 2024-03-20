using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Model.STTs;
using REFWebApp.Server.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Text.Json;


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
        public IEnumerable<MetricList> Post([FromBody] MetricsRequestModel request)
        {
            List<string> scenarioNames = new List<string>();
            List<string> paths = new List<string>();
            List<string> groundTruths = new List<string>();

            for (int i = 0; i < request.ScenarioList?.Length; i++)
            {
                if (request.ScenarioList?[i].Name != null) 
                { 
                    scenarioNames.Add(request.ScenarioList?[i].Name);
                };
                for (int j = 0; j < request.ScenarioList?[i].Audios?.Count; j++)
                {
                    if (request.ScenarioList?[i].Audios?[j].Path != null && request.ScenarioList?[i].Audios?[j].GroundTruth != null)
                    {
                        paths.Add(request.ScenarioList?[i].Audios?[j].Path);
                        groundTruths.Add(request.ScenarioList?[i].Audios?[j].GroundTruth);
                    };
                }
            }
            for (int i = 0; i < request.SttList?.Length; i++)
            { 
            //Choose stts
            }
                GoogleCloud x = new GoogleCloud();
            var hi = request.SttList?[0].Name;
            var bye = request.ScenarioList?[0].Name;
            var cry = request.ScenarioList?[0].Audios?[0].Path;


            // for timing
            var stopwatch = new Stopwatch();
            var starting_time = new DateTime(Stopwatch.GetTimestamp());
            stopwatch.Start();

            //string[] audiofiles = ["C:\\Users\\micro\\source\\repos\\REFWebApp\\REFWebApp.Server\\Evaluation\\test.wav"];
            // Run the STT with audio files
            List<string> transcriptions = x.Run(paths.ToArray());

            // get time for running the STT
            stopwatch.Stop();
            var elapsed_time = stopwatch.Elapsed.Seconds;
            Console.WriteLine("time taken: "+ elapsed_time.ToString());

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

                }) ;
            }

            // Add transcriptions to database
            // this.AddTranscriptionsToDB(transcription_objects);


            // serialize into json format
            // string json_transcriptions = JsonSerializer.Serialize(transcriptions_class_list);

            // Console.WriteLine(json_transcriptions); 


            Console.WriteLine("from metrics controller: " + transcriptions[0]);
            // ground truths should be a list from the database for the specific audio files
            //List<string> groundtruths = ["The colorful autumn leaveks rustled in the gentle breeze as I took a leisurely stroll through the serene forest."];
            List<List<float>> metrics = x.Metrics(transcriptions, groundTruths);

            return Enumerable.Range(0, metrics.Count).Select(index => new MetricList
            {
                Metrics = metrics[index],
                Transcriptions = transcriptions[index]
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

        public class MetricsRequestModel
        {
            public IndividualStt[]? SttList { get; set; }
            public IndividualScenarioRequest[]? ScenarioList { get; set; }
        }
    }
}
