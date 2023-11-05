using System.Collections.Generic;

namespace ScarbroScript
{ 

	public abstract class Stmt
	{
		public interface IVisitor<T> 
		{
			T VisitExpressionStmt(Expression stmt);
			T VisitPrintStmt(Print stmt);
		}
		public class Expression : Stmt
		{
			public Expression(Expr expression) 
			{
				this.expression = expression;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitExpressionStmt(this);

			}

			public readonly Expr expression;
		}
		public class Print : Stmt
		{
			public Print(Expr expression) 
			{
				this.expression = expression;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitPrintStmt(this);

			}

			public readonly Expr expression;
		}

		public abstract T Accept<T>(IVisitor<T> visitor);

	}

}
