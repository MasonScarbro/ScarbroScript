﻿using System;
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
        private int loopDepth = 0;

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
                statements.Add(Declaration());
            }

            return statements;
        }


        public Expr ParseToExpr()
        {
            try
            {
                return Expression();   
            } catch (ParseError e)
            {
                return null;
            }
        }

        private Stmt Statement()
        {
            //Console.WriteLine("Parsing Statement...");
            if (Match(TokenType.IMPORT)) return ImportStatement();
            if (Match(TokenType.FOR)) return ForStatement();
            if (Match(TokenType.FOREACH)) return ForEachStatement();
            if (Match(TokenType.IF)) return IfStatement();
            if (Match(TokenType.TRY)) return TryCatchStatement();
            if (Match(TokenType.SWITCH)) return SwitchStmt();
            if (Match(TokenType.CASE)) return CaseStmt();
            if (Match(TokenType.RETURN)) return ReturnStatement();
            if (Match(TokenType.BREAK)) return BreakStatement();
            // if token is a print its obviously of the statement type Print
            if (Match(TokenType.PRINT)) return PrintStatement();
            if (Match(TokenType.WHILE)) return WhileStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Stmt.Block(Block());
            //else
            return ExpressionStatement();
        }

        private Stmt ImportStatement()
        {
            var keyword = Previous();
            var fileName = ConsumeEither(TokenType.IDENTIFIER, TokenType.STRING, "Expected file name or path after Import statement");
            Consume(TokenType.SEMICOLON, "Expected ';' to close the Import statement");
            return new Stmt.Import(keyword, fileName.lexeme);

        }

        private Stmt BreakStatement()
        {
            // checks if its inside a loop
            if (IsInsideLoop())
            {
                Consume(TokenType.SEMICOLON, "Expected a semicolon after break statement");
                return new Stmt.Break();
            } else
            {
                throw Error(Previous(), "Break statement is only valid inside a loop.");
                
            }
        }

        private Stmt SwitchStmt()
        {
            Consume(TokenType.LEFT_PAREN, "Expected a '(' after and if");
            Expr comparable = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expected a ')' after and if condition starting brace '('");

            Stmt thenBranch = Statement();
            if (thenBranch is Stmt.Block errCheck)
            {
                foreach (Stmt stmt in errCheck.statements)
                {
                    if (!(stmt is Stmt.Case))
                    {
                        throw Error(Previous(), "Switches must only contain Case Stmts");
                    }
                }
            }

            return new Stmt.Switch(comparable, thenBranch);
        }

        private Stmt CaseStmt()
        {
            Expr condition = Expression();
            Consume(TokenType.COLON, "Expected ':' after condition of case. Ex: case condition: what to execute ");
            List<Stmt> thenBranch = new List<Stmt>();
            thenBranch.Add(Statement());
            return new Stmt.Case(condition, thenBranch);

        }

        private Stmt ForStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected a '(' after for");
            loopDepth++;
            Stmt initializer;
            if (Match(TokenType.SEMICOLON))
            {
                initializer = null;
            }
            else if (Match(TokenType.VAR))
            {
                initializer = VarDeclaration();
            }
            else
            {
                initializer = ExpressionStatement();
            }

            Expr condition = null;
            if (!Check(TokenType.SEMICOLON))
            {
                condition = Expression();
            }
            Consume(TokenType.SEMICOLON, "Expected a ';' after loop condition");

            Expr increment = null;
            if (!Check(TokenType.RIGHT_PAREN))
            {
                increment = Expression();
            }
            Consume(TokenType.RIGHT_PAREN, "Expected a ')' after clause");
            Stmt body = Statement();

            /**
             * The increment, if there is one, 
             * executes after the body in each iteration of the loop.
             * We do that by replacing the body with a little block 
             * that contains the original body followed by an expression
             * statement that evaluates the increment.
             */
            if (increment != null)
            {
                List<Stmt> stmts = new List<Stmt>();
                stmts.Add(body);
                stmts.Add(new Stmt.Expression(increment));

                body = new Stmt.Block(stmts);
            }

            if (condition == null) condition = new Expr.Literal(true);
            //else
            body = new Stmt.While(condition, body);

            if (initializer != null)
            {
                List<Stmt> stmts = new List<Stmt>();
                stmts.Add(initializer);
                stmts.Add(body);
                body = new Stmt.Block(stmts);
            }
            return body;
        }


        private Stmt ForEachStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected a '(' after for");
            loopDepth++;
            Stmt initializer = null;
            if (Match(TokenType.VAR))
            {
                initializer = VarDeclaration();
            }
            Consume(TokenType.COLON, "Needed the ':' for the in operation of a foreach stmt");
            Expr arr = Primary();
            Consume(TokenType.RIGHT_PAREN, "Expected a ')' after for");
            Stmt body = Statement();

            return new Stmt.ForEach(initializer, arr, body);
            


        }


        private Stmt TryCatchStatement()
        {
            Stmt tryBlock = Statement();
            Consume(TokenType.CATCH, "You Must Provide a 'catch' after each try statement");
            Consume(TokenType.LEFT_PAREN, "Instance must be wrapped in parentheses you are missing '('");
            Token exc = Consume(TokenType.IDENTIFIER, "Please Supply an Instance for the Exception, Expectes an Identifier");
            Consume(TokenType.RIGHT_PAREN, "Instance must be wrapped in parentheses you are missing ')'");
            Stmt catchBlock = Statement();
            
            return new Stmt.TryCatch(tryBlock, exc, catchBlock);
        }

        private Stmt IfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected a '(' after and if");
            Expr condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expected a ')' after and if condition starting brace '('");

            Stmt thenBranch = Statement();
            Stmt elseBranch = null;
            if (Match(TokenType.ELSE))
            {
                elseBranch = Statement();
            }

            return new Stmt.If(condition, thenBranch, elseBranch);
        }


        private Stmt ReturnStatement()
        {
            Token keyword = Previous();

            Expr value = null;
            if (!(Check(TokenType.SEMICOLON)))
            {
                value = Expression();
            }
            Consume(TokenType.SEMICOLON, "Expected a semicolon after value");
            return new Stmt.Return(keyword, value);
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

        private Stmt WhileStatement()
        {
            try
            {
                //Console.WriteLine("Entering While Loop... (Parse)");

                Consume(TokenType.LEFT_PAREN, "Expected a '(' after and if");
                Expr condition = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expected a ')' after and if condition starting brace '('");

                //Increments loopDepth to indicate if we are in a loop
                loopDepth++;
                //Console.WriteLine("Loop Depth: " + loopDepth);


                Stmt body = Statement();

                return new Stmt.While(condition, body);
            }
            finally
            {
                //Console.WriteLine("Exiting While Loop... (Parse)");
                loopDepth--;
                //Console.WriteLine("Loop Depth: " + loopDepth);
            }

        }


        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON, "Expected a semicolon after value");
            return new Stmt.Expression(expr);
        }

        private List<Stmt> Block()
        {
            //Console.WriteLine("Parsing Block...");
            List<Stmt> statements = new List<Stmt>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }
            //Console.WriteLine(tokens.ToString());
            Consume(TokenType.RIGHT_BRACE, "Expected a '}' after block");

            return statements;
        }

        private Expr Expression()
        {
            return Assignment(); // "kickstarts" the descent
        }

        private Expr Assignment()
        {
            Expr expr = Or();

            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr.GetType() == typeof(Expr.Variable))
                {
                    Token name = ((Expr.Variable)expr).name;
                    List<Expr> index = ((Expr.Variable)expr).index;
                    if (index == null || index.Count == 0)
                    {
                        return new Expr.Assign(name, value, null);
                    }
                    return new Expr.Assign(name, value, index);
                } 
                else if (expr is Expr.Access) // Since the dot call are not technically variables they are Get Exprs (which are variables at the class level)
                {
                    Expr.Access get = (Expr.Access)expr;
                    return new Expr.Set(get.obj, get.name, value);
                }
                
                Error(equals, "Invalid Assignment Target");
            }

            return expr;

        }

        private Expr Or()
        {
            Expr expr = And();

            while (Match(TokenType.OR))
            {
                Token oper = Previous();
                Expr right = And();
                expr = new Expr.Logical(expr, oper, right);
            }

            return expr;
        }

        private Expr And()
        {
            Expr expr = Exists();

            while (Match(TokenType.AND))
            {
                Token oper = Previous();
                Expr right = Equality();
                expr = new Expr.Logical(expr, oper, right);
            }
            return expr;
        }
        private Expr Exists()
        {
            Expr expr = Is();

            while (Match(TokenType.EXISTS))
            {
                Token oper = Previous();
                Expr right = null;
                expr = new Expr.Logical(expr, oper, right);
            }
            return expr;
        }
        private Expr Is()
        {
            Expr expr = Equality();

            while (Match(TokenType.IS))
            {
                Token oper = Previous();
                Expr right = Equality();
                expr = new Expr.Logical(expr, oper, right);
            }
            return expr;
        }

        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.CLASS)) return ClassDeclaration();
                if (Match(TokenType.FUN)) return Function("function");
                if (Match(TokenType.VAR)) return VarDeclaration();
                

                return Statement();
            } catch (ParseError)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt ClassDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expected Class Name");
            Consume(TokenType.LEFT_BRACE, "Expected '{' before class body (After class Identifier)");

            List<Stmt.Function> methods = new List<Stmt.Function>();
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                methods.Add(Function("method"));
            }

            Consume(TokenType.RIGHT_BRACE, "Expected '}' after class body");

            return new Stmt.Class(name, methods);
        }

        private Stmt.Function Function(String kind)
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expected " + kind + "name");
            Consume(TokenType.LEFT_PAREN, "Expected '(' after a" + kind + " declaration");
            List<Token> parameters = new List<Token>();

            //Handles Zero params
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    parameters.Add(Consume(TokenType.IDENTIFIER, "Parameter name needed in between the parantheses of your function"));

                } while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, "Expected a a')' after the parameters");
            Consume(TokenType.LEFT_BRACE, "Expected a '{' Before a " + kind + " body");
            List<Stmt> body = Block();
            return new Stmt.Function(name, parameters, body);
        }
        private Stmt VarDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expected Variable name ");

            Expr intializer = null;
            
            if (Match(TokenType.EQUAL))
            {
                intializer = Expression();
            }
            Console.WriteLine("Initializer was: " + intializer);

            ConsumeEither(TokenType.SEMICOLON, TokenType.FOREACH , "Expected ';' after your variable declaration");
            return new Stmt.Var(name, intializer);
        }

        private Expr Equality()
        {
            Expr expr = Ternary();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token oper = Previous();
                Expr right = Comparable();
                expr = new Expr.Binary(expr, oper, right);
            }


            return expr;
        }

        private Expr Ternary()
        {
            Expr expr = Comparable();

            while (Match(TokenType.TERNARY))
            {
                Expr left = Term();
                Consume(TokenType.COLON, "Expected ':' after initial term? Ternarys need an ''else''");
                Expr right = Term();
                expr = new Expr.Ternary(expr, left, right);
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

            while (Match(TokenType.STAR, TokenType.SLASH, TokenType.EXPON))
            {
                Token oper = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, oper, right);
            }


            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS, TokenType.INCREMENT, TokenType.DECREMENT))
            {
                Token oper = Previous();
                Expr right = Unary();
                return new Expr.Unary(oper, right);
            }

            return Call();
        }

        private Expr Call()
        {
            Expr expr = Primary();

            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                {
                    expr = ParseArgumentsAndFinishCall(expr);
                }
                else if (Match(TokenType.DOT))
                {
                    Token name = Consume(TokenType.IDENTIFIER, "Expected property name after '.'");
                    expr = new Expr.Access(expr, name);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expr ParseArgumentsAndFinishCall(Expr callee)
        {
            List<Expr> arguments = new List<Expr>();
            // checks for no arguments!
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(Expression());

                } while (Match(TokenType.COMMA));

            }
            Token paren = Consume(TokenType.RIGHT_PAREN, "Expected ')' After arguments");

            return new Expr.Call(callee, paren, arguments);
        }

        private Expr Array()
        {
            List<Expr> elements = new List<Expr>();
            // checks for no arguments!
            if (!Check(TokenType.RIGHT_BRACKET))
            {
                do
                {
                    elements.Add(Expression());

                } while (Match(TokenType.COMMA));

            }
            Token bracket = Consume(TokenType.RIGHT_BRACKET, "Expected ']' After arguments");

            return new Expr.Array(bracket, elements);
        }

        private Expr Dict()
        {
            Expr key;
            Expr value;
            List<Expr> elements = new List<Expr>();
            // checks for no arguments!
            if (!Check(TokenType.RIGHT_BRACKET))
            {
                do
                {
                    key = (Expression());
                    Consume(TokenType.COLON, "Needs a Colon After a key");
                    value = Expression();
                    elements.Add(new Expr.KeyValue(key, value));
                } while (Match(TokenType.COMMA));

            }
            Consume(TokenType.RIGHT_BRACE, "Expected '}' After arguments");

            return new Expr.Dict(elements);
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

            if (Match(TokenType.THIS)) return new Expr.This(Previous());

            if (Match(TokenType.IDENTIFIER))
            {
                List<Expr> index = new List<Expr>();
                bool isArray = false;
                Token variable = Previous();
                if (Match(TokenType.LEFT_BRACKET))
                {
                    index.Add(Or());
                    isArray = true;
                    Consume(TokenType.RIGHT_BRACKET, "Expected ']'");
                    Console.WriteLine(index);

                }
                //Console.WriteLine(variable.lexeme);
                return new Expr.Variable(variable, isArray, index);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
                return new Expr.Grouping(expr);

            }

            if (Match(TokenType.LEFT_BRACKET))
            {
                return Array();
            }

            if (Match(TokenType.LEFT_BRACE))
            {
                return Dict();
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

        /// <summary>
        /// This is just Consume but for specific edge cases where syntax would be nicer 
        /// with syntax like foreach
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Token ConsumeEither(TokenType type1, TokenType type2, string message)
        {
            //foreach syntax
            if ((LookBack(4).type == type2))
            {
                current--; 
                return Advance();
            }
            
            if (Check(type1) || Check(type2)) return Advance();

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

      
        private Token LookBack(int numOfTokensBack)
        {
            return tokens[current - numOfTokensBack];
        }

        private bool IsInsideLoop()
        {
            if (loopDepth <= 0)
            {
                Error(Previous(), "Break Statement not in loop");
            }
            //else
            return true;
            
            
        }
    }
}
