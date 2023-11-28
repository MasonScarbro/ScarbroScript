using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class Interpreter : Expr.IVisitor<Object>, Stmt.IVisitor<object>
    {
        private class BreakException : Exception { };

        public static readonly Enviornment globals = new Enviornment();
        private Enviornment enviornment;
        private readonly Dictionary<Expr, int> locals = new Dictionary<Expr, int>();


        public Interpreter()
        {
            globals.Define("clock", new Clock());
            globals.Define("evalExpr", new EvaluateExpression());
            globals.Define("parseToNum", new ParseToNum());
            globals.Define("parseToString", new ParseToString());
            globals.Define("Scarbro", new Scarbro());
            enviornment = globals;
        }



        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            } catch (RuntimeError error)
            {
                ScarbroScript.RuntimeErrorToCons(error);
            }
        }

       
        

        /// <summary>
        /// The parser took the value and stuck it in the literal tree node,
        /// so to evaluate a literal, we pull it back out.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns return=expressions value></returns>
        public Object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value;
        }


        public Object VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.expression);
        }


        public Object VisitUnaryExpr(Expr.Unary expr)
        {
            Object right = Evaluate(expr.right);

            switch (expr.oper.type)
            {
                case TokenType.MINUS:
                    return -(double)right;
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.INCREMENT:
                    HandlePlusIncr(expr, right);
                    return null;
                case TokenType.DECREMENT:
                    HandleMinusIncr(expr, right);
                    return null;

            }

            // Unreachable
            return null;
        }


        // ERROR HANDLING //
        private void CheckNumOperand(Token oper, Object operand)
        {
            if (operand.GetType() == typeof(double)) { return; }
            throw new RuntimeError(oper, "Operand must be a number");

        }

        private void CheckNumberOperands(Token oper, Object left, Object right)
        {
            if (left.GetType() == typeof(double) && right.GetType() == typeof(double)) return;
            //else
            throw new RuntimeError(oper, "Operands Must be Numbers!");
        }
        // END ERROR HANDLING //

        public Object VisitBinaryExpr(Expr.Binary expr)
        {
            Object left = Evaluate(expr.left);
            Object right = Evaluate(expr.right);

            switch (expr.oper.type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expr.oper, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.oper, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.oper, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.oper, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    CheckNumberOperands(expr.oper, left, right);//err
                    return (double)left - (double)right;
                case TokenType.SLASH:

                    CheckNumberOperands(expr.oper, left, right);
                    //Checks if dividing by zero
                    if ((double)right == 0)
                    {
                        throw new RuntimeError(expr.oper, "This isnt math class... But you cant divide by zero unless you are taking the limit as an interger n aproaches 0 which would be infinity!");
                    }
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.oper, left, right);
                    return (double)left * (double)right;
                case TokenType.EXPON:
                    CheckNumberOperands(expr.oper, left, right);
                    return Math.Pow((double)left, (double)right);
                case TokenType.PLUS:
                    if (left.GetType() == typeof(Double) && right.GetType() == typeof(Double))
                    {
                        return (double)left + (double)right;
                    }

                    if (left.GetType() == typeof(String) && right.GetType() == typeof(String))
                    {
                        return (string)left + (string)right;
                    }
                    //String Concatenation!
                    if ((left is String && right is Double))
                    {
                        return (string)left + (string)right.ToString();
                    } 
                    else if ((left is Double) && (right is String))
                    {
                        return (string)left.ToString() + (string)right;
                    }
                    throw new RuntimeError(expr.oper, "Make sure that both operators are either a double or string (Remember string concatnation is allowed)!");
                    break;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
            }

            return null;
        }

        private bool IsEqual(Object a, Object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            //else
            return a.Equals(b); //Handles strings and doubles
        }

        private String Stringify(Object obj)
        {
            if (obj == null) return "nil";
            
            if (obj.GetType() == typeof(double))
            {
                String text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }
            return obj.ToString();
        }

        

        /// <summary>
        /// The this statements can seem a little confusing but dont let them fool you
        /// it does exactly what you think an interpreter would do with blocks
        /// first to execute the code within the "scope" (the block)
        /// It updates the interpreters global enviornment to the passed in enviornment 
        /// runs through each of those statments and executes them then using the 
        /// finally block it restores the enviornment to the old one!
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="enviornment"></param>
        public void ExecuteBlock(List<Stmt> statements, Enviornment enviornment)
        {
            Enviornment previous = this.enviornment;
            try
            {
                this.enviornment = enviornment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.enviornment = previous;
            }
            
        }

        // STATEMENT INTERPRETING PART //

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public object VisitPrintStmt(Stmt.Print stmt)
        {
            Object value = Evaluate(stmt.expression);
            Console.WriteLine(Stringify(value)); // Write what they wrote 
            return null;
        }

        public object VisitVarStmt(Stmt.Var stmt)
        {
            //just sets it to null if no initilizlizer is given
            Object value = null;
            if (stmt.initializer != null)
            {
                value = Evaluate(stmt.initializer);
            }

            enviornment.Define(stmt.name.lexeme, value);
            return null;
        }

        public Object VisitAssignExpr(Expr.Assign expr)
        {
            Object value = Evaluate(expr.value);
            

            var found = locals.TryGetValue(expr, out var distance);
            if (found)
            {
                Console.WriteLine($"Assigned variable {expr.name.lexeme} with value: {value} in scope: {distance}");
                enviornment.AssignAt(distance, expr.name, value);
            } 
            else
            {
                globals.Assign(expr.name, value);
            }

            return value;
        }

        public Object VisitVariableExpr(Expr.Variable expr)
        {
            return LookUpVariable(expr.name, expr);
        }

        public object VisitBlockStmt(Stmt.Block stmt)
        {
            Console.WriteLine("Entering a new environment...");
            ExecuteBlock(stmt.statements, new Enviornment(enviornment));
            Console.WriteLine("Exiting the environment...");
            return null;
        }

        public object VisitIfStmt(Stmt.If stmt)
        {
            if (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.thenBranch);
            }
            else
            {
                Execute(stmt.elseBranch);
            }
            return null;
        }

        /// <summary>
        /// Remmeber that this is dynamically typed
        /// and pay attention to the nested if's
        /// So this evals left and checks if the oper is an or
        /// if it is than it checks left is truth if is than return
        /// if not it breaks and evals right
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitLogicalExpr(Expr.Logical expr)
        {
            Object left = Evaluate(expr.left);

            
            if (expr.oper.type == TokenType.OR)
            {
                if (IsTruthy(left)) return left;
            }
            else
            {
                if (!IsTruthy(left)) return left;
            }

            return Evaluate(expr.right);
        }

        public object VisitWhileStmt(Stmt.While stmt)
        {
            try
            {
                while (IsTruthy(Evaluate(stmt.condition)))
                {
                    Execute(stmt.body);
                }

            } catch (BreakException)
            {
                //Breaks out of the loop, Not very pretty but it works
            }
            
            return null;
        }

        public object VisitBreakStmt(Stmt.Break stmt)
        {
            Console.WriteLine("Encountered a break statement");
            throw new BreakException();
        }

        // END STATEMENT INTERPRETING PART //

        // FUNCTION INTERPRETING //

        public Object VisitCallExpr(Expr.Call expr)
        {
            Object callee = Evaluate(expr.callee);

            List<Object> arguments = new List<Object>();
            foreach (Expr argument in expr.arguments)
            {
                arguments.Add(Evaluate(argument));
            }
            Console.WriteLine(callee.GetType());
            if (!(callee is ScarbroScriptCallable)) 
            {
                throw new RuntimeError(expr.paren, "Can only call functions and classes.");
            }

           

            ScarbroScriptCallable function = (ScarbroScriptCallable)callee;
            if (arguments.Count != function.Arity)
            {
                throw new RuntimeError(expr.paren, "Expected " + function.Arity + " Args but got " + arguments.Count);
            }
            return function.Call(this, arguments);
        }

        public object VisitFunctionStmt(Stmt.Function stmt)
        {
            ScarbroScriptFunction function = new ScarbroScriptFunction(stmt , enviornment);
            enviornment.Define(stmt.name.lexeme, function);
            return null;
        }

        public object VisitReturnStmt(Stmt.Return stmt)
        {
            Object value = null;
            if (stmt.value != null) value = Evaluate(stmt.value);
            // Just like the break Stmt! we basically
            // catch an exception to exit out of the block
            // or more rather the visitfunction here!
            throw new Return(value); 
        }

        // END FUNCTION INTERPRETING //

        /// <summary>
        /// This Is a Helper method that is vey similar to the VisitVarExpr
        /// This is because it takes in a unary and detirmines if its "right"
        /// is a variable if it is then we do the same thing (basically) and assign 
        /// the variable! We add one to the right which is taken from the visitUnary when its evaluated
        /// and the value is now assigned to the 'right' and associated with its evaluated right + 1
        /// Kinda funny sounding but its pretty simple actually.
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="right"></param>
        public void HandlePlusIncr(Expr.Unary expr, Object right)
        {
            if (expr.right is Expr.Variable variableExpr)
            {
                var variableName = variableExpr.name;

                if (locals.TryGetValue(expr.right, out var distance))
                {
                    Console.WriteLine($"Assigned variable {variableName} with value: {right} in scope: {distance}");
                    enviornment.AssignAt(distance, variableName, (double)right + 1);
                }
                else
                {
                    globals.Assign(variableExpr.name, (double)right + 1);
                }
            }
            else
            {
                throw new RuntimeError(expr.oper, "You Cannot Use the Increment Operator on a non var");
            }
            return;
        }

        public void HandleMinusIncr(Expr.Unary expr, Object right)
        {
            if (expr.right is Expr.Variable variableExpr)
            {
                var variableName = variableExpr.name;

                if (locals.TryGetValue(expr.right, out var distance))
                {
                    Console.WriteLine($"Assigned variable {variableName} with value: {right} in scope: {distance}");
                    enviornment.AssignAt(distance, variableName, (double)right - 1);
                }
                else
                {
                    globals.Assign(variableExpr.name, (double)right - 1);
                }
            }
            else
            {
                throw new RuntimeError(expr.oper, "You Cannot Use the Increment Operator on a non var");
            }
            return;
        }

        private bool IsTruthy(Object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(bool)) return (bool)obj;
            //else
            return true;
        }

        private Object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private void Execute(Stmt stmt)
        {
            Console.WriteLine("Executing statement: " + stmt);
            stmt.Accept(this);
        }

        private Object LookUpVariable(Token name, Expr expr)
        {
            var wasFound = locals.TryGetValue(expr, out var distance);
            if (wasFound)
            {
                return enviornment.GetAt(distance, name.lexeme);
            } 
            else
            {
                return globals.Get(name);
            }

        }

        public void Resolve(Expr expr, int depth)
        {
            locals.Add(expr, depth);
        }

    }
}
