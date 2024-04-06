using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript.NativeMods
{
    public class MathMod : ScarbroScriptClass
    {
        public static object context = null;
        
        public MathMod(string name) : base(name, new Dictionary<string, ScarbroScriptFunction>(), new Dictionary<string, ScarbroScriptCallable>())
        {
            // Super 
            modMethods.Add("cos", new CosVal());
            modMethods.Add("sin", new SinVal());  
            modMethods.Add("tan", new TanVal());
            modMethods.Add("random", new RandomNum());
            modMethods.Add("exp", new ExpVal());
            modMethods.Add("sqrt", new Sqrt());
            modMethods.Add("abs", new Abs());
            modMethods.Add("ceil", new Ceil());
            modMethods.Add("floor", new Floor());
            modMethods.Add("pow", new Pow());
            modMethods.Add("tes", new Test());
            

        }

        public class SinVal : ScarbroScriptCallable
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

                return Math.Sin((double)arguments[0]);
                

            }

            public override string ToString() => "<native fn>";
        }

        class CosVal : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Cos(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                return Math.Cos((double)arguments[0]);

            }

            public override string ToString() => "<native fn>";
        }

        class TanVal : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Tan(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                return Math.Tan((double)arguments[0]);

            }

            public override string ToString() => "<native fn>";
        }

        class ExpVal : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Tan(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                return Math.Exp((double)arguments[0]);

            }

            public override string ToString() => "<native fn>";
        }

        class Sqrt : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Tan(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                return Math.Sqrt((double)arguments[0]);

            }

            public override string ToString() => "<native fn>";
        }

        class Abs : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Absolut(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                return Math.Abs((double)arguments[0]);

            }

            public override string ToString() => "<native fn>";
        }

        class Ceil : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Ceiling(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                return Math.Ceiling((double)arguments[0]);

            }

            public override string ToString() => "<native fn>";
        }


        class Floor : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 1; } }

            /// <summary>
            /// Returns the Floor(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {

                return Math.Floor((double)arguments[0]);

            }

            public override string ToString() => "<native fn>";
        }


        class Pow : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 2; } }

            /// <summary>
            /// Returns the Square root(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                var v = 0.0;
                for (int i = 0; i < (double)arguments[1]; i++)
                {
                    v = ((double)arguments[0] * (double)arguments[0]);
                }
                return v;

            }

            public override string ToString() => "<native fn>";
        }

        class Test : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 2; } }

            /// <summary>
            /// Returns the Square root(x) value
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                
                return this.ToString();

            }

            public override string ToString() => "<native fn>";
        }

        class RandomNum : ScarbroScriptCallable
        {
            //Arity is 0 due to no arguments
            public int Arity { get { return 2; } }

            /// <summary>
            /// Returns a random number with bounds
            /// </summary>
            /// <param name="interpreter"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public object Call(Interpreter interpreter, List<Object> arguments)
            {
                Random rand = new Random();
                return rand.Next(int.Parse((string)arguments[0].ToString()), int.Parse((string)arguments[1].ToString()));

            }


            public override string ToString() => "<native fn>";
        }
    }
    public class MathModI : ScarbroScriptInstance
    {
        public MathModI() : base(new MathMod("Math"))
        {
            //Construct
        }

        public MathModI(string name) : base(new MathMod(name))
        {
            //Construct
        }
    }
}
