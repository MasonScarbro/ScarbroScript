using System.Collections.Generic;

namespace ScarbroScript
{ 

	public abstract class Stmt
	{
		public interface IVisitor<T> 
		{
			T VisitBlockStmt(Block stmt);
			T VisitExpressionStmt(Expression stmt);
			T VisitIfStmt(If stmt);

			T VisitTryCatchStmt(TryCatch stmt);
			T VisitPrintStmt(Print stmt);
			T VisitVarStmt(Var stmt);
			T VisitWhileStmt(While stmt);

			T VisitBreakStmt(Break stmt);

			T VisitFunctionStmt(Function stmt);

			T VisitClassStmt(Class stmt);

			T VisitReturnStmt(Return stmt);

			T VisitCaseStmt(Case Stmt);

			T VisitSwitchStmt(Switch Stmt);

			T VisitImportStmt(Import stmt);
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
		public class If : Stmt
		{
			public If(Expr condition, Stmt thenBranch, Stmt elseBranch) 
			{
				this.condition = condition;
				this.thenBranch = thenBranch;
				this.elseBranch = elseBranch;
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitIfStmt(this);

			}

			public readonly Expr condition;
			public readonly Stmt thenBranch;
			public readonly Stmt elseBranch;
		}

		public class TryCatch : Stmt
		{
			public TryCatch(Stmt tryBranch, Token instance, Stmt catchBranch)
			{
				
				this.tryBranch = tryBranch;
				this.instance = instance;
				this.catchBranch = catchBranch;
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitTryCatchStmt(this);

			}

			
			public readonly Stmt tryBranch;
			public readonly Token instance;
			public readonly Stmt catchBranch;
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
		public class While : Stmt
		{
			public While(Expr condition, Stmt body) 
			{
				this.condition = condition;
				this.body = body;
				
			}

			public override T Accept<T>(IVisitor<T> visitor) 
			{
				return visitor.VisitWhileStmt(this);

			}

			public readonly Expr condition;
			public readonly Stmt body;
		}

		public class Break : Stmt
		{
			public Break()
			{
				
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitBreakStmt(this);

			}

		
			
		}

		public class Function : Stmt
		{
			public Function(Token name, List<Token> parameters, List<Stmt> body)
			{

				this.name = name;
				this.parameters = parameters;
				this.body = body;
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitFunctionStmt(this);

			}

			public readonly Token name;
			public readonly List<Token> parameters;
			public readonly List<Stmt> body;

		}

		public class Class : Stmt
		{
			public Class(Token name, List<Stmt.Function> methods)
			{

				this.name = name;
				this.methods = methods;
				
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitClassStmt(this);

			}

			public readonly Token name;
			public readonly List<Stmt.Function> methods;
			

		}

		public class Case : Stmt
		{
			public Case(Expr condition, List<Stmt> thenBranch)
			{

				this.condition = condition;
				this.thenBranch = thenBranch;
				
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitCaseStmt(this);

			}

			public readonly Expr condition;
			public readonly List<Stmt> thenBranch;
			

		}

		public class Switch : Stmt
		{
			public Switch(Expr comparable, Stmt thenBranch)
			{

				this.comparable = comparable;
				this.thenBranch = thenBranch;

			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitSwitchStmt(this);

			}

			public readonly Expr comparable;
			public readonly Stmt thenBranch;


		}

		public class Return : Stmt
		{
			public Return(Token keyword, Expr value)
			{

				this.keyword = keyword;
				this.value = value;
				
			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitReturnStmt(this);

			}

			public readonly Token keyword;
			public readonly Expr value;

		}

		public class Import : Stmt
		{
			public Import(Token keyword, string fileName)
			{

				this.keyword = keyword;
				this.fileName = fileName;

			}

			public override T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitImportStmt(this);

			}

			public readonly Token keyword;
			public readonly string fileName;

		}

		public abstract T Accept<T>(IVisitor<T> visitor);

	}

}
