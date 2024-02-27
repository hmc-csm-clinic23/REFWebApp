using Microsoft.AspNetCore.Mvc;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SttListController : ControllerBase
    {
        private static readonly string[] Stts = new[]
        {
            "OpenAI Whisper", "Google Chirp", "Meta MMS", "DeepGram", "PaddleSpeech", "Amazon Transcribe", "Microsoft Azure"
        };

        private readonly ILogger<SttListController> _logger;

        public SttListController(ILogger<SttListController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetSttList")]
        public IEnumerable<SttList> Get()
        {
            return Enumerable.Range(0, Stts.Length-1).Select(index => new SttList
            {
                Name = Stts[index]
            })
            .ToArray();
        }
    }
}
