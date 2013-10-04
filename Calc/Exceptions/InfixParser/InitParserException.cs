using System;

namespace Calc.Exceptions.InfixParser
{
    public sealed class InitParserException : Exception
    {
        public InitParserException(string message)
            : base(message)
        {
        }
    }
}