namespace REFWebApp.Server
{
<<<<<<< HEAD
    public class Stt
=======
    public class STT
>>>>>>> 041b6b9f72f4b7ff6933e63b094466a41cb28a7c
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
        
        public string? Text { get; set; }
    }
}
