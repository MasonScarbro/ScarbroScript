using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarbroScript;

namespace ScarbroScriptLSP.Analysis
{

    public static class Linterpreter
    {
        public class LinterpreterError : FormatException
        {
            public int line;
            public string problemChild;
            public LinterpreterError(string message, int line, string problemChild) : base(message)
            {
                this.line = line;
                this.problemChild = problemChild;
            }
        }

        public static Dictionary<string, object> scopedVariables = new Dictionary<string, object>();
        public static Dictionary<string, object> scopedClasses = new Dictionary<string, object>();
        public static List<LinterpreterError> lintErrors = new List<LinterpreterError>();
        public class Scoper
        {

            public Scoper()
            {

            }


            public Scoper(List<Stmt> stmts)
            {
                foreach (Stmt stmt in stmts)
                {
                    if (stmt is Stmt.Var stmtv)
                    {
                        Program.logger.Log("Inside Scoper Just found a Variable");
                        StoreScopedObject(stmtv.name.lexeme, EvaluateType(stmtv.initializer));
                    }
                    if (stmt is Stmt.Class stmtc)
                    {

                    }
                }
            }





            /// <summary>
            /// for now this only handles variables but since I only
            /// need the list of methods for classes I can probably
            /// check ifn its a ''type'' or a list of strings (name of each method)
            /// </summary>
            /// <param name="name"></param>
            /// <param name="type"></param>
            public void StoreScopedObject(string name, object type)
            {
                scopedVariables[name] = type;
            }


            private object EvaluateType(Expr value)
            {
                Program.logger.Log("Evaluating Type... " + value);

                //Example if the variable is init to another it should match the previous variable
                if (value is Expr.Variable ev)
                {
                    Program.logger.Log("Value was a variable");
                    if (!scopedVariables.ContainsKey(ev.name.lexeme))
                    {
                        lintErrors.Add(new LinterpreterError
                            (
                            "Variable not found in file",
                            ev.name.line,
                            ev.name.lexeme
                            ));
                    }
                    //else
                    return scopedVariables[ev.name.lexeme];
                }
                //if its an array just return the type so it can be scoped
                if (value is Expr.Array)
                {
                    Program.logger.Log("Inside Scoper, found arr type!");
                    return typeof(List<>);
                }
                if (value is Expr.Call expc)
                {
                    Program.logger.Log("It was a Call");
                    if (expc.callee is Expr.Variable expcv)
                    {
                        Program.logger.Log("Call was a Var and Name was: " + expcv.name.lexeme);
                        if (expcv.name.lexeme.Equals("Queue")) return typeof(Queue<>);
                        if (expcv.name.lexeme.Equals("Stack")) return typeof(Stack<>);
                        if (expcv.name.lexeme.Equals("Dict")) return typeof(HashSet<>);

                    }
                }
                if (value is Expr.Dict) return typeof(HashSet<>);
                if (value is Expr.Binary expb)
                {
                    object left = EvaluateType(expb.left);
                    object right = EvaluateType(expb.right);

                    switch (expb.oper.type)
                    {
                        case TokenType.GREATER:
                        case TokenType.LESS_EQUAL:
                        case TokenType.GREATER_EQUAL:
                        case TokenType.LESS:
                        case TokenType.BANG_EQUAL:
                        case TokenType.EQUAL_EQUAL:
                            return typeof(bool);
                        case TokenType.SLASH:
                            CheckOperands(expb, left, right);
                            if ((double)right == 0)
                            {
                                lintErrors.Add(
                                    new LinterpreterError(
                                        "Cant Divide By Zero",
                                        expb.oper.line,
                                        expb.oper.lexeme
                                        )
                                    );
                            }
                            return typeof(double);
                        case TokenType.MINUS:
                        case TokenType.STAR:
                            CheckOperands(expb, left, right);
                            return typeof(double);
                        case TokenType.PLUS:
                            if (left is double && right is double)
                            {
                                return typeof(double);
                            }
                            if (left is double && right is string || right is double && left is string)
                            {
                                return typeof(string);
                            }
                            //else
                            lintErrors.Add(
                                new LinterpreterError(
                                    "Type mismatch between operands (Must be double or string)",
                                    expb.oper.line,
                                    expb.oper.lexeme
                                    )
                                );
                            return null;
                    }
                }

                return null;
            }



            private void CheckOperands(Expr.Binary expb, object left, object right)
            {
                if (left is double && right is double) return;
                //else
                lintErrors.Add(
                    new LinterpreterError(
                        "Operands Must Be Numbers",
                        expb.oper.line,
                        expb.oper.lexeme
                        )
                    );
            }
        }


    }
}
