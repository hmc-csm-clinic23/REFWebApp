using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Model.STTs;
using REFWebApp.Server.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

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
            GoogleCloud x = new GoogleCloud();

            // for timing
            var stopwatch = new Stopwatch();
            var starting_time = Stopwatch.GetTimestamp();
            stopwatch.Start();

            // Run the STT with audio files
            List<string> transcriptions = x.Run(["C:\\Users\\micro\\source\\repos\\REFWebApp\\REFWebApp.Server\\Model\\test.wav"]);

            // get time for running the STT
            stopwatch.Stop();
            var elapsed_time = stopwatch.Elapsed.Seconds;
            Console.WriteLine("time taken: "+ elapsed_time.ToString());

            Console.WriteLine("from metrics controller: " + transcriptions[0]);
            // ground truths should be a list from the database for the specific audio files
            List<string> groundtruths = ["The colorful autumn leaves rustled in the gentle breeze as I took a leisurely stroll through the serene forest."];
            List<List<float>> metrics = x.Metrics(transcriptions, groundtruths);

            return Enumerable.Range(0, metrics.Count).Select(index => new MetricList
            {
                Metrics = metrics[index],
                Transcriptions = transcriptions[index]
            })
            .ToArray();
        }

        public class MetricsRequestModel
        {
            public string[] Text { get; set; }
        }
    }
}
