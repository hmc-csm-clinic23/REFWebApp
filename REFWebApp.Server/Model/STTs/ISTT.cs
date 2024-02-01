namespace REFWebApp.Server.Model
{
    public interface ISTT
    {
        Audio ProcessInput(Audio audio);
        void Run();
        void Metrics();
        string[] ProcessOutput(string[] args);
    }
}
