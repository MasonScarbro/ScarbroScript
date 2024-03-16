using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    //IO STREAMS
    class IOMod : ScarbroScriptClass
    {

        public IOMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            

        }

    }

    //FILE STREAM
    class FileMod : ScarbroScriptClass
    {

        public FileMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 


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
                    if (((string)arguments[2]).Equals("w"))
                    {
                         File.WriteAllText((string)arguments[0], (string)arguments[1]);
                    }
                    if (((string)arguments[2]).Equals("a"))
                    {
                        File.AppendAllText((string)arguments[0], (string)arguments[1]);
                    }
                    if (((string)arguments[2]).Equals("ow"))
                    {
                        string content = File.ReadAllText((string)arguments[0]);
                        File.WriteAllText(content, (string)arguments[1]);
                    }
                    if (((string)arguments[2]).Equals("oa"))
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
