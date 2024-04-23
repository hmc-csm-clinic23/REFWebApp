using REFWebApp.Server.Model;
using REFWebApp.Server.Model.STTs;

namespace REFTests
{
    public class Tests
    {
        private ISTT _googleCloud { get; set; } = null!;
        private ISTT _deepgram { get; set; } = null!;
        
        //private ISTT _amazonTranscribe { get; set; } = null!;
        private ISTT _gladia { get; set; } = null!;
        private ISTT _whisper { get; set; } = null!;

        [SetUp]
        public void Setup()
        { 
            _googleCloud = new GoogleCloud();
            _deepgram = new DeepGram();
            //_amazonTranscribe = new AmazonTranscribe();
            _gladia = new Gladia();
            _whisper = new Whisper();

        }

        [Test]
        public void TestGoogleCloud()
        {
            var output = _googleCloud.Run("C:\\Users\\micro\\source\\repos\\REFWebApp\\REFTests\\REFTest.wav");
            var metrics = _googleCloud.Metrics(output, "Alright. So I I can take that low five back on the PJT.");
            Assert.IsNotNull(metrics);
            Assert.That(metrics.Count, Is.EqualTo(5));
            Assert.IsNotNull(output);
        }

        [Test]
        public void TestDeepGram()
        {
            var output = _deepgram.Run("C:\\Users\\micro\\source\\repos\\REFWebApp\\REFTests\\REFTest.wav");
            var metrics = _deepgram.Metrics(output, "Alright. So I I can take that low five back on the PJT.");
            Assert.IsNotNull(output);
        }

        [Test]
        public void TestGladia()
        {
            var output = _gladia.Run("C:\\Users\\micro\\source\\repos\\REFWebApp\\REFTests\\REFTest.wav");
            var metrics = _gladia.Metrics(output, "Alright. So I I can take that low five back on the PJT.");
            Assert.IsNotNull(output);
        }

        [Test]
        public void TestWhisper()
        {
            var output = _whisper.Run("C:\\Users\\micro\\source\\repos\\REFWebApp\\REFTests\\REFTest.wav");
            var metrics = _whisper.Metrics(output, "Alright. So I I can take that low five back on the PJT.");
            Assert.IsNotNull(output);
        }

    }
}