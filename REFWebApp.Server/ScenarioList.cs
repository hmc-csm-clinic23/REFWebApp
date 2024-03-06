using REFWebApp.Server.Models;

namespace REFWebApp.Server
{
    public class ScenarioList
    {
        public string? Name { get; set; }
        public virtual ICollection<AudioFile> Audios { get; set; } = new List<AudioFile>();
    }
}