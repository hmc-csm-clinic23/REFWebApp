using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class AudioFile
{
    public int Id { get; set; }

    public string? Path { get; set; }

    public string? GroundTruth { get; set; }

    public int? FormatId { get; set; }

    public virtual FileFormat? Format { get; set; }

    public virtual ICollection<Transcription> Transcriptions { get; set; } = new List<Transcription>();

    public virtual ICollection<Scenario> Scenarios { get; set; } = new List<Scenario>();
}
