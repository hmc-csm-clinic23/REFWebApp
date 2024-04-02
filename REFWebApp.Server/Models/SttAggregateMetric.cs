using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class SttAggregateMetric
{
    public int Id { get; set; }

    public int? ScenarioId { get; set; }

    public int? SttId { get; set; }

    public DateTime? Timestamp { get; set; }

    public double? Wer { get; set; }

    public double? Mer { get; set; }

    public double? Wil { get; set; }

    public double? Sim { get; set; }

    public double? Dist { get; set; }

    public long? Rawtime { get; set; }

    public virtual Scenario? Scenario { get; set; }

    public virtual Stt? Stt { get; set; }
}
