using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class Scenario
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<SttAggregateMetric> SttAggregateMetrics { get; set; } = new List<SttAggregateMetric>();

    public virtual ICollection<AudioFile> Audios { get; set; } = new List<AudioFile>();
}
