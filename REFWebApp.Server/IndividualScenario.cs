using REFWebApp.Server.Models;

namespace REFWebApp.Server
{
    public class IndividualScenario
    {
        public string? Name { get; set; }
        public int? Id { get; set; }
        public List<AudioFile>? Audios { get; set; }
    }
}