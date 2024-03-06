using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Models;
using System.Xml.Linq;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScenarioListController : ControllerBase
    {

        private readonly ILogger<ScenarioListController> _logger;

        public ScenarioListController(ILogger<ScenarioListController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetScenarioList")]
        public IEnumerable<ScenarioList> Get()
        {
            using PostgresContext context = new PostgresContext();
            List<Scenario> scenarios = context.Scenarios.ToList();
            return Enumerable.Range(0, scenarios.Count).Select(index => new ScenarioList
            {
                Name = scenarios[index].Name,
                Audios = scenarios[index].Audios
            })
            .ToArray();
        }
    }
}
