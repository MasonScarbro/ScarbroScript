using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeParser
{
    class Program
    {
        public static string src = "cos(4)"; // just a test value for now

        static void Main(string[] args)
        {

            Tokenizer tokenner = new Tokenizer(src);
            List<Token> tokens = tokenner.ScanTokens();
            PrettyPrintTokens(tokens);
            MathParser mparser = new MathParser(tokens);
            List<Formula> formulas = mparser.Parse();
            PrettyPrintFormulas(formulas);

        }

        private static void PrettyPrintTokens(List<Token> tokens)
        {
            foreach (Token tolkien in tokens)
            {
                Console.WriteLine(tolkien.type);
                if (tolkien.type is TokenType.VARIABLE)
                {
                    Console.WriteLine("Variable is: " + tolkien.lexeme);
                }
            }
        }

        private static void PrettyPrintFormulas(List<Formula> formulas)
        {
            foreach (Formula formula in formulas)
            {
                Console.WriteLine(formula);
                if (formula is Formula.QuotientRule forq)
                {
                    Console.WriteLine("The Quotient Rule is : \n");
                    Console.WriteLine(forq.hi);
                    Console.WriteLine(forq.variable.lexeme);
                    Console.WriteLine(forq.lo);

                }
            }
        }

        public static Exception Error(Token tok, String message)
        {
            report(tok, "", message);
            return new Exception();
        }

        private static void report(Token tok, String where, String message)
        {
            Console.WriteLine("Error Parsing Math Expr at " + tok +  where + ": " + message);
            
        }
    }
}
