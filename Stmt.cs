using System.Collections.Generic;

namespace ScarbroScript
{ 

	public abstract class Stmt
	{
		public interface IVisitor<T> 
		{
			T VisitExpressionStmt(Expression stmt);
			T VisitPrintStmt(Print stmt);
			T VisitVarStmt(Var stmt);

			T VisitBlockStmt(Block stmt);
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
		public class Var : Stmt
		{
			public Var(Token name, Expr initializer) 
			{
				this.name = name;
				this.initializer = initializer;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitVarStmt(this);

			}

			public readonly Token name;
			public readonly Expr initializer;
		}

		public class Block : Stmt
		{
			public Block(List<Stmt> statements)
			{
				this.statements = statements;
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitBlockStmt(this);

			}

			public readonly List<Stmt> statements;
			
		}

		public abstract T Accept<T>(IVisitor<T> visitor);

	}

}
