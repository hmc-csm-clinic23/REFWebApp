using REFWebApp.Server.Models;

namespace REFWebApp.Server
{
    public class IndividualScenarioRequest
    {
        public string? Name { get; set; }
        public int? Id { get; set; }
        public List<IndividualAudioFile>? Audios { get; set; }
    }
}
