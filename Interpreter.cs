using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class Interpreter : Expr.IVisitor<Object>
    {

        public void Interpret(Expr expression)
        {
            try
            {
                Object value = Evaluate(expression);
                Console.WriteLine(Stringify(value));
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
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.oper, left, right);
                    return (double)left * (double)right;
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
                    if (left.GetType() == typeof(String) && right.GetType() == typeof(Double))
                    {
                        return (string)left + (string)right.ToString();
                    }
                    throw new RuntimeError(expr.oper, "POOOOP");
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

        private Object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private bool IsTruthy(Object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(bool)) return (bool)obj;
            //else
            return true;
        }

        
    }
}
