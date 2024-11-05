using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace C_C___Scanner
{
    /// <summary>
    /// Lexical analyzer for C/C++ source code
    /// </summary>
    public class Scanner
    {
        private readonly string _sourceCode;
        private int _position;
        private int _line;
        private int _column;

        // Regular expressions for different patterns
        private static readonly Dictionary<string, TokenType> Keywords = new()
        {
            {"if", TokenType.IF},
            {"else", TokenType.ELSE},
            {"while", TokenType.WHILE},
            {"for", TokenType.FOR},
            {"do", TokenType.DO},
            {"switch", TokenType.SWITCH},
            {"case", TokenType.CASE},
            {"default", TokenType.DEFAULT},
            {"break", TokenType.BREAK},
            {"continue", TokenType.CONTINUE},
            {"return", TokenType.RETURN},
            {"class", TokenType.CLASS},
            {"public", TokenType.PUBLIC},
            {"private", TokenType.PRIVATE},
            {"protected", TokenType.PROTECTED},
            {"void", TokenType.VOID},
            {"using", TokenType.USING},
            {"namespace", TokenType.NAMESPACE},
            {"new", TokenType.NEW},
            {"delete", TokenType.DELETE},
            {"this", TokenType.THIS},
            {"const", TokenType.CONST},
            {"static", TokenType.STATIC},
            {"virtual", TokenType.VIRTUAL},
            {"int", TokenType.INT},
            {"double", TokenType.DOUBLE},
            {"float", TokenType.FLOAT},
            {"char", TokenType.CHAR},
            {"bool", TokenType.BOOL},
            {"string", TokenType.STRING},
            {"auto", TokenType.AUTO}
        };

        private static readonly Dictionary<string, TokenType> StdLibIdentifiers = new()
        {
            {"cout", TokenType.COUT},
            {"cin", TokenType.CIN},
            {"endl", TokenType.ENDL},
            {"cerr", TokenType.CERR},
            {"clog", TokenType.CLOG}
        };

        private static readonly Dictionary<string, TokenType> Operators = new()
        {
            {"+", TokenType.PLUS},
            {"-", TokenType.MINUS},
            {"*", TokenType.MULTIPLY},
            {"/", TokenType.DIVIDE},
            {"%", TokenType.MODULO},
            {"=", TokenType.ASSIGN},
            {"+=", TokenType.PLUS_ASSIGN},
            {"-=", TokenType.MINUS_ASSIGN},
            {"*=", TokenType.MULT_ASSIGN},
            {"/=", TokenType.DIV_ASSIGN},
            {"==", TokenType.EQUAL},
            {"!=", TokenType.NOT_EQUAL},
            {">", TokenType.GREATER},
            {"<", TokenType.LESS},
            {">=", TokenType.GREATER_EQUAL},
            {"<=", TokenType.LESS_EQUAL},
            {"++", TokenType.INCREMENT},
            {"--", TokenType.DECREMENT},
            {"&&", TokenType.AND},
            {"||", TokenType.OR},
            {"!", TokenType.NOT},
            {"&", TokenType.BITWISE_AND},
            {"|", TokenType.BITWISE_OR},
            {"^", TokenType.BITWISE_XOR},
            {"~", TokenType.BITWISE_NOT},
            {"<<", TokenType.INSERTION},
            {">>", TokenType.EXTRACTION},
            {".", TokenType.DOT},
            {"->", TokenType.ARROW},
            {"::", TokenType.SCOPE_RESOLUTION}
        };

        // Regular expression patterns
        private static readonly string PreprocessorPattern = @"^#\w+";
        private static readonly string IncludePattern = @"^#include\s*<[^>]+>";
        private static readonly string IdentifierPattern = @"^[a-zA-Z_]\w*";
        private static readonly string NumberPattern = @"^(\d+(\.\d*)?|\.\d+)([eE][+-]?\d+)?";
        private static readonly string StringPattern = @"^""([^""\\]|\\.)*""";
        private static readonly string CharPattern = @"^'([^'\\]|\\.)'";
        private static readonly string SingleLineCommentPattern = @"^//.*";
        private static readonly string MultiLineCommentPattern = @"^/\*[\s\S]*?\*/";
        private static readonly string WhitespacePattern = @"^\s+";

        private bool _inStdNamespace = false;

        public Scanner(string sourceCode)
        {
            _sourceCode = sourceCode;
            _position = 0;
            _line = 1;
            _column = 1;
        }

        public Token GetNextToken()
        {
            if (_position >= _sourceCode.Length)
            {
                return new Token(TokenType.EOF, "", _line, _column);
            }

            string remainingText = _sourceCode[_position..];

            // Skip whitespace
            var whitespaceMatch = Regex.Match(remainingText, WhitespacePattern);
            if (whitespaceMatch.Success)
            {
                UpdatePosition(whitespaceMatch.Value);
                remainingText = _sourceCode[_position..];
            }

            if (_position >= _sourceCode.Length)
            {
                return new Token(TokenType.EOF, "", _line, _column);
            }

            // Check for "using namespace std;"
            if (remainingText.TrimStart().StartsWith("using namespace std;"))
            {
                _inStdNamespace = true;
            }

            // Preprocessor directives
            var includeMatch = Regex.Match(remainingText, IncludePattern);
            if (includeMatch.Success)
            {
                var token = new Token(TokenType.INCLUDE, includeMatch.Value, _line, _column);
                UpdatePosition(includeMatch.Value);
                return token;
            }

            var preprocessorMatch = Regex.Match(remainingText, PreprocessorPattern);
            if (preprocessorMatch.Success)
            {
                var token = new Token(TokenType.PREPROCESSOR_DIRECTIVE, preprocessorMatch.Value, _line, _column);
                UpdatePosition(preprocessorMatch.Value);
                return token;
            }

            // Comments
            var commentMatch = Regex.Match(remainingText, SingleLineCommentPattern);
            if (commentMatch.Success)
            {
                var token = new Token(TokenType.SINGLE_LINE_COMMENT, commentMatch.Value, _line, _column);
                UpdatePosition(commentMatch.Value);
                return token;
            }

            commentMatch = Regex.Match(remainingText, MultiLineCommentPattern);
            if (commentMatch.Success)
            {
                var token = new Token(TokenType.MULTI_LINE_COMMENT, commentMatch.Value, _line, _column);
                UpdatePosition(commentMatch.Value);
                return token;
            }

            // Strings
            var stringMatch = Regex.Match(remainingText, StringPattern);
            if (stringMatch.Success)
            {
                var token = new Token(TokenType.STRING_LITERAL, stringMatch.Value, _line, _column);
                UpdatePosition(stringMatch.Value);
                return token;
            }

            // Characters
            var charMatch = Regex.Match(remainingText, CharPattern);
            if (charMatch.Success)
            {
                var token = new Token(TokenType.CHAR_LITERAL, charMatch.Value, _line, _column);
                UpdatePosition(charMatch.Value);
                return token;
            }

            // Numbers
            var numberMatch = Regex.Match(remainingText, NumberPattern);
            if (numberMatch.Success)
            {
                string value = numberMatch.Value;
                var tokenType = value.Contains(".") || value.Contains("e") || value.Contains("E")
                    ? TokenType.FLOAT_LITERAL
                    : TokenType.INTEGER_LITERAL;
                var token = new Token(tokenType, value, _line, _column);
                UpdatePosition(value);
                return token;
            }

            // Multi-character operators
            var sortedOperators = Operators.Keys.OrderByDescending(x => x.Length);
            foreach (var op in sortedOperators)
            {
                if (remainingText.StartsWith(op))
                {
                    var token = new Token(Operators[op], op, _line, _column);
                    UpdatePosition(op);
                    return token;
                }
            }

            // Identifiers, Keywords, and Standard Library Identifiers
            var identifierMatch = Regex.Match(remainingText, IdentifierPattern);
            if (identifierMatch.Success)
            {
                string value = identifierMatch.Value;
                TokenType tokenType;

                // Check if it's a keyword
                if (Keywords.ContainsKey(value))
                {
                    tokenType = Keywords[value];
                }
                // Check if it's a standard library identifier
                else if (_inStdNamespace && StdLibIdentifiers.ContainsKey(value))
                {
                    tokenType = StdLibIdentifiers[value];
                }
                // Otherwise it's a regular identifier
                else
                {
                    tokenType = TokenType.IDENTIFIER;
                }

                var token = new Token(tokenType, value, _line, _column);
                UpdatePosition(value);
                return token;
            }

            // Single character tokens
            var currentChar = remainingText[0];
            TokenType? type = currentChar switch
            {
                ';' => TokenType.SEMICOLON,
                ',' => TokenType.COMMA,
                ':' => TokenType.COLON,
                '(' => TokenType.LPAREN,
                ')' => TokenType.RPAREN,
                '{' => TokenType.LBRACE,
                '}' => TokenType.RBRACE,
                '[' => TokenType.LBRACKET,
                ']' => TokenType.RBRACKET,
                '"' => TokenType.QUOTES,
                '\'' => TokenType.SINGLE_QUOTE,
                _ => null
            };

            if (type.HasValue)
            {
                var token = new Token(type.Value, currentChar.ToString(), _line, _column);
                UpdatePosition(currentChar.ToString());
                return token;
            }

            // Invalid token
            var invalidToken = new Token(TokenType.INVALID, currentChar.ToString(), _line, _column);
            UpdatePosition(currentChar.ToString());
            return invalidToken;
        }

        private void UpdatePosition(string matchedText)
        {
            foreach (char c in matchedText)
            {
                if (c == '\n')
                {
                    _line++;
                    _column = 1;
                }
                else
                {
                    _column++;
                }
            }
            _position += matchedText.Length;
        }
    }
}
