using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeParser
{
    class MathParser
    {
        private readonly List<Token> tokens;
        private int current = 0;
        private List<Formula> formulas;
        public MathParser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<Formula> Parse()
        {
            formulas = new List<Formula>();
            while (!IsAtEnd())
            {
                formulas.Add(ParseFullExpression());
            }
            return formulas;
        }


        private Formula ParseFullExpression()
        {
            if (Match(TokenType.LEFT_PAREN))
            {
                Formula expr = ParseFullExpression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return expr;
            }
            return ParseTerm();
        }

        private Formula ParseTerm()
        {
            Formula formula = ParseFactor();
            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                Token operatorToken = Previous();
                Formula right = ParseFactor();
                Token variable = FindRelevantVariable(formula, right);

                if (operatorToken.type == TokenType.PLUS)
                {
                    formula = new Formula.SumRule(formula, variable, right);
                }
                else
                {
                    formula = new Formula.DifferenceRule(formula, variable, right);
                }
            }

            return formula;
        }

        private Formula ParseFactor()
        {
            Formula formula = ParseExpon();

            while (Match(TokenType.SLASH))
            {
                Token operatorToken = Previous();
                Formula right = Primary();
                Token variable = FindRelevantVariable(formula, right);

                formula = new Formula.QuotientRule(formula, variable, right);
            }
            while (Match(TokenType.STAR))
            {
                Formula right = Primary();
                Token variable = FindRelevantVariable(formula, right);

                formula = new Formula.QuotientRule(formula, variable, right);
            }

            return formula;
        }

        


        private Formula ParseExpon()
        {
            Formula formula = Primary();

            while (Match(TokenType.EXPON))
            {
                Token operatorToken = Previous();
                Formula right = Primary();
                Token variable = FindRelevantVariable(formula, right);
                if (right is Formula.ConstantRule && formula is Formula.ConstantRule constant)
                {
                    formula = new Formula.PowerRule(constant.expr, variable, right);
                }
                else
                {
                    formula = new Formula.ChainRule(formula, variable, TokenType.NULL, right);
                }
                
            }

            return formula;
        }

        
        private Formula Primary()
        {
            if (Match(TokenType.COS)) ParseCosExpr();
            if (Match(TokenType.SIN)) ParseSinExpr();
            if (Match(TokenType.TAN)) ParseTanExpr();

            if (Match(TokenType.NUMBER))
            {
                Expr num = new Expr.Literal(Previous().literal);
                if (Match(TokenType.VARIABLE))
                {
                    return new Formula.VariableRule(Previous(), num); 
                }
                else
                {
                    return new Formula.ConstantRule(num);
                }

            }
            if (Match(TokenType.LEFT_PAREN))
            {
                Formula expr = ParseFullExpression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return expr;
            }
            //else
            try
            {
                return PopFormula();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Token " + tokens[current].type + " ERROR IN PARSER: " + e.Message);
                throw new Exception("Unexpected Token " + tokens[current].type);
            }

            
        }

        private Formula ParseCosExpr()
        {
            Formula inner = Primary();
            Formula outer = null;
            //Implied Multiplication I.e 4cos(...)
            if (PreviousFormula() is Formula.ConstantRule && !IsOperator(Previous().type))
            {
                outer = PopFormula();
                
                
            }
            Token variable = FindRelevantVariable(inner, outer);
            // if there is no variable its a constant anyways lets just handle that in the parser
            if (variable.type == TokenType.NULL)
            {
                return new Formula.ConstantRule(new Expr.Literal(0));
            }
            //else
            return new Formula.ChainRule(outer, variable, TokenType.COS, inner);
        }
        private Formula ParseSinExpr()
        {
            Formula inner = Primary();
            Formula outer = null;
            //Implied Multiplication I.e 4cos(...)
            if (PreviousFormula() is Formula.ConstantRule && !IsOperator(Previous().type))
            {
                outer = PopFormula();


            }
            Token variable = FindRelevantVariable(inner, outer);
            // if there is no variable its a constant anyways lets just handle that in the parser
            if (variable.type == TokenType.NULL)
            {
                return new Formula.ConstantRule(new Expr.Literal(0));
            }
            //else
            return new Formula.ChainRule(outer, variable, TokenType.SIN, inner);
        }
        private Formula ParseTanExpr()
        {
            Formula inner = Primary();
            Formula outer = null;
            //Implied Multiplication I.e 4cos(...)
            if (PreviousFormula() is Formula.ConstantRule && !IsOperator(Previous().type))
            {
                outer = PopFormula();


            }
            Token variable = FindRelevantVariable(inner, outer);
            // if there is no variable its a constant anyways lets just handle that in the parser
            if (variable.type == TokenType.NULL)
            {
                return new Formula.ConstantRule(new Expr.Literal(0));
            }
            //else
            return new Formula.ChainRule(outer, variable, TokenType.TAN, inner);
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private Token Consume(TokenType type, String message)
        {
            if (Check(type)) return Advance();

            throw Program.Error(Peek(), message);
            
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }

        private bool IsAtEnd()
        {
            return Peek().type == TokenType.EOF;
        }

        private Token LookBack(int numOfTokens)
        {
            return tokens[current - numOfTokens];
        }

        private Formula PopFormula()
        {
            Formula prev = formulas[formulas.Count - 1];
            formulas.Remove(prev);
            return prev;
        }
        private Formula PreviousFormula()
        {
            try
            {
                Formula prev = formulas[formulas.Count - 1];
                return prev;
            }
            catch
            {
                return null;
            }
            
            
        }

        private Formula FormulaAt(int i)
        {
            Formula prev = formulas[i];
            formulas.RemoveAt(i);
            return prev;
        }
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            //else
            return Peek().GetTokenType() == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            //else
            return Previous();
        }

        private bool IsOperator(TokenType type)
        {
            return type == TokenType.PLUS || type == TokenType.MINUS || type == TokenType.SLASH;
        }

        private Token FindRelevantVariable(Formula first, Formula second)
        {
            Token _first = new Token();
            Token _second = new Token();
            
            if (first is Formula.VariableRule firV)
            {
                _first = firV.var;
            }
            if (first is Formula.QuotientRule firQ) _first = firQ.variable;
            if (first is Formula.SumRule firS) _first = firS.variable;
            if (first is Formula.DifferenceRule firD) _first = firD.variable;
            if (first is Formula.ProductRule firP) _first = firP.variable;
            if (first is Formula.PowerRule firPR) _first = firPR.variable;
            if (second is Formula.VariableRule secV)
            {
                _second = secV.var;
            }
            if (second is Formula.QuotientRule secQ) _second = secQ.variable;
            if (second is Formula.SumRule secS) _second = secS.variable;
            if (second is Formula.DifferenceRule secD) _second = secD.variable;
            if (second is Formula.ProductRule secP) _second = secP.variable;
            if (second is Formula.PowerRule secPR) _second = secPR.variable;

            //this is so dumb I should prob just make variable a public amongst Formula 
            
            //else
            if (_second.lexeme is null && !(_first.lexeme is null)) return _first; // This is the effect of not having AI its a TON of edge cases
            if (_first.lexeme is null && !(_second.lexeme is null)) return _second;
            if (_first.lexeme is null && _second.lexeme is null) return new Token(TokenType.NULL, "0", 0, 0); // In the fallback its a pure constant i.e cos(4) is just another constant rule
            if (_second.lexeme.Equals(_first.lexeme)) return _first;
            throw new Exception("Feature Not Available Variables Must Match");
        }


    }
}
