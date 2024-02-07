using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScenarioListController : ControllerBase
    {
        private static readonly string[] Scenarios = new[]
        {
            "Loud", "Quiet", "Noisy", "Sparse", "Windy", "Space", "Clear"
        };

        private readonly ILogger<ScenarioListController> _logger;

        public ScenarioListController(ILogger<ScenarioListController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetScenarioList")]
        public IEnumerable<ScenarioList> Get()
        {
            return Enumerable.Range(0, Scenarios.Length - 1).Select(index => new ScenarioList
            {
                Name = Scenarios[index]
            })
            .ToArray();
        }
    }
}
