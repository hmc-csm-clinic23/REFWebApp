using Python.Runtime;
using System.Net;


namespace REFWebApp.Server.Model
{
    public class Evaluator
    {
        public static void Initialize()
        {
            //string pythonDll = @"/Users/sathv/opt/anaconda3/lib/libpython3.9.dylib";
            string pythonDLL = @"C:\Users\micro\AppData\Local\Programs\Python\Python39\python39.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDLL);
            PythonEngine.Initialize();
        }

        public List<List<float>> Run(List<string> transcriptions_file, List<string> groundtruth)
        {
            string scriptname = "eval";
            Initialize();
            // Runtime.PythonDLL = @"/Users/sathv/opt/anaconda3/lib/libpython3.9.dylib";
            // PythonEngine.Initialize();
            Py.GIL();

            string file = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/eval.py";
            // throw new NotImplementedException();

            if (!PythonEngine.IsInitialized)// Since using asp.net, we may need to re-initialize
            {
                PythonEngine.Initialize();
                Py.GIL();
            }

            using (var scope = Py.CreateScope())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\micro\source\repos\REFWebApp\REFWebApp.Server\Model");
                //sys.path.append(@"/Users/sathv/Desktop/REFApplication/REFApplication/Model");

                var scriptCompiled = Py.Import(scriptname);
                //string[] message = new string[] {transcriptions_file, "/Users/sathv/Desktop/REFApplication/REFApplication/ground_truth.csv"};
                List<string> tra = transcriptions_file;
                List<string> gt =groundtruth ;
                //Console.WriteLine(message);
                var result = scriptCompiled.InvokeMethod("evaluate", tra.ToPython(), gt.ToPython());
                Console.WriteLine("RESULT: " + result);
                PyObject[] pyOuterlist = result.AsManagedObject(typeof(PyObject[])) as PyObject[];

                List<List<float>> metricslist = new List<List<float>>();

                foreach(PyObject pyInnerlist in pyOuterlist){
                    PyObject[] pyInnerObject = pyInnerlist.AsManagedObject(typeof(PyObject[])) as PyObject[];
                    List<float> innerlist = new List<float>();

                    foreach(PyObject pyobject in pyInnerObject)
                    {
                        float val = (float)pyobject.AsManagedObject(typeof(float));
                        innerlist.Add(val);
                    }
                    metricslist.Add(innerlist);
                }

                //List<float> metricslist = (List<float>)result;
                //List<float> metricslist = (List<float>)result.AsManagedObject(typeof(List<float>));
                //if (result != null)
                //{
                //    //metricslist = (float[])result.AsManagedObject(typeof(float[]));
                //    metricslist = 
                //}

                Console.WriteLine("metricslist: " + metricslist);

                // string code = File.ReadAllText(file); // Get the python file as raw text
                // var scriptCompiled = PythonEngine.Compile(code, file); // Compile the code/file
                // scope.Execute(scriptCompiled); // Execute the compiled python so we can start calling it.
                // PyObject exampleClass = scope.Get("exampleClass"); // Lets get an instance of the class in python
                // PyObject pythongReturn = exampleClass.InvokeMethod("sayHello"); // Call the sayHello function on the exampleclass object
                // string? result = pythongReturn.AsManagedObject(typeof(string)) as string; // convert the returned string to managed string object
                return metricslist;
            }
        }
    }

}

