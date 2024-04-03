using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class Stt
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Usability { get; set; }

    public virtual ICollection<SttAggregateMetric> SttAggregateMetrics { get; set; } = new List<SttAggregateMetric>();

    public virtual ICollection<Transcription> Transcriptions { get; set; } = new List<Transcription>();
}
