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
            Assert.IsNotNull(output);
        }

    }
}