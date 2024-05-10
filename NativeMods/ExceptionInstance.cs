using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    class ExceptionInstance : ScarbroScriptClass
    {

        public static Exception context = null;

        public ExceptionInstance(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 

            modMethods.Add("message", new ExceptionMsg());
            modMethods.Add("stacktrace", new StackTrace());
            modMethods.Add("helplink", new HelpLink());


        }


        class ExceptionMsg : ScarbroScriptCallable
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

                String input = context.Message;
                return input;

            }

            public override string ToString() => "<native fn>";
        }

        class StackTrace : ScarbroScriptCallable
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

                String input = context.StackTrace;
                return input;

            }

            public override string ToString() => "<native fn>";
        }

        class HelpLink : ScarbroScriptCallable
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

                String input = context.HelpLink;
                return input;

            }

            public override string ToString() => "<native fn>";
        }


    }

    public class ExceptionInstanceI : ScarbroScriptInstance
    {

        public ExceptionInstanceI(string name, Exception context) : base(new ExceptionInstance(name))
        {
            ExceptionInstance.context = context;
        }
    }
}
