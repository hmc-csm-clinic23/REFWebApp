using Microsoft.AspNetCore.Mvc;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddScenarioController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AddScenarioController> _logger;

        public AddScenarioController(ILogger<AddScenarioController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostScenario")]
        public void Post([FromBody] AddScenarioRequestModel request)
        {
        }

        public class AddScenarioRequestModel
        {
            public string Name { get; set; }
            public string[] Audios { get; set; }
        }
    }
}
