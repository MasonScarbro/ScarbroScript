using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarbroScript;

namespace ScarbroScriptLSP.Analysis
{
    
    public static class Linterpreter
    {
        public class LinterpreterError : FormatException
        {
            public int line;
            public string problemChild;
            public LinterpreterError(string message, int line, string problemChild) : base(message)
            {
                this.line = line;
                this.problemChild = problemChild;
            }
        }

        public static Dictionary<string, object> scopedVariables = new Dictionary<string, object>();
        public static Dictionary<string, object> scopedClasses = new Dictionary<string, object>();
        public static List<LinterpreterError> lintErrors = new List<LinterpreterError>();
        public class Scoper
        {

            public Scoper()
            {

            }
            public Scoper(List<Stmt> stmts)
            {
                foreach(Stmt stmt in stmts)
                {
                    if (stmt is Stmt.Var stmtv)
                    {
                        Program.logger.Log("Inside Scoper Just found a Variable");
                        StoreScopedObject(stmtv.name.lexeme, EvaluateType(stmtv.initializer));
                    }
                    if (stmt is Stmt.Class stmtc)
                    {
                        
                    }
                }
            }

            



            /// <summary>
            /// for now this only handles variables but since I only
            /// need the list of methods for classes I can probably
            /// check ifn its a ''type'' or a list of strings (name of each method)
            /// </summary>
            /// <param name="name"></param>
            /// <param name="type"></param>
            public void StoreScopedObject(string name, object type)
            {
                scopedVariables[name] = type;
            }


            private object EvaluateType(Expr value)
            {

                //Example if the variable is init to another it should match the previous variable
                if (value is Expr.Variable ev)
                {
                    if (!scopedVariables.ContainsKey(ev.name.lexeme))
                    {
                        lintErrors.Add(new LinterpreterError
                            (
                            "Variable not found in file",
                            ev.name.line,
                            ev.name.lexeme
                            ));
                    }
                    //else
                    return scopedVariables[ev.name.lexeme];
                }
                //if its an array just return the type so it can be scoped
                if (value is Expr.Array)
                {
                    Program.logger.Log("Inside Scoper, found arr type!");
                    return typeof(List<>);
                }
                

                return null;
            }
        }
        

    }
}
