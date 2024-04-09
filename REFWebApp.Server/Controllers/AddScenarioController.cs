using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Models;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddScenarioController : ControllerBase
    {

        private readonly ILogger<AddScenarioController> _logger;

        public AddScenarioController(ILogger<AddScenarioController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostScenario")]
        public void Post([FromBody] AddScenarioRequestModel request)
        {
           using (var context = new PostgresContext())
           {
               Scenario scenario_object = new Scenario
               {
                   Name = request.Name,
                   Audios = request.Audios,
               };
               context.Scenarios.Add(scenario_object);
               context.SaveChanges();
           }
        }

        public class AddScenarioRequestModel
        {
            public string? Name { get; set; }
            public AudioFile[]? Audios { get; set; }
        }
    }
}
