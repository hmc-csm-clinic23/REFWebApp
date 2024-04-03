using Python.Runtime;

namespace REFWebApp.Server.Model.STTs
// export IRONPYTHONPATH=/Library/Frameworks/IronPython.framework/Versions/3.4.1/
{
    public class Gladia : ISTT
    {

        public Audio ProcessInput(Audio audio)
        {
            var mappedAudio = Mapper.MapGenericInput(audio);
            //throw new NotImplementedException();
            return audio;
        }

        public string Run(string filename)
        {
            string scriptname = "Gladia";
            //Runtime.PythonDLL = @"/Users/sathv/opt/anaconda3/lib/libpython3.9.dylib";
            // PythonEngine.Initialize();
            // Py.GIL();

            //string file = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/Gladia.py";
            // throw new NotImplementedException();

            if (!PythonEngine.IsInitialized)// Since using asp.net, we may need to re-initialize
            {
                Runtime.PythonDLL = @"C:\Users\micro\AppData\Local\Programs\Python\Python311\python311.dll";
                PythonEngine.Initialize();
                Py.GIL();
            }

            using (var scope = Py.CreateScope())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\micro\source\repos\REFWebApp\REFWebApp.Server\Evaluation\STTs\");
                //sys.path.append(@"/Users/sathv/Desktop/REFApplication/REFApplication/Model/STTs");

                //            // string code = File.ReadAllText(file); // Get the python file as raw text
                //            // var scriptCompiled = PythonEngine.Compile(code, file); 
                var scriptCompiled = Py.Import(scriptname);
                // string[] message = new string[] { "C:\\Users\\micro\\Desktop\\oldREF\\REFApplication\\REFApplication\\Model\\test.wav" };
                string message = filename;
                //string[] message = new string[] {"/Users/sathv/Desktop/REFApplication/REFApplication/Model/test.wav"};

                var result = scriptCompiled.InvokeMethod("transcribe_one", message.ToPython());
                Console.WriteLine("GLADIAOUTPUT: " + result);
                
                PyObject pyobject = result.AsManagedObject(typeof(PyObject)) as PyObject;

                string transcription = (string)pyobject.AsManagedObject(typeof(string));
                Console.WriteLine(transcription);
                return transcription;
            }

            Console.WriteLine("run works");
        }


        public  List<float> Metrics(string transcriptions, string groundtruths)
        {
            Evaluator y = new Evaluator();
            List<float> metricslist = y.Run(transcriptions, groundtruths);
            Console.WriteLine("Gladia metrics works : " + metricslist);
            return metricslist;
        }

        public string[] ProcessOutput(string[] args)
        {
            //var outputList = Mapper.MapGenericOutput(args);
            ////throw new NotImplementedException();
            return args;
        }
    }
}
