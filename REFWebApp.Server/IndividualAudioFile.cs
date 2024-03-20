namespace REFWebApp.Server.Models;

public class IndividualAudioFile
{
    public int Id { get; set; }

    public string? Path { get; set; }

    public string? GroundTruth { get; set; }

    public int? FormatId { get; set; }

}