using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class ScarbroScript
    {
        static bool hadError = false;
        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: scarbro.exe [script]");
                Environment.Exit(64);
            } else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        private static void RunFile(String path)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                String SourceAsBytes = Encoding.Default.GetString(bytes);
                Run(SourceAsBytes);
                if (hadError) Environment.Exit(65);
            } catch (IOException)
            {

            }
        }

        private static void RunPrompt()
        {
            try
            {
                TextReader reader = Console.In;

                for (; ; ) // Infinite Loop
                {
                    Console.Write("> ");
                    String line = reader.ReadLine();
                    if (line == null) break;
                    Run(line);
                }
            }
            catch
            {

            }
        }
        //prints out the tokens the scanner emits!
        private static void Run(String source)
        {
            try
            {
                Scanner scanner = new Scanner(source); //Our own scanner not the built in one (for Java)
                List<Token> tokens = scanner.ScanTokens();
                Parser parser = new Parser(tokens);
                Expr expression = parser.Parse();

                if (hadError) return;

                Console.WriteLine(new AstPrinter().Print(expression));

            } catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
           

        }

        public static void Error(int line, String message)
        {
            report(line, "", message);
        }

        private static void report(int line, String where, String message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }

        public static void Error(Token token, String message)
        {
            if (token.type == TokenType.EOF)
            {
                report(token.line, " at end", message);
            }
            else
            {
                report(token.line, " at '" + token.lexeme + "'", message);
            }
        }
    }
}
