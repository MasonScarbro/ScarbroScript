using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    class Clock : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 0; } }

        /// <summary>
        ///  The Call method is where the actual implementation of the "clock"
        ///  function resides. It returns the current time in seconds
        ///  since a fixed point in time (usually the Unix epoch)
        ///  as a double value.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {
            return DateTime.Now.Ticks / (double)TimeSpan.TicksPerSecond;
        }

        public override string ToString() => "<native fn>";
    }

    /// <summary>
    /// 
    /// </summary>
    class EvaluateExpression : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 1; } }

        /// <summary>
        ///  I literally reused the resources I already wrote. 
        ///  Basically we pass in the expression as an argument 
        ///  tokenize it and then interpret it but of course we treat it
        ///  as an Expression instead of list of Stmts, we use the AstPrinter as a sort of interpreter only built for expressions
        ///  similar to what was written at the beginning of the book this way we can ignore things like ';' and etc.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {
 
            Scanner scanner = new Scanner((string)arguments[0]); //Our own scanner not the built in one (for Java)
            List<Token> tokens = scanner.ScanTokens();
            Parser parser = new Parser(tokens);
            Expr expression = parser.ParseToExpr();
            return new AstPrinter().Print(expression);



        }

        public override string ToString() => "<native fn>";
    }

    class ParseToNum : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 1; } }

        /// <summary>
        /// Parses a value to a num
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {

            return double.Parse((string)arguments[0]);

        }

        public override string ToString() => "<native fn>";
    }

    class ParseToString : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 1; } }

        /// <summary>
        ///  Not really needed but it allows paring to a string.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {

            return arguments[0].ToString();
           
        }

        public override string ToString() => "<native fn>";
    }

    class Scarbro : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 0; } }

        /// <summary>
        ///  Not really needed but it allows paring to a string.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {

            return "Mason Scarbro, Creator of this language and Mediocare Developer, \n" +
                "Born July 7th 2003, \n" +
                "[July 7th 2003, 5:06am]: Mason escapes his mothers canal \n" +
                "[July 7th 2004-2009]: Mason has infintile amnesia from these points thus memories cannot be processed \n" +
                "[2014]: Mason Graduates from 5th grade" +
                "[2015]: Cam Newton goes to the superbowl (No relevent information pertaining to mason) \n" +
                "[2017]: Mason starts Highschool \n" +
                "[2021]: Mason Graduates Highschool \n";

        }

        public override string ToString() => "<native fn>";
    }
}
