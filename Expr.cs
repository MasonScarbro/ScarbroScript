using System.Collections.Generic;

namespace ScarbroScript
{ 

	public abstract class Expr
	{
		public interface IVisitor<T> 
		{
			T VisitAssignExpr(Assign expr);
			T VisitBinaryExpr(Binary expr);
			T VisitGroupingExpr(Grouping expr);
			T VisitLiteralExpr(Literal expr);
			T VisitLogicalExpr(Logical expr);
			T VisitUnaryExpr(Unary expr);
			T VisitVariableExpr(Variable expr);
			T VisitArrayExpr(Array expr);
			T VisitCallExpr(Call expr);
		}
		public class Assign : Expr
		{
			public Assign(Token name, Expr value) 
			{
				this.name = name;
				this.value = value;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitAssignExpr(this);

			}

			public readonly Token name;
			public readonly Expr value;
		}
		public class Binary : Expr
		{
			public Binary(Expr left, Token oper, Expr right) 
			{
				this.left = left;
				this.oper = oper;
				this.right = right;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitBinaryExpr(this);

			}

			public readonly Expr left;
			public readonly Token oper;
			public readonly Expr right;
		}
		public class Grouping : Expr
		{
			public Grouping(Expr expression) 
			{
				this.expression = expression;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitGroupingExpr(this);

			}

			public readonly Expr expression;
		}
		public class Literal : Expr
		{
			public Literal(object value) 
			{
				this.value = value;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitLiteralExpr(this);

			}

			public readonly object value;
		}
		public class Logical : Expr
		{
			public Logical(Expr left, Token oper, Expr right) 
			{
				this.left = left;
				this.oper = oper;
				this.right = right;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitLogicalExpr(this);

			}

			public readonly Expr left;
			public readonly Token oper;
			public readonly Expr right;
		}
		public class Unary : Expr
		{
			public Unary(Token oper, Expr right) 
			{
				this.oper = oper;
				this.right = right;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitUnaryExpr(this);

			}

			public readonly Token oper;
			public readonly Expr right;
		}
		public class Variable : Expr
		{
			public Variable(Token name) 
			{
				this.name = name;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitVariableExpr(this);

			}

			public readonly Token name;
		}

		public class Array : Expr
		{
			public Array(Token bracket, List<Expr> elements)
			{
				this.bracket = bracket;
				this.elements = elements;
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitArrayExpr(this);

			}

			public readonly Token bracket;
			public readonly List<Expr> elements;
		}

		public class Call : Expr
        {
			public Call(Expr callee, Token paren, List<Expr> arguments)
            {
				this.callee = callee;
				this.paren = paren;
				this.arguments = arguments;
            }

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitCallExpr(this);

			}

			public readonly Expr callee;
			public readonly Token paren;
			public readonly List<Expr> arguments;


		}

		public abstract T Accept<T>(IVisitor<T> visitor);

	}

}
