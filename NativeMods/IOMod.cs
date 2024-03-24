using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ScarbroScript.NativeMods
{
    //IO STREAMS
    public class IOMod : ScarbroScriptClass
    {

        public IOMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("readln", new ReadFromCons());

        }

        class ReadFromCons : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 0; } }

            /// <summary>
            ///  Reads from the console
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                String input = Console.ReadLine();
                return input;

            }

            public override string ToString() => "<native fn>";
        }

    }

    //FILE STREAM
    class FileMod : ScarbroScriptClass
    {

        public FileMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("write", new WriteTo());
            modMethods.Add("read", new ReadIn());
            modMethods.Add("readlns", new ReadInLine());

        }


        class WriteTo : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 3; } }

            /// <summary>
            /// Writes to a file given a path 
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                
                try 
                {
                    arguments[0] = FileMod.CorrectPath((string)arguments[0]);
                    if (((string)arguments[2]).Equals("w"))
                    {
                         File.WriteAllText((string)arguments[0], (string)arguments[1]);
                    }
                    else if (((string)arguments[2]).Equals("a"))
                    {
                        File.AppendAllText((string)arguments[0], (string)arguments[1]);
                    }
                    else if (((string)arguments[2]).Equals("ow"))
                    {
                        string content = File.ReadAllText((string)arguments[0]);
                        File.WriteAllText(content, (string)arguments[1]);
                    }
                    else if (((string)arguments[2]).Equals("oa"))
                    {
                        string content = File.ReadAllText((string)arguments[0]);
                        File.AppendAllText(content, (string)arguments[1]);
                    }
                    else
                    {
                        ScarbroScript.ModuleError((string)arguments[2], "Missing or Incorrect Write Command Current Commands are... w: write, a: append, ow: overwrite, oa: append to file ");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;
                

            }

            public override string ToString() => "<native fn>";
        }

        class ReadIn : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Writes to a file given a path 
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                try
                {
                    arguments[0] = FileMod.CorrectPath((string)arguments[0]);
                    return File.ReadAllText((string)arguments[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        class ReadInLine : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            
            public int Arity { get { return 1; } }

            /// <summary>
            /// Writes to a file given a path 
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                try
                {
                    arguments[0] = FileMod.CorrectPath((string)arguments[0]);
                    return File.ReadLines((string)arguments[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;
                

            }
            
            public override string ToString() => "<native fn>";
        }


        class RemoveFile : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments

            public int Arity { get { return 1 ; } }

            /// <summary>
            /// Writes to a file given a path 
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                try
                {
                    arguments[0] = FileMod.CorrectPath((string)arguments[0]);
                    File.Delete((string)arguments[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        class MoveFile : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments

            public int Arity { get { return 2; } }

            /// <summary>
            /// Writes to a file given a path 
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                try
                {
                    arguments[0] = FileMod.CorrectPath((string)arguments[0]);
                    arguments[1] = FileMod.CorrectPath((string)arguments[0]);
                    File.Move((string)arguments[0], (string)arguments[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public static string CorrectPath(string path)
        {
            Regex regex = new Regex(@"\\\\");
            if (!regex.IsMatch(path))
            {
                path = path.Replace("\\", "\\\\"); // replace any single '\' with '\\'
                path = path.Replace("/", "\\\\");
                path = path.Replace("//", "\\\\");
                return path;
            }
            return path; // else

        }
    }

    public class IOModI : ScarbroScriptInstance
    {
        public IOModI() : base(new IOMod("IO"))
        {
            //Construct
        }

    }

    public class FileModI : ScarbroScriptInstance
    {
        public FileModI() : base(new FileMod("File"))
        {
            //Construct
        }

    }
    
}
