using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_C___Scanner
{
    /// <summary>
    /// Enum defining all possible token types in C/C++
    /// </summary>
    public enum TokenType
    {
        // Preprocessor
        PREPROCESSOR_DIRECTIVE, INCLUDE,

        // Keywords
        IF, ELSE, WHILE, FOR, DO, SWITCH, CASE, DEFAULT, BREAK, CONTINUE,
        CLASS, PUBLIC, PRIVATE, PROTECTED, VOID, RETURN, USING, NAMESPACE,
        NEW, DELETE, THIS, CONST, STATIC, VIRTUAL,

        // Types
        INT, DOUBLE, FLOAT, CHAR, BOOL, STRING, AUTO,

        // Standard Library Identifiers
        COUT, CIN, ENDL, CERR, CLOG,

        // Operators
        PLUS, MINUS, MULTIPLY, DIVIDE, MODULO,
        ASSIGN, PLUS_ASSIGN, MINUS_ASSIGN, MULT_ASSIGN, DIV_ASSIGN,
        EQUAL, NOT_EQUAL, GREATER, LESS, GREATER_EQUAL, LESS_EQUAL,
        INCREMENT, DECREMENT,
        AND, OR, NOT, BITWISE_AND, BITWISE_OR, BITWISE_XOR, BITWISE_NOT,
        LEFT_SHIFT, RIGHT_SHIFT,

        // Member Access
        DOT, ARROW, SCOPE_RESOLUTION,

        // Stream Operators
        INSERTION, EXTRACTION,

        // Delimiters
        SEMICOLON, COMMA, COLON,
        LPAREN, RPAREN, LBRACE, RBRACE, LBRACKET, RBRACKET,
        QUOTES, SINGLE_QUOTE,

        // Others
        IDENTIFIER, INTEGER_LITERAL, FLOAT_LITERAL, STRING_LITERAL, CHAR_LITERAL,
        SINGLE_LINE_COMMENT, MULTI_LINE_COMMENT,

        // Special
        EOF, INVALID
    }
}

