using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Models;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AudioListController : ControllerBase
    {

        private readonly ILogger<AddScenarioController> _logger;

        public AudioListController(ILogger<AddScenarioController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAudio")]
        public AudioFile[] Get()
        {
            using PostgresContext context = new PostgresContext();
            List<AudioFile> audios = context.AudioFiles.ToList();
            return audios.ToArray();
        }
    }
}
