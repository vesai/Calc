using System;

namespace Calc.InfixParser.Exceptions
{
    public class CantParseException : Exception
    {
        public int Position { private set; get; }

        public CantParseException(string errorText, int position)
            : base(errorText)
        {
            Position = position;
        }
    }
}
