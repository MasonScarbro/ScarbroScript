using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    class QueueMod : ScarbroScriptClass
    {
        public static object context = new Queue<object>();
        public QueueMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("enqueue", new Enq());
            modMethods.Add("printValues", new PrintValues());
            modMethods.Add("dequeue", new Deq());
            modMethods.Add("count", new Count());
            modMethods.Add("clear", new Clear());
            modMethods.Add("peek", new Peek());


        }


        public class Enq : ScarbroScriptCallable
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
                if (QueueMod.context != null && QueueMod.context is Queue<object> val)
                {
                    val.Enqueue(arguments[0]);
                    
                    QueueMod.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        public class Deq : ScarbroScriptCallable
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
                if (QueueMod.context != null && QueueMod.context is Queue<object> val)
                {
                    val.Dequeue();
                    QueueMod.context = null;
                    return val;
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
                if (QueueMod.context != null && QueueMod.context is Queue<object> val)
                {
                    val.Clear();
                    QueueMod.context = null;
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
                if (QueueMod.context != null && QueueMod.context is Queue<object> val)
                {
                    
                    QueueMod.context = null;
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
                if (QueueMod.context != null && QueueMod.context is Queue<object> val)
                {
                    val.Peek();
                    QueueMod.context = null;
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
                if (QueueMod.context != null && QueueMod.context is Queue<object> val)
                {
                    
                    QueueMod.context = null;
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
    public class QueueModI : ScarbroScriptInstance
    {
        public QueueModI() : base(new QueueMod("Queue"))
        {
            //Construct
            QueueMod.context = new Queue<object>();
        }

        public object GetContext()
        {
            return QueueMod.context;
        }

        public QueueModI(Queue<object> q) : base(new QueueMod(q.ToString()))
        {
            //Construct
            QueueMod.context = q;
            
        }
    }
}
