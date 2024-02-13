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

    class Log : ScarbroScriptCallable
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

            Console.WriteLine(arguments[0]);
            return null;


        }

        public override string ToString() => "<native fn>";
    }

    class ReadFromCons : ScarbroScriptCallable
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
            
            String input = Console.ReadLine();
            return input;

        }

        public override string ToString() => "<native fn>";
    }


    class Matrix : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 1; } }

        /// <summary>
        ///  Reads from the console
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {

            String matrix = "010101010101010101001110101010101010101010110101010101010100110101010101010";
            Random rand = new Random();

            char[] matrixArr = matrix.Substring(0, (matrix.Length / 4) - 1).ToCharArray();
            int iter = 0;
            while (iter < (double)arguments[0])
            {
                if (rand.Next(0, 1) == 1)
                {
                    Array.Reverse(matrixArr);
                }
                String matrix1 = new String(matrixArr);
                matrixArr = matrix.Substring(matrix.Length / 4, (matrix.Length / 2) - 1).ToCharArray();
                if (rand.Next(1, 2) == 2)
                {
                    Array.Reverse(matrixArr);
                }
                String matrix2 = new String(matrixArr);
                matrixArr = matrix.Substring(matrix.Length / 2).ToCharArray();
                if (rand.Next(0, 2) == 2)
                {
                    Array.Reverse(matrixArr);
                }
                String matrix3 = new String(matrixArr);
                matrixArr = matrix.Substring((matrix.Length / 2) + (matrix.Length / 4)).ToCharArray();
                if (rand.Next(0, 2) == 2)
                {
                    Array.Reverse(matrixArr);
                }
                String matrix4 = new String(matrixArr);
                matrix += matrix1 + matrix2 + matrix3 + matrix4;
                
                Console.WriteLine(matrix);
                iter++;
            }
            
            return null;


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

    class SinVal : ScarbroScriptCallable
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

    class ArrGet : ScarbroScriptCallable
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
            if (arguments[0] is List<object> varArr)
            {
               return varArr[int.Parse(arguments[1].ToString())];
            }
            return "error";
        }


        public override string ToString() => "<native fn>";
    }

    class ArrSet : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 3; } }

        /// <summary>
        /// Returns a random number with bounds
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {
            if (arguments[0] is List<object> varArr)
            {
                varArr[int.Parse(arguments[1].ToString())] = arguments[2];
            }
            return "error";
        }


        public override string ToString() => "<native fn>";
    }

    class ArrAdd : ScarbroScriptCallable
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
            if (arguments[0] is List<object> varArr)
            {
                varArr.Add(arguments[1]);
            }
            return "error";
        }


        public override string ToString() => "<native fn>";
    }

    class ArrLen : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 1; } }

        /// <summary>
        /// Returns a random number with bounds
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {
            if (arguments[0] is List<object> varArr)
            {
                return varArr.Count();
            }
            return "Length Must have 2 params and Must be either a String or List";
        }


        public override string ToString() => "<native fn>";
    }

    class ReverseArr : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 1; } }

        /// <summary>
        /// Returns a random number with bounds
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {
            if (arguments[0] is List<object> varArr)
            {
                varArr.Reverse();
            }
            else if (arguments[0] is String s)
            {
                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                return new String(charArray);
            }
            return "error";
        }


        public override string ToString() => "<native fn>";
    }

    class SortArr : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 1; } }

        /// <summary>
        /// Returns a random number with bounds
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {
            if (arguments[0] is List<object> varArr)
            {
                varArr.Sort();
            }
            
            return "error";
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

            return 
                @"The surname Scarbro was first found in North Riding of Yorkshire at Scarborough, a borough, markettown, and parish. [1]

                ""The origin of this town has not been satisfactorily ascertained: it is supposed to have derived its name from the Saxon Scear, a rock, and Burgh, a fortified place. The earliest authentic record of it is a charter of Henry II., conferring certain privileges on the inhabitants; and in the reign of Henry III., a charter was granted for making a new pier at Scardeburgh, as the place was then called."" [2]

                Some of the family were found further north in Scotland in early years too. ""Nicholas de Scardbrow witnessed charters by Willelmus de Hawoc, burgess of Perth, c. 1245 and Roger de Scardtheburge was clericus domini regis, c. 1272. Robert de Scardeburgh was parson of the church of Conington in 1295."" [3] But this latter source notes that the name was indeed from Yorkshire.

                Sir Robert de Scorburgh (d. 1340), was Baron of the Exchequer and ""derived his name from Scorborough in the East Riding of Yorkshire. He is no doubt the Robert de Scorburgh of Beverley to whom there are some references in 1320 to 1322. At his death he is described as possessing the manor of Scoreby, together with property in Stamford Bridge and Etton."" [4]

                It is only in the last few hundred years that the English language has been standardized. For that reason, early Anglo-Saxon surnames like Scarbro are characterized by many spelling variations. As the English language changed and incorporated elements of other European languages, even literate people changed the spelling of their names. The variations of the name Scarbro include: Scarbrough, Scarboro, Scarborough, Scasbridge, Scarbrow, Scarburg, Scarburgh, Scarsbridge and many more.

                There are currently only 107 people with the surname Scarbro Alive Today. The Author, Of this Lang: Mason Scarbro Is one Of them!" +
                "Mason Scarbro, Creator of this language and Mediocare Developer, \n" +
                "Born July 7th 2003, \n" +
                "[July 7th 2003, 5:06am]: Mason escapes his mothers canal \n" +
                "[July 7th 2004-2009]: Mason has infintile amnesia from these points thus memories cannot be processed \n" +
                "[2014]: Mason Graduates from 5th grade" +
                "[2015]: Cam Newton goes to the superbowl (No relevent information pertaining to Mason) \n" +
                "[2017]: Mason starts Highschool \n" +
                "[2021]: Mason Graduates Highschool \n";

        }

        public override string ToString() => "<native fn>";
    }
}
