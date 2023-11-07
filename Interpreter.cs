﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class Interpreter : Expr.IVisitor<Object>, Stmt.IVisitor<object>
    {
        private Enviornment enviornment = new Enviornment();
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
                    if ((left.GetType() == typeof(String) && right.GetType() == typeof(Double)) || (right.GetType() == typeof(Double) && left.GetType() == typeof(String)))
                    {
                        return (string)left + (string)right.ToString();
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

        private Object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
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
            enviornment.Assign(expr.name, value);
            return value;
        }

        public Object VisitVariableExpr(Expr.Variable expr)
        {
            return enviornment.Get(expr.name);
        }

        public object VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.statements, new Enviornment(enviornment));
            return null;
        }

        // END STATEMENT INTERPRETING PART //

        private bool IsTruthy(Object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(bool)) return (bool)obj;
            //else
            return true;
        }


        
    }
}
