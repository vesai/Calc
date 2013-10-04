using System;

namespace Calc.InfixParser
{
    internal sealed class StateException : Exception
    {
        public StateException(string message)
            : base(message)
        {
        }
    }
}