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
            List<Scenario> scenarios = context.Scenarios.ToList();//.Include(s => s.Audios).ToList();


            //using PostgresContext context = new PostgresContext();
            //List<Scenario> scenarios = context.Scenarios.ToList();

            /*
             return Enumerable.Range(0, scenarios.Count).Select(index => new IndividualScenario
            {
                Name = scenarios[index].Name,
                Audios = Enumerable.Range(0, scenarios.Count).Select(i => new IndividualAudioFile
                {
                    Id = context.AudioFiles.FromSql($"SELECT id FROM audio_files WHERE id IN (SELECT audio_id FROM audio_scenarios where scenario_id = {i + 1})").ToList();
                    Path = context.AudioFiles.FromSql($"SELECT path FROM audio_files WHERE id IN (SELECT audio_id FROM audio_scenarios where scenario_id = {i + 1})").ToList();
                    GroundTruth= context.AudioFiles.FromSql($"SELECT ground_truth FROM audio_files WHERE id IN (SELECT audio_id FROM audio_scenarios where scenario_id = {i + 1})").ToList();
                    FormatId = context.AudioFiles.FromSql($"SELECT format_id FROM audio_files WHERE id IN (SELECT audio_id FROM audio_scenarios where scenario_id = {i + 1})").ToList();
                }
            })
            .ToArray();
             */

            return Enumerable.Range(0, scenarios.Count).Select(index => new IndividualScenario
            {
                Name = scenarios[index].Name,
                Id = scenarios[index].Id,
                Audios = context.AudioFiles.FromSql($"SELECT * FROM audio_files WHERE id IN (SELECT audio_id FROM audio_scenarios where scenario_id = {index+1})").ToList()
            })
            .ToArray();
        }

    }
}
