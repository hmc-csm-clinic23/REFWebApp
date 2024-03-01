using System;
using System.Collections.Generic;

namespace REFWebApp.Server.Models;

public partial class FileFormat
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<AudioFile> AudioFiles { get; set; } = new List<AudioFile>();
}
