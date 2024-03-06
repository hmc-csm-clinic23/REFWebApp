using Python.Runtime;

namespace REFWebApp.Server.Model.STTs
// export IRONPYTHONPATH=/Library/Frameworks/IronPython.framework/Versions/3.4.1/
{
    public class GoogleCloud
    {

        public Audio ProcessInput(Audio audio)
        {
            var mappedAudio = Mapper.MapGenericInput(audio);
            //throw new NotImplementedException();
            return audio;
        }

        public List<string> Run(string[] filenames)
        {
            string scriptname = "GoogleCloud";
            //Runtime.PythonDLL = @"/Users/sathv/opt/anaconda3/lib/libpython3.9.dylib";
            Runtime.PythonDLL = @"C:\Users\micro\AppData\Local\Programs\Python\Python39\python39.dll";
            PythonEngine.Initialize();
            Py.GIL();

            //string file = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/GoogleCloud.py";
            // throw new NotImplementedException();

            if (!PythonEngine.IsInitialized)// Since using asp.net, we may need to re-initialize
            {
                PythonEngine.Initialize();
                Py.GIL();
            }

            using (var scope = Py.CreateScope())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\micro\Desktop\oldREF\REFApplication\REFApplication\Model\STTs");
                //sys.path.append(@"/Users/sathv/Desktop/REFApplication/REFApplication/Model/STTs");

                //            // string code = File.ReadAllText(file); // Get the python file as raw text
                //            // var scriptCompiled = PythonEngine.Compile(code, file); 
                var scriptCompiled = Py.Import(scriptname);
                // string[] message = new string[] { "C:\\Users\\micro\\Desktop\\oldREF\\REFApplication\\REFApplication\\Model\\test.wav" };
                string[] message = filenames;
                //string[] message = new string[] {"/Users/sathv/Desktop/REFApplication/REFApplication/Model/test.wav"};

                var result = scriptCompiled.InvokeMethod("transcribe_all", message.ToPython());
                Console.WriteLine("GOOGLECLOUD_OUTPUT: " + result);

                PyObject[] pylist = result.AsManagedObject(typeof(PyObject[])) as PyObject[];

                List<string> transcriptions = new List<string>();

                foreach (PyObject pyobject in pylist)
                {
                    string transcript = (string)pyobject.AsManagedObject(typeof(string));
                    transcriptions.Add(transcript);

                }
                //Console.WriteLine(transcriptions); 
                return transcriptions;
                //scriptCompiled.InvokeMethod("returndict");
                //                                    // PyObject Pythonnet = scope.Get("Pythonnet"); // Lets get an instance of the class in python
                //                                    // PyObject pythongReturn = Pythonnet.InvokeMethod("returndict"); // Call the sayHello function on the exampleclass object
                //                                    // string? result = pythongReturn.AsManagedObject(typeof(string)) as string; // convert the returned string to managed string object
                //                                    // //Console.WriteLine(result)
            }
            Console.WriteLine("run works");
        }


        public  List<List<float>> Metrics(List<string> transcriptions, List<string> groundtruths)
        {
            // {
            //     var speed = Speed.SpeedCalc();
            //     var memory = Memory.MemoryCalc();
            //     Console.WriteLine("metrics works");


            Evaluator y = new Evaluator();
            List<List<float>> metricslist = y.Run(transcriptions, groundtruths);
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
