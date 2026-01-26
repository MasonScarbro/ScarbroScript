using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    class ArrayModRunValue : ScarbroScriptClass
    {
        public static object context = null;
        
        public ArrayModRunValue(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("append", new Append());
            modMethods.Add("sizeOf", new SizeOf());
            modMethods.Add("remove", new Remove());
            modMethods.Add("removeAt", new RemoveAt());
            modMethods.Add("contains", new Contains());
            modMethods.Add("reverse", new Reverse());
            modMethods.Add("getAt", new GetAt());
            modMethods.Add("indexOf", new IndexOf());
            modMethods.Add("clear", new Clear());
            modMethods.Add("sort", new Sort());
            modMethods.Add("concat", new Concat());
        }

       
        public class SizeOf : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    var len = (double)val.Count;
                    
                    ArrayModRunValue.context = null;
                    return len;
                }
                ScarbroScript.ModuleError(context, "Error trying to access substring of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Append : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    val.Add(arguments[0]);
                    ArrayModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to Append Value onto" + context.ToString());
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    val.Remove(arguments[0]);
                    ArrayModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access substring of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        public class RemoveAt : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    val.RemoveAt(int.Parse(arguments[0].ToString()));
                    ArrayModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access substring of" + context.ToString());
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    
                    ArrayModRunValue.context = null;
                    return val.Contains(arguments[0]);
                }
                ScarbroScript.ModuleError(context, "Error trying to access substring of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Reverse : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {

                    val.Reverse();
                    ArrayModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to reverse" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class GetAt : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    
                    ArrayModRunValue.context = null;
                    return val[int.Parse(arguments[0].ToString())];
                }
                ScarbroScript.ModuleError(context, "Error trying to access Value of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        public class IndexOf : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    
                    ArrayModRunValue.context = null;
                    return val.IndexOf(arguments[0]);
                }
                ScarbroScript.ModuleError(context, "Error trying to access Index of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Clear : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    val.Clear();
                    ArrayModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access" + context.ToString() + "and Clear it");
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        public class Sort : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    val.Sort();
                    ArrayModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to sort" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        public class Concat : ScarbroScriptCallable
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
                if (ArrayModRunValue.context != null && context is List<object> val)
                {
                    val.Concat((List<object>)arguments[0]);
                    ArrayModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to concat" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

    }

    public class ArrayModRunValueI : ScarbroScriptInstance
    {

        public ArrayModRunValueI(List<object> list) : base(new ArrayModRunValue(list.ToString()))
        {
            
            ArrayModRunValue.context = list;
        }
    }
}
