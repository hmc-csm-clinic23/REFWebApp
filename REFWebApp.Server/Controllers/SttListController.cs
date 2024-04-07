using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Models;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SttListController : ControllerBase
    {

        private readonly ILogger<SttListController> _logger;

        public SttListController(ILogger<SttListController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetSttList")]
        public IEnumerable<IndividualStt> Get()
        {
            using PostgresContext context = new PostgresContext();
            List<Stt> stts = context.Stts.ToList();
            return Enumerable.Range(0, stts.Count).Select(index => new IndividualStt
            {
                Name = stts[index].Name,
                Id = stts[index].Id
            })
            .ToArray();
        }
    }
}
