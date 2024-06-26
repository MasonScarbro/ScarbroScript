﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeParser
{
    public abstract class Expr
    {

		public interface IVisitor<T>
		{
			T VisitBinaryExpr(Binary expr);
			T VisitGroupingExpr(Grouping expr);
			T VisitLiteralExpr(Literal expr);
			T VisitUnaryExpr(Unary expr);
			


		}

		public class Binary : Expr
		{
			public Binary(Expr left, Token oper, Expr right)
			{
				this.left = left;
				this.oper = oper;
				this.right = right;
			}

			public T Accept<T>(IVisitor<T> visitor)
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

			public T Accept<T>(IVisitor<T> visitor)
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

			public T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitLiteralExpr(this);

			}

			public readonly object value;
		}

		public class Unary : Expr
		{
			public Unary(Token oper, Expr right)
			{
				this.oper = oper;
				this.right = right;
			}

			public T Accept<T>(IVisitor<T> visitor)
			{
				return visitor.VisitUnaryExpr(this);

			}

			public readonly Token oper;
			public readonly Expr right;
		}

	}
}
