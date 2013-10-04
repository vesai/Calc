using System;

namespace Calc.Exceptions.InfixParser
{
    public sealed class CantParseException : Exception
    {
        public CantParseException(string errorText, int position)
            : base(errorText)
        {
            Position = position;
        }

        public int Position { private set; get; }
    }
}