using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeParser
{
    public abstract class Formula
    {


		public class ProductRule : Formula
		{
			public ProductRule(Formula fx, Token variable, Formula gx)
			{
				this.fx = fx;
				this.variable = variable;
				this.gx = gx;
			}

			public readonly Formula fx;
			public readonly Token variable;
			public readonly Formula gx;
		}

		public class QuotientRule : Formula
		{
			public QuotientRule(Formula hi, Token variable, Formula lo)
			{
				this.hi = hi;
				this.variable = variable;
				this.lo = lo;
			}

			public readonly Formula hi;
			public readonly Token variable;
			public readonly Formula lo;
		}

		public class ChainRule : Formula
		{
			public ChainRule(Formula outer, Token variable, TokenType keyword, Formula inner)
			{
				this.outer = outer;
				this.variable = variable;
				this.keyword = keyword;
				this.inner = inner;
			}

			public readonly Formula outer;
			public readonly Token variable;
			public readonly TokenType keyword;
			public readonly Formula inner;
		}

		public class PowerRule : Formula
		{
			public PowerRule(Expr constant, Token variable, Formula exponent)
			{
				this.constant = constant;
				this.variable = variable;
				this.exponent = exponent;
			}

			public readonly Expr constant;
			public readonly Token variable;
			public readonly Formula exponent;
		}

		public class DifferenceRule : Formula
		{
			public DifferenceRule(Formula fx, Token variable, Formula gx)
			{
				this.fx = fx;
				this.variable = variable;
				this.gx = gx;
			}

			public readonly Formula fx;
			public readonly Token variable;
			public readonly Formula gx;
		}

		public class SumRule : Formula
		{
			public SumRule(Formula fx, Token variable, Formula gx)
			{
				this.fx = fx;
				this.variable = variable;
				this.gx = gx;
			}

			public readonly Formula fx;
			public readonly Token variable;
			public readonly Formula gx;
		}

		public class ConstantRule : Formula
		{
			public ConstantRule(Expr expr)
			{
				this.expr = expr;
			}

			public readonly Expr expr;
			
		}

		public class VariableRule : Formula
		{
			public VariableRule(Token var, Expr constant)
			{
				this.var = var;
				this.constant = constant;
			}

			public readonly Token var;
			public readonly Expr constant;

		}



	}
}
