using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class SttAggregateMetric
{
    public int Id { get; set; }

    public int? ScenarioId { get; set; }

    public int? SttId { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Scenario? Scenario { get; set; }

    public virtual Stt? Stt { get; set; }
}
