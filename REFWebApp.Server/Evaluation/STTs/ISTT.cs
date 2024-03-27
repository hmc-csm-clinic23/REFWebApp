namespace REFWebApp.Server.Model
{
    public interface ISTT
    {
        Audio ProcessInput(Audio audio);
        public List<string> Run(string[] filenames);
        List<List<float>> Metrics(List<string> transcriptions, List<string> groundtruths);
        string[] ProcessOutput(string[] args);
    }
}
