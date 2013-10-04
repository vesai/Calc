using System;

namespace Calc.Exceptions.InfixToRpn
{
    public class CantConvertException : Exception
    {
        public CantConvertException(string message)
            : base(message)
        {
        }
    }
}