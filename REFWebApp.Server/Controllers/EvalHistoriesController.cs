using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Data;
using REFWebApp.Server.Model.STTs;
using REFWebApp.Server.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Text.Json;
using System.IO;
using System;
using Microsoft.Identity.Client;
using REFWebApp.Server.Model;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.ConstrainedExecution;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace REFWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EvalHistoriesController : ControllerBase
    {

        private readonly ILogger<EvalHistoriesController> _logger;

        public EvalHistoriesController(ILogger<EvalHistoriesController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostEvalHistories")]
        public IEnumerable<EvalHistoriesResponseModel> Post([FromBody] EvalHistoriesRequestModel request)
        {
            using PostgresContext context = new PostgresContext();
            Stt stt = context.Stts.Where(s => s.Name == request.SttName).ToList()[0];
            Scenario scenario = context.Scenarios.Where(s => s.Name == request.ScenarioName)
                                                 .Include(s => s.Audios)
                                                 .ToList()[0];
            List<int> audioIdsByScenario = scenario.Audios.Select(a => a.Id).ToList();
            List<Transcription> transcription = context.Transcriptions.Where(t => t.SttId == stt.Id)
                                                            .Where(t => t.Timestamp == request.Timestamp)
                                                            .Where(t => audioIdsByScenario.Contains(t.AudioId.Value))
                                                            .ToList();
            return Enumerable.Range(0, transcription.Count).Select(index => new EvalHistoriesResponseModel
            {
                Path = context.AudioFiles.Find(transcription[index].AudioId)?.Path,
                GroundTruth = context.AudioFiles.Find(transcription[index].AudioId)?.GroundTruth,
                Transcription = transcription[index].Transcript,
                Wer = transcription[index].Wer,
                Mer = transcription[index].Mer,
                Wil = transcription[index].Wil,
                Sim = transcription[index].Sim,
                Dist = transcription[index].Dist
            })
            .ToArray();
        }

        public class EvalHistoriesRequestModel
        {
            public string? SttName { get; set; }
            public string? ScenarioName { get; set; }
            public DateTime? Timestamp { get; set; }
        }

        public class EvalHistoriesResponseModel
        {
            public string? Path { get; set; }
            public string? GroundTruth { get; set; }
            public string? Transcription { get; set; }
            public double? Wer { get; set; }
            public double? Mer { get; set; }
            public double? Wil { get; set; }
            public double? Sim { get; set; }
            public double? Dist { get; set; }
        }
    }
}
