using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeParser
{
    public enum TokenType
    {

        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        DOT, MINUS, PLUS, SLASH, STAR, RIGHT_BRACKET, LEFT_BRACKET,

        // Operaters
        EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL, EXPON,

        // Literals.
        VARIABLE, NUMBER,

        // Keywords.
        DX, SIN, COS, TAN, NULL,

        EOF

    }
}
