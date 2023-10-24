using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Stores the Data of a Token in a Class 
 */
namespace ScarbroScript
{
    public class Token
    {
        readonly TokenType type;
        readonly String lexeme;
        readonly Object literal;
        readonly int line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public String toString()
        {
            return type + " " + lexeme + " " + literal;
        }
    }
}
