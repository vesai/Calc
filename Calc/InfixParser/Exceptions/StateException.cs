using System;

namespace Calc.InfixParser.Exceptions
{
    public sealed class StateException : Exception
    {
        public StateException(string message)
            :base(message)
        {
        }
    }
}
