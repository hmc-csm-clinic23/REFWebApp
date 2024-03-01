using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Model.STTs;
using REFWebApp.Server.Models;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SttController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SttController> _logger;

        public SttController(ILogger<SttController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostStt")]
        public IEnumerable<STT> Post([FromBody] SttRequestModel request)
        {
            //string[] text = request.Text;
            //GoogleCloud x = new GoogleCloud();
            //x.Run();
            //x.Metrics();
            using PostgresContext context = new PostgresContext();
            List<AudioFile> audios = context.AudioFiles.ToList();

            return Enumerable.Range(1, 5).Select(index => new STT
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Text = audios[index].Path
            })
    .ToArray();
        }

        public class SttRequestModel
        {
            public string[] Text { get; set; }
        }
    }
}
