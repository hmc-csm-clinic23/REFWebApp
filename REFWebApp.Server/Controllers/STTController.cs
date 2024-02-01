using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Model;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class STTController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<STTController> _logger;

        public STTController(ILogger<STTController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostSTT")]
public IEnumerable<STT> Post([FromBody] STTRequestModel request)
{
    string[] text = request.Text;
    GoogleCloud x = new GoogleCloud();
    x.Run();

    return Enumerable.Range(1, 5).Select(index => new STT
    {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        Text = text[index]
    })
    .ToArray();
}

public class STTRequestModel
{
    public string[] Text { get; set; }
}
    }
}
