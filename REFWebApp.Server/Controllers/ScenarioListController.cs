using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<IndividualScenario> Get()
        {
            using PostgresContext context = new PostgresContext();
            List<Scenario> scenarios = context.Scenarios.ToList();

            return Enumerable.Range(0, scenarios.Count).Select(index => new IndividualScenario
            {
                Name = scenarios[index].Name,
                Id = scenarios[index].Id,
                Audios = context.AudioFiles.FromSql($"SELECT * FROM audio_files WHERE id IN (SELECT audio_id FROM audio_scenarios where scenario_id = {scenarios[index].Id})").ToList()
            })
            .ToArray();
        }

    }
}
