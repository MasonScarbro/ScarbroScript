using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScarbroScript.NativeMods
{
    class KVMod : ScarbroScriptClass
    {
        public static object context = null;
        public KVMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("add", new Add());
            modMethods.Add("remove", new Remove());
            modMethods.Add("contains", new Contains());
            modMethods.Add("getVal", new GetVal());
            modMethods.Add("getVals", new GetValues());
            modMethods.Add("getKeys", new GetKeys());
            
        }


        public class Add : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 2; } }

            /// <summary>
            /// Returns the Sin(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                if (KVMod.context != null && KVMod.context is Dictionary<object, object> val)
                {
                    val.Add(arguments[0], arguments[1]);
                    KVMod.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Remove : ScarbroScriptCallable
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
                if (KVMod.context != null && KVMod.context is Dictionary<object, object> val)
                {
                    val.Remove(arguments[0]);
                    KVMod.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Contains : ScarbroScriptCallable
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
                if (KVMod.context != null && KVMod.context is Dictionary<object, object> val)
                {
                    val.ContainsKey(arguments[1]);
                    KVMod.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class GetVal : ScarbroScriptCallable
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
                if (KVMod.context != null && KVMod.context is Dictionary<object, object> val)
                {
                    
                    KVMod.context = null;
                    return val[arguments[0]];
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class GetValues : ScarbroScriptCallable
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
                if (KVMod.context != null && KVMod.context is Dictionary<object, object> val)
                {

                    KVMod.context = null;
                    return val.Values;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class GetKeys : ScarbroScriptCallable
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
                if (KVMod.context != null && KVMod.context is Dictionary<object, object> val)
                {

                    KVMod.context = null;
                    return val.Keys;
                }
                ScarbroScript.ModuleError(context, "Error trying to add Key and Value to " + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

    }
    public class KVModI : ScarbroScriptInstance
    {
        public KVModI() : base(new KVMod("Dict"))
        {
            //Construct
            KVMod.context = new Dictionary<object, object>();
        }

        public object GetContext()
        {
            return KVMod.context;
        }

        public KVModI(Dictionary<object, object> kv) : base(new KVMod(kv.ToString()))
        {
            //Construct
            KVMod.context = kv;
        }
    }
}
