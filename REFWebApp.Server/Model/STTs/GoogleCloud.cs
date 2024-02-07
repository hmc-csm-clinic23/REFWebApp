using Python.Runtime;

namespace REFWebApp.Server.Model
// export IRONPYTHONPATH=/Library/Frameworks/IronPython.framework/Versions/3.4.1/
{
    public class GoogleCloud : ISTT
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello");
        }

        public Audio ProcessInput(Audio audio)
        {
            var mappedAudio = Mapper.MapGenericInput(audio);
            //throw new NotImplementedException();
            return audio;
        }

        public void Run()
        {
            string scriptname = "GoogleCloud";
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

                //            // string code = File.ReadAllText(file); // Get the python file as raw text
                //            // var scriptCompiled = PythonEngine.Compile(code, file); 
                           var scriptCompiled = Py.Import(scriptname);
                           string[] message = new string[] {"C:\\Users\\micro\\Desktop\\oldREF\\REFApplication\\REFApplication\\Model\\test.wav"};
                           var result = scriptCompiled.InvokeMethod("transcribe_all", message.ToPython());
                           Console.WriteLine(result);
                //                                        //scriptCompiled.InvokeMethod("returndict");
                //                                    // PyObject Pythonnet = scope.Get("Pythonnet"); // Lets get an instance of the class in python
                //                                    // PyObject pythongReturn = Pythonnet.InvokeMethod("returndict"); // Call the sayHello function on the exampleclass object
                //                                    // string? result = pythongReturn.AsManagedObject(typeof(string)) as string; // convert the returned string to managed string object
                //                                    // //Console.WriteLine(result)
            }
            Console.WriteLine("run works");
        }
        

        public void Metrics()
        {
            var speed = Speed.SpeedCalc();
            var memory = Memory.MemoryCalc();
            Console.WriteLine("metrics works");
        }

        public string[] ProcessOutput(string[] args)
        {
            var outputList = Mapper.MapGenericOutput(args);
            //throw new NotImplementedException();
            return args;
        }
    }
}
