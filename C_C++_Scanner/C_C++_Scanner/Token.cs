using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_C___Scanner
{
    /// <summary>
    /// Represents a token identified by the scanner
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The type of the token
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// The actual text value of the token
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The line number where the token appears
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// The column number where the token appears
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Creates a new Token instance
        /// </summary>
        public Token(TokenType type, string value, int line, int column)
        {
            Type = type;
            Value = value;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"Token:\n" +
                   $"    Type    : {Type}\n" +
                   $"    Value   : '{Value}'\n" +
                   $"    Line    : {Line}\n" +
                   $"    Column  : {Column}";
        }
    }
}
