using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    class StringModRunValue : ScarbroScriptClass
    {
        public static object context = null;

        public StringModRunValue(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("substring", new SubString());
            modMethods.Add("toLower", new ToLowa());
            modMethods.Add("toUpper", new ToUppa());
            modMethods.Add("indexOf", new IndexOf());
            modMethods.Add("trim", new Trim());
            modMethods.Add("replace", new Replace());
            modMethods.Add("length", new LengthOf());
            modMethods.Add("startsWith", new StartsWith());
            modMethods.Add("endsWith", new EndsWith());
            modMethods.Add("split", new Split());
            modMethods.Add("splitBy", new SplitBy());
        }



        public class SubString : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().Substring(int.Parse(arguments[0].ToString()), int.Parse(arguments[1].ToString()));
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access substring of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class ToLowa : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().ToLower();
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class ToUppa : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().ToUpper();
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().IndexOf(arguments[0].ToString());
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Trim : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().Trim();
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }



        public class Replace : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().Replace(arguments[0].ToString(), arguments[1].ToString());
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class LengthOf : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().Length;
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }


        public class StartsWith : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().StartsWith(arguments[0].ToString());
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class EndsWith : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().EndsWith(arguments[0].ToString());
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class Split : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    var val = StringModRunValue.context.ToString().Split();
                    StringModRunValue.context = null;
                    return val;
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        public class SplitBy : ScarbroScriptCallable
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
                if (StringModRunValue.context != null)
                {
                    if (arguments[0] is char _ch)
                    {
                        var val = StringModRunValue.context.ToString().Split(_ch).ToList<object>();
                        StringModRunValue.context = null;
                        return val;
                    }
                    
                }
                ScarbroScript.ModuleError(context, "Error trying to access string of" + context.ToString() + "To Convert Top Upper");
                return null;


            }

            public override string ToString() => "<native fn>";
        }

    }



    public class StringModRunValueI : ScarbroScriptInstance
    {
       
        public StringModRunValueI(string name) : base(new StringModRunValue(name))
        {
            StringModRunValue.context = name;
        }
    }
}
