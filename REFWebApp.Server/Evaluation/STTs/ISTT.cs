namespace REFWebApp.Server.Model
{
    public interface ISTT
    {
        Audio ProcessInput(Audio audio);
        public string Run(string filenames);
        List<float> Metrics(string transcriptions, string groundtruths);
        string[] ProcessOutput(string[] args);
    }
}
