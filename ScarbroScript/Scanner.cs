using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    class Scanner
    {
        // The Token KeyWords for Identifiers
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "for", TokenType.FOR },
            { "fun", TokenType.FUN },
            { "if", TokenType.IF },
            { "nil", TokenType.NIL },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "true", TokenType.TRUE },
            { "var", TokenType.VAR },
            { "break", TokenType.BREAK },
            { "while", TokenType.WHILE }
        };


        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0; // start points to the first character
        private int current = 0; // current points at the char currently being considered
        private int line = 1; // tracks what source line current is on so we can produce tokens that have a location

        public Scanner(string source)
        {
            this.source = source;
 
        }

        /**
         * The scanner works its way through the source code, adding tokens until it runs out of characters. 
         * Then it appends one final “end of file” token.
         * That isn’t strictly needed, but it makes our parser a little cleaner.
         */
        public List<Token> ScanTokens()
        {   
            while (!IsAtEnd()) {
                // We are at the beginning of the next Lexeme
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
            
            
        }

        //Recognizing Lexemes
        private void ScanToken()
        {
            
            char c = Advance();
            Console.WriteLine($"Scanning character: {c}, Line: {line}, Current Index: {current}");
            switch (c)
            {
                //Single Char Lexemes
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case '[': AddToken(TokenType.LEFT_BRACKET); break;
                case ']': AddToken(TokenType.RIGHT_BRACKET); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-':
                    AddToken(Match('-') ? TokenType.DECREMENT : TokenType.MINUS);  
                    break;
                case '+': 
                    AddToken(Match('+') ? TokenType.INCREMENT : TokenType.PLUS);
                    break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*':
                    AddToken(Match('*') ? TokenType.EXPON : TokenType.STAR);
                    break;
                //Operators Bangs, Bang='s, etc.
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else if (Match('*'))
                    {
                        while (Peek() != '*' && !Match('/') && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                //Meaningless WhitesSpaces
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    line++;
                    break;
                //Literals
                case '"': String(); break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    } 
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    } 
                    else
                    {
                       ScarbroScript.Error(line, "Unexpected Character");
                    }
                    
                    break;

            }
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }



        /**
         * Looking past the decimal point requires a second character of lookahead
         * since we don’t want to consume the . until we’re sure there is a digit after it.
         */
        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        /**
         * Maximal munch means we can’t easily detect a reserved word until we’ve
         * reached the end of what might instead be an identifier. 
         * After all, a reserved word is an identifier, 
         * it’s just one that has been claimed by the language for its own use. 
         * That’s where the term reserved word comes from.
         * So we begin by assuming any lexeme starting with a letter or underscore is an
         */
        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            String text = source.Substring(start, current - start);
            // tryGetValue for error handling bc dicts cant have nulls like javas Hash maps
            if (keywords.TryGetValue(text, out TokenType type) == false) 
            {
                type = TokenType.IDENTIFIER;
            }
            AddToken(type);
        }


        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }


        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        /**
         * We consume as many digits as we find for the integer part of the literal. 
         * Then we look for a fractional part, which is a decimal point (.) followed by at least one digit. 
         * If we do have a fractional part, again, we consume as many digits as we can find.
         */
        private void Number()
        {
            while (IsDigit(Peek())) Advance();
            //Look for the fractional part
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Console.WriteLine("Number() - peek");
                //consume the decimal
                Advance();
                while (IsDigit(Peek())) Advance();
            }
            
            var result = source.Substring(start, current - start);
            Console.WriteLine(result);
            AddToken(TokenType.NUMBER, Double.Parse(result));
        }

        /**
         * Like the comments we basically "consume" every character until the end of the String
         */
        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++; //handles multiline strings 
                Advance(); 
            }

            if (IsAtEnd())
            {
                ScarbroScript.Error(line, "Unterminated String");
                return;
            }

            //The Closing "
            Advance();
            //just trims the qoutes for the Token, strings when outputted to console dont come with "
            Console.WriteLine("Current = " + current);
            Console.WriteLine("Substring = " + source.Substring(start + 1, current - start - 2));
            String value = source.Substring(start + 1, current - start - 2);
            AddToken(TokenType.STRING, value);
        }

        /**
         * Similar to Advance but doesnt "consume" a character like advance (and match kinda)
         * Its called LookAhead since it only looks at teh current unconsumed character
         * so basically if its at the end exit 0 other wise look at the current char (remeber like match its incremented post index)
         * 
         */
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        /**
         * Works very similar to Advance() "consume" the next char but only if its what we are looking for
         * using the curretn which was inremented in the advance it checks the next character and if its an equals it 
         * increments again so that current isnt the same char that  Match just checked remember that due to the post
         * operator the index of c is actually 0 since its a post increment!
         */
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;

            current++; //Increments for the advance so that it will skip checking the same char twice
            return true;
        }

        //Lets us know if we have consumed all characters
        private bool IsAtEnd()
        {
            return current >= source.Length;
        }

        //"Consumption" takes in a char the next in the source file 
        private char Advance()
        {
            return source[current++]; //Char at current pre increment and then increments after
        }

        // "Output" grabs the text for the current lexeme and creates a new Token for it 
        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }
        // handles output for literals @Overloaded function
        private void AddToken(TokenType type, Object literal)
        {
            String text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        

    }
}
