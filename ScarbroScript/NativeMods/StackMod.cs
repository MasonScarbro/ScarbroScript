using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    class StackMod : ScarbroScriptClass
    {
        public static object context = new Stack<object>();
        public StackMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("push", new Push());
            modMethods.Add("printValues", new PrintValues());
            modMethods.Add("pop", new Pop());
            modMethods.Add("count", new Count());
            modMethods.Add("clear", new Clear());
            modMethods.Add("peek", new Peek());


        }


        public class Push : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                if (StackMod.context != null && StackMod.context is Stack<object> val)
                {
                    val.Push(arguments[0]);

                    StackMod.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        public class Pop : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 0; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                if (StackMod.context != null && StackMod.context is Stack<object> val)
                {
                    
                    StackMod.context = null;
                    return val.Pop();
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Clear : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 0; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                if (StackMod.context != null && StackMod.context is Stack<object> val)
                {
                    val.Clear();
                    StackMod.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Count : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 0; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                if (StackMod.context != null && StackMod.context is Stack<object> val)
                {

                    StackMod.context = null;
                    return val.Count;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Peek : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 0; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                if (StackMod.context != null && StackMod.context is Stack<object> val)
                {
                    val.Peek();
                    StackMod.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class PrintValues : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 0; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                if (StackMod.context != null && StackMod.context is Stack<object> val)
                {

                    StackMod.context = null;
                    foreach (object value in val)
                    {
                        Console.WriteLine("Values: " + value);
                    }
                }
                //ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }



    }
    public class StackModI : ScarbroScriptInstance
    {
        public StackModI() : base(new StackMod("Stack"))
        {
            //Construct
            StackMod.context = new Stack<object>();
        }

        public object GetContext()
        {
            return StackMod.context;
        }

        public StackModI(Stack<object> q) : base(new StackMod(q.ToString()))
        {
            //Construct
            StackMod.context = q;

        }
    }
}
