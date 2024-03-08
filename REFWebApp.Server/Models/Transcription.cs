using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class Transcription
{
    public int Id { get; set; }

    public int? AudioId { get; set; }

    public int? SttId { get; set; }

    public DateTime? Timestamp { get; set; }
    
    public virtual String? Transcript { get; set; }

    public virtual AudioFile? Audio { get; set; }

    public virtual Stt? Stt { get; set; }

}
