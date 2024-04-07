using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Models;
using System.Collections.Generic;
using static REFWebApp.Server.Controllers.HistoriesController;

namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EvalListController : ControllerBase
    {

        private readonly ILogger<EvalListController> _logger;

        public EvalListController(ILogger<EvalListController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostEvalList")]
        public List<DateTime?> Post([FromBody] EvalRequestModel request)
        {
            using PostgresContext context = new PostgresContext();
            List<Stt> stts = context.Stts.Where(s => s.Name == request.SttName).ToList();
            Stt stt = stts[0];
            List<Scenario> scenarios = context.Scenarios.Where(s => s.Name == request.ScenarioName).ToList();
            Scenario scenario = scenarios[0];
            List<DateTime?> timestamps = context.SttAggregateMetrics.Where(t => t.SttId == stt.Id)
                                                            .Where(t => t.ScenarioId == scenario.Id)
                                                            .Select(t => t.Timestamp)
                                                            .Distinct()
                                                            .ToList();
            return timestamps;
        }

        public class EvalRequestModel
        {
            public string? SttName { get; set; }
            public string? ScenarioName { get; set; }
        }

        public class EvalResponseModel
        {
            public int? EvalIndex { get; set; }
            public List<DateTime?>? Timestamps { get; set; }
        }
    }
}
