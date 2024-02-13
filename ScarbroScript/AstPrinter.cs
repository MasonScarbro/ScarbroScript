using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    // NOT NEEDED
    class AstPrinter : Expr.IVisitor<String>
    {
        public String Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string VisitArrayExpr(Expr.Array expr)
        {
            throw new NotImplementedException();
        }

        //Uses the Visitor pattern to pass in a Binary Expr value (Object) and return a String (inplace of the genric type)
        public String VisitBinaryExpr(Expr.Binary expr)
        {
            Object left = Print(expr.left);
            Object right = Print(expr.right);

            switch (expr.oper.type)
            {
                case TokenType.MINUS:

                    return (double.Parse((string)left) - double.Parse((string)right)).ToString(); ;
                case TokenType.SLASH:

                    //Checks if dividing by zero

                    return (double.Parse((string)left) / double.Parse((string)right)).ToString(); ;
                case TokenType.STAR:

                    return (double.Parse((string)left) * double.Parse((string)right)).ToString();
                case TokenType.PLUS:
                    //Console.WriteLine((double.Parse((string)left) + double.Parse((string)right)).ToString());
                    return (double.Parse((string)left) + double.Parse((string)right)).ToString();


                    throw new RuntimeError(expr.oper, "Make sure that both operators are either a double or string (Remember string concatnation is allowed)!");
                    break;
            }
            return null;
        }
        public String VisitGroupingExpr(Expr.Grouping expr)
        {
            return Print(expr.expression);
        }
        public String VisitLiteralExpr(Expr.Literal expr)
        {
            if (expr.value == null) return "nil";
            return expr.value.ToString();
        }
        public String VisitUnaryExpr(Expr.Unary expr)
        {
            return Parenthesize(expr.oper.lexeme, expr.right);
        }

        private String Parenthesize(String name, params Expr[] exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (Expr expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }

        string Expr.IVisitor<string>.VisitAssignExpr(Expr.Assign expr)
        {
            throw new NotImplementedException();
        }


        string Expr.IVisitor<string>.VisitCallExpr(Expr.Call expr)
        {
            throw new NotImplementedException();
        }


        string Expr.IVisitor<string>.VisitLogicalExpr(Expr.Logical expr)
        {
            throw new NotImplementedException();
        }


        string Expr.IVisitor<string>.VisitVariableExpr(Expr.Variable expr)
        {
            throw new NotImplementedException();
        }
    }
}
