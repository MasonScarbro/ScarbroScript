using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeParser
{
    public static class DerivativeTaker
    {

        public static void TakeDerivative(List<Formula> formulas)
        {
            try
            {
                foreach (Formula formula in formulas)
                {
                    Evaluate(formula);
                }
            } catch (Exception e)
            {
                
            }
        }

        private static object Evaluate(Formula formula)
        {
            if (formula is Formula.SumRule formulaSum)
            {
                return EvaluateSumRule(formulaSum);
            }
            if (formula is Formula.DifferenceRule formulaDif)
            {
                return EvaluateDifferenceRule(formulaDif);
            }
            if (formula is Formula.ProductRule formulaProd)
            {
                return EvaluateProductRule(formulaProd);
            }
            if (formula is Formula.ConstantRule)
            {
                return 0;
            }
            if (formula is Formula.VariableRule formulaVar)
            {
                return EvaluateNumerical(formulaVar.constant);
            }

            throw new Exception("Hmm It was none of those!!!");
        }

        private static object EvaluateSumRule(Formula.SumRule formula)
        {
            object left = Evaluate(formula.fx);
            object right = Evaluate(formula.gx);
            Console.WriteLine("(" + left.ToString() + " + " + right.ToString() + ")");
            return left + " + " + right;
        }

        private static object EvaluateProductRule(Formula.ProductRule formula)
        {
            object left = Evaluate(formula.fx);
            object right = Evaluate(formula.gx);
            Console.WriteLine($"({right} * ({ FormulaToString(formula.gx)}) + {left} * ({FormulaToString(formula.fx)}))");
            return $"({right} * ({ FormulaToString(formula.gx)}) + {left} * ({FormulaToString(formula.fx)}))";
        }

        private static object EvaluateDifferenceRule(Formula.DifferenceRule formula)
        {
            object left = Evaluate(formula.fx);
            object right = Evaluate(formula.gx);
            Console.WriteLine("(" + left.ToString() + " - " + right.ToString() + ")");
            return left + " - " + right;
        }


        private static string FormulaToString(Formula formula)
        {
            if (formula is Formula.SumRule sum)
            {
                return $"({FormulaToString(sum.fx)} + {FormulaToString(sum.gx)})";
            }
            if (formula is Formula.DifferenceRule difference)
            {
                return $"({FormulaToString(difference.fx)} - {FormulaToString(difference.gx)})";
            }
            if (formula is Formula.ProductRule product)
            {
                return $"({FormulaToString(product.fx)} * {FormulaToString(product.gx)})";
            }
            if (formula is Formula.ConstantRule constant)
            {
                return EvaluateNumerical(constant.expr).ToString();
            }
            if (formula is Formula.VariableRule variable)
            {
                return $"({EvaluateNumerical(variable.constant).ToString() + variable.var.lexeme})";
            }

            throw new Exception("Unknown formula type");
        }

        private static object EvaluateNumerical(Expr expr)
        {
            if (expr is Expr.Literal exprL) return exprL.value;
            if (expr is Expr.Unary exprU)
            {
                object right = EvaluateNumerical(exprU.right);
                switch (exprU.oper.type)
                {
                    case TokenType.MINUS:
                        return -(double)right;
                }
            }
            if (expr is Expr.Grouping exprG) return EvaluateNumerical(exprG.expression);
            throw new Exception(expr + " Type not recognized can only parse literals, unary's, and groupings");
        }
    }
}
