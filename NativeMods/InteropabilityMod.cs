using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace ScarbroScript.NativeMods
{
    class InteropabilityMod : ScarbroScriptClass
    {

        public InteropabilityMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("executeMethod", new ExecuteMethod());
            modMethods.Add("generateExe", new GenerateExe());
            modMethods.Add("executeFile", new ExecuteFile());




        }

        public class ExecuteMethod : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 4; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                string code = File.ReadAllText(arguments[0].ToString());

                // Set up compiler options
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false; // We don't want an executable file, just an assembly
                parameters.GenerateInMemory = true; // We want to generate the assembly in memory
                parameters.TreatWarningsAsErrors = false; // We don't want to treat warnings as errors

                // Add any necessary assemblies
                parameters.ReferencedAssemblies.Add("System.dll"); // Add any other assemblies you might need
                CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
                using (codeProvider)
                {
                    // Compile the code
                    CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);

                    // Check for compilation errors
                    if (results.Errors.Count > 0)
                    {
                        Console.WriteLine("Compilation errors:");
                        foreach (CompilerError error in results.Errors)
                        {
                            Console.WriteLine(error.ErrorText);
                        }
                    }
                    else
                    {
                        // Compilation successful, get the assembly
                        Assembly assembly = results.CompiledAssembly;

                        
                        Type type = assembly.GetType(arguments[1].ToString() + "." + arguments[2].ToString());
                        if (type != null)
                        {
                            MethodInfo method = type.GetMethod(arguments[3].ToString());
                            if (method != null)
                            {
                                // Call the method and capture the return value
                                object returnValue = method.Invoke(null, null);

                                // Display the return value
                                Console.WriteLine("Return value: " + returnValue);
                                return returnValue;
                            }
                            else
                            {
                                Console.WriteLine("Method not found.");

                            }
                        }
                        else
                        {
                            Console.WriteLine("Type not found.");
                        }
                    }
                }

                ScarbroScript.ModuleError(arguments[0], "Problem With Compiling File");
                return null;
            }

            public override string ToString() => "<native fn>";
        }


        public class GenerateExe : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 3; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                string code = File.ReadAllText(arguments[0].ToString());

                // Set up compiler options
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = true;
                parameters.OutputAssembly = Path.Combine(arguments[2].ToString(), arguments[1].ToString());
                parameters.GenerateInMemory = false; 
                parameters.TreatWarningsAsErrors = false;

                // Add any necessary assemblies
                parameters.ReferencedAssemblies.Add("System.dll"); // Add any other assemblies you might need
                CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
                using (codeProvider)
                {
                    // Compile the code
                    CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);

                    // Check for compilation errors
                    if (results.Errors.Count > 0)
                    {
                        Console.WriteLine("Compilation errors:");
                        foreach (CompilerError error in results.Errors)
                        {
                            Console.WriteLine(error.ErrorText);
                        }
                    }
                    else
                    {
                        // Compilation successful, get the assembly
                        Console.WriteLine("Executable file generated successfully:" + arguments[1].ToString());
                    }
                }

                ScarbroScript.ModuleError(arguments[0], "Problem With Compiling File");
                return null;
            }

            public override string ToString() => "<native fn>";
        }

        public class ExecuteFile : ScarbroScriptCallable
        {
            public int Arity { get { return 1; } }

            public object Call(Interpreter interpreter, List<object> arguments)
            {
                List<object> returnValues = new List<object>();

                string code = File.ReadAllText(arguments[0].ToString());
                FileInfo srcInfo = new FileInfo(arguments[0].ToString());
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.TreatWarningsAsErrors = false;

                CodeDomProvider provider = null;
                parameters.ReferencedAssemblies.Add("System.dll");
                if (srcInfo.Extension.ToUpper().Equals(".CS"))
                {
                    provider = CodeDomProvider.CreateProvider("CSharp");
                }
                else
                {
                    Console.WriteLine("Not a C# File Extension :(");
                }
                using (provider)
                {
                    CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);


                    if (results.Errors.Count > 0)
                    {
                        Console.WriteLine("Compiler Errors");
                        foreach (CompilerError ce in results.Errors){
                            Console.WriteLine(ce.ErrorText);
                        }
                    }
                    //else
                    Assembly asm = results.CompiledAssembly;
                    Type[] types = asm.GetTypes();
                    var typesByNamespace = types.GroupBy(t => t.Namespace);
                    
                    foreach (var group in typesByNamespace)
                    {
                        foreach (var klass in group)
                        {
                            Type type = asm.GetType($"{group.Key}.{klass.Name}");
                            if (type != null)
                            {
                                MethodInfo[] methods = type.GetMethods();
                                foreach (var method in methods)
                                {
                                    if (method != null)
                                    {
                                        // Call the method and capture the return values
                                        
                                        returnValues.Add(method.Invoke(null, null));
                                    }
                                }
                            }
                        }
                    }
                    
                }
                if (returnValues.Count == 0) ScarbroScript.ModuleError(returnValues, "No return Values Detected, Check C# file for returns");
                return returnValues;
                    
            }

            public override string ToString() => "<native fn>";
        }


    }
    public class InteropabilityModI : ScarbroScriptInstance
    {
        public InteropabilityModI() : base(new InteropabilityMod("Interope"))
        {
            //Construct
        }

        public InteropabilityModI(string name) : base(new InteropabilityMod(name))
        {
            //Construct
        }
    }
}
