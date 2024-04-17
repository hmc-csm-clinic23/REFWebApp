﻿using Python.Runtime;
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

        public List<float> Run(string transcriptions_file, string groundtruth)
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

            // var m_threadState = PythonEngine.BeginAllowThreads();
            using (var gil = Py.GIL())
            {
                using (var scope = Py.CreateScope())
                {
                    dynamic sys = Py.Import("sys");
                    sys.path.append(@"C:\Users\micro\source\repos\REFWebApp\REFWebApp.Server\Evaluation\");
                    //sys.path.append(@"/Users/sathv/Desktop/REFApplication/REFApplication/Model");

                    var scriptCompiled = Py.Import(scriptname);
                    //string[] message = new string[] {transcriptions_file, "/Users/sathv/Desktop/REFApplication/REFApplication/ground_truth.csv"};
                    string tra = transcriptions_file;
                    string gt = groundtruth;
                    //Console.WriteLine(message);
                    var result = scriptCompiled.InvokeMethod("metrics", gt.ToPython(), tra.ToPython());
                    Console.WriteLine("RESULT: " + result);
                    PyObject[] pylist = result.AsManagedObject(typeof(PyObject[])) as PyObject[];

                    List<float> metricslist = new List<float>();


                    foreach (PyObject pyobject in pylist)
                    {
                        float val = (float)pyobject.AsManagedObject(typeof(float));
                        metricslist.Add(val);
                    }

                    //List<float> metricslist = (List<float>)result;
                    //List<float> metricslist = (List<float>)result.AsManagedObject(typeof(List<float>));
                    //if (result != null)
                    //{
                    //    //metricslist = (float[])result.AsManagedObject(typeof(float[]));
                    //    metricslist = 
                    //}

                    Console.WriteLine("metricslist: " + metricslist);

                    // PythonEngine.EndAllowThreads(m_threadState);
                    //PythonEngine.Shutdown();

                    // string code = File.ReadAllText(file); // Get the python file as raw text
                    // var scriptCompiled = PythonEngine.Compile(code, file); // Compile the code/file
                    // scope.Execute(scriptCompiled); // Execute the compiled python so we can start calling it.
                    // PyObject exampleClass = scope.Get("exampleClass"); // Lets get an instance of the class in python
                    // PyObject pythongReturn = exampleClass.InvokeMethod("sayHello"); // Call the sayHello function on the exampleclass object
                    // string? result = pythongReturn.AsManagedObject(typeof(string)) as string; // convert the returned string to managed string object
                    return metricslist;
                }
            }

          //return metricslist;
        }
    }

}

