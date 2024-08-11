using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    class NumberModRunValue : ScarbroScriptClass
    {
        public static object context = null;

        public NumberModRunValue(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("compareTo", new CompareTo());
            
        }
        


        public class CompareTo : ScarbroScriptCallable
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
                if (NumberModRunValue.context != null)
                {
                    double val = (double)NumberModRunValue.context;
                    NumberModRunValue.context = null;
                    return val.CompareTo(arguments[0]);
                }
                ScarbroScript.ModuleError(context, "Error trying to access compare To of" + context.ToString());
                return null;


            }

            public override string ToString() => "<native fn>";
        }

        

    }



    public class NumberModRunValueI : ScarbroScriptInstance
    {

        public NumberModRunValueI(double context) : base(new NumberModRunValue(context.ToString()))
        {
            NumberModRunValue.context = context;
        }
    }
}
