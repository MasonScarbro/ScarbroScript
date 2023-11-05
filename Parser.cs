using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class Parser
    {
        private class ParseError : FormatException { }

        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }
        //Kickstarts the parsing, parses as many statements until EOI/EOF
        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                statements.Add(Statement());
            }

            return statements;
        }

        private Stmt Statement()
        {
            // if token is a print its obviously of the statement type Print
            if (Match(TokenType.PRINT)) return PrintStatement();
            //else
            return ExpressionStatement();
        }

        /// <summary>
        /// Just like the rest of the descent parse we "start descent"
        /// to figure out the value of whats after the print 
        /// </summary>
        /// <returns> a New Stmt syntax node of Type print</returns>
        private Stmt PrintStatement()
        {
            Expr value = Expression(); 
            Consume(TokenType.SEMICOLON, "Expected a semicolon after value");
            return new Stmt.Print(value);
        }

        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON, "Expected a semicolon after value");
            return new Stmt.Expression(expr);
        }

        private Expr Expression()
        {
            return Equality(); // "kickstarts the descent"
        }

        private Expr Equality()
        {
            Expr expr = Comparable();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token oper = Previous();
                Expr right = Comparable();
                expr = new Expr.Binary(expr, oper, right);
            }


            return expr;
        }

        private Expr Comparable()
        {
            Expr expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token oper = Previous();
                Expr right = Term();
                expr = new Expr.Binary(expr, oper, right);
            }


            return expr;
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token oper = Previous();
                Expr right = Factor();
                expr = new Expr.Binary(expr, oper, right);
            }


            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(TokenType.STAR, TokenType.SLASH))
            {
                Token oper = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, oper, right);
            }


            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token oper = Previous();
                Expr right = Unary();
                return new Expr.Unary(oper, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new Expr.Literal(false);
            if (Match(TokenType.TRUE)) return new Expr.Literal(true);
            if (Match(TokenType.NIL)) return new Expr.Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Expr.Literal(Previous().literal);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
                return new Expr.Grouping(expr);

            }
            throw Error(Peek(), "Expect Expression");
            
        }

        /**
         * This checks to see if the current token has any of the given types.
         * If so, it consumes the token and returns true.
         * Otherwise, it returns false and leaves the current token alone.
         * 
         */
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

            //else
            return false;
        }

        /// <summary>
        /// Basically the check function but also has error handeling
        /// checks the next token and advances unless its at EOF (part of the Advance func)
        /// otherwise it throws an error for the user with the given param message
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns> Advance() which returns Token </returns>        
        private Token Consume(TokenType type, String message)
        {
            if (Check(type)) return Advance();

            throw Error(Peek(), message);

        }

        private ParseError Error(Token token, String message)
        {
            Console.WriteLine($"Parser Error at line {token.line}: {message}");
            ScarbroScript.Error(token, message);
            return new ParseError();
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().type == TokenType.SEMICOLON) return;

                switch (Peek().type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }
                Advance();
            }
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
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().type == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens[current];
            
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }
    }
}
