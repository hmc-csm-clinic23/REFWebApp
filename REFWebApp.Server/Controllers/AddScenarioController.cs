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
               List<AudioFile?> files = new List<AudioFile?>();
               foreach(AudioFile audioFile in request.Audios) //Makes no sense why you need this for each, but it breaks if you don't have it (see below comment)
               {
                    files.Add(context.AudioFiles.Find(audioFile.Id));
               }
               Scenario scenario_object = new Scenario()
               {
                   Name = request.Name,
                   Audios = files //Breaks if you do request.Audios for some reason
               };
               context.Scenarios.Add(scenario_object);
               context.SaveChanges();
           }
        }

        public class AddScenarioRequestModel
        {
            public string? Name { get; set; }
            public List<AudioFile>? Audios { get; set; }
        }
    }
}
