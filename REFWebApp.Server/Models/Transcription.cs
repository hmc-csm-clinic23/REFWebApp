using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class Transcription
{
    public int Id { get; set; }

    public int AudioId { get; set; }

    public int SttId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Transcript { get; set; }

    public double? Sim { get; set; }

    public double? Mer { get; set; }

    public double? Wil { get; set; }

    public double? Wer { get; set; }

    public double? Dist { get; set; }

    public TimeSpan? Rawtime { get; set; }

    public virtual AudioFile? Audio { get; set; }

    public virtual Stt? Stt { get; set; }
}
