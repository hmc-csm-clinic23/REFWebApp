using Python.Runtime;


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

        public void Run(string transcriptions_file)
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
                sys.path.append(@"C:\Users\micro\Desktop\oldREF\REFApplication\REFApplication\Model");
                //sys.path.append(@"/Users/sathv/Desktop/REFApplication/REFApplication/Model");

                var scriptCompiled = Py.Import(scriptname);
                //string[] message = new string[] {transcriptions_file, "/Users/sathv/Desktop/REFApplication/REFApplication/ground_truth.csv"};
                string[] message = new string[] { transcriptions_file, "C:\\Users\\micro\\Desktop\\oldREF\\REFApplication\\REFApplication\\ground_truth.csv" };
                Console.WriteLine(message);
                var result = scriptCompiled.InvokeMethod("evaluate", message.ToPython());

                Console.WriteLine(result);

                // string code = File.ReadAllText(file); // Get the python file as raw text
                // var scriptCompiled = PythonEngine.Compile(code, file); // Compile the code/file
                // scope.Execute(scriptCompiled); // Execute the compiled python so we can start calling it.
                // PyObject exampleClass = scope.Get("exampleClass"); // Lets get an instance of the class in python
                // PyObject pythongReturn = exampleClass.InvokeMethod("sayHello"); // Call the sayHello function on the exampleclass object
                // string? result = pythongReturn.AsManagedObject(typeof(string)) as string; // convert the returned string to managed string object
            }
            
            return result;
        }
    }

}

