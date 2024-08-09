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
        public class LinterpreterError : Exception
        {
            public int line;
            public string problemChild;
            public LinterpreterError(string message, int line, string problemChild) : base(message)
            {
                this.line = line;
                this.problemChild = problemChild;
            }
        }

        public static Dictionary<string, ScopedObject> scopedObjects = new Dictionary<string, ScopedObject>();
       
        public static List<Exception> lintErrors = new List<Exception>();
        public class Scoper
        {

            public Scoper() { }


            public Scoper(List<Stmt> stmts)
            {
                foreach (Stmt stmt in stmts)
                {
                    if (stmt is Stmt.Var stmtv)
                    {
                        Program.logger.Log("Inside Scoper Just found a Variable");
                        var variableType = EvaluateType(stmtv.initializer);
                        scopedObjects[stmtv.name.lexeme] = new ScopedObject(variableType);
                    }
                    if (stmt is Stmt.Class stmtc)
                    {
                        List<UserBuiltMethod> _methods = new List<UserBuiltMethod>();
                        foreach (Stmt.Function stmtf in stmtc.methods)
                        {
                            _methods.Add(
                                new UserBuiltMethod(
                                    stmtf.name.lexeme,
                                    stmtf.parameters.Count
                                    )
                                );
                        }
                        scopedObjects[stmtc.name.lexeme] = new ScopedObject(null, _methods);
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
            


            private object EvaluateType(Expr value)
            {
                Program.logger.Log("Evaluating Type... " + value);

                //Example if the variable is init to another it should match the previous variable
                if (value is Expr.Variable ev)
                {
                    Program.logger.Log("Value was a variable");
                    if (!scopedObjects.ContainsKey(ev.name.lexeme))
                    {
                        lintErrors.Add(new LinterpreterError
                            (
                            "Variable not found in file",
                            ev.name.line,
                            ev.name.lexeme
                            ));
                    }
                    //else
                    return scopedObjects[ev.name.lexeme].ObjectType;
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
                        if (scopedObjects.ContainsKey(expcv.name.lexeme))
                        {
                            return scopedObjects[expcv.name.lexeme];
                        }

                    }
                }
                if (value is Expr.Dict) return typeof(HashSet<>);
                if (value is Expr.Binary expb)
                {
                    object left = EvaluateType(expb.left);
                    object right = EvaluateType(expb.right);

                    Program.logger.Log("Inside Binary and Left Was: " + left + " of type " + left?.GetType());
                    Program.logger.Log("Inside Binary and Right Was: " + right + " of type " + right?.GetType());

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
                            if ((Type)left == typeof(double) && (Type)right == typeof(double))
                            {
                                Program.logger.Log("Was a Double (Binary)");
                                return typeof(double);
                            }
                            if ((Type)left == typeof(string) || (Type)right == typeof(string))
                            {
                                Program.logger.Log("Was a string (Binary)");
                                return typeof(string);
                            }
                            //else
                            Program.logger.Log("Was Neither Double nor String :(");
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
                if (value is Expr.Literal expl)
                {
                    //If literal just get type
                    Program.logger.Log("Inside Literal and was: " + expl.value.GetType());
                    return expl.value.GetType();
                }
                if (value is Expr.Logical)
                {
                    return typeof(bool);
                }
                if (value is Expr.Grouping expg)
                {
                    return EvaluateType(expg.expression);
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


            public List<Exception> GetLinterpreterErrors()
            {
                return lintErrors;
            }

            public Dictionary<string, ScopedObject> GetscopedObjects ()
            {
                return scopedObjects;
            }
        }


    }
}
