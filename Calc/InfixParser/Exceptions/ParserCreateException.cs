using System;

namespace Calc.InfixParser.Exceptions
{
    public sealed class ParserCreateException : Exception
    {
        public ParserCreateException(string message)
            : base(message)
        {
        }
    }
}
