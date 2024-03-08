namespace REFWebApp.Server.Model
{
    public interface ISTT
    {
        Audio ProcessInput(Audio audio);
        public void Run(string[] filenames);
        void Metrics();
        string[] ProcessOutput(string[] args);
    }
}
