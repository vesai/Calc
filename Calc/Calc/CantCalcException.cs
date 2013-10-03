using System;

namespace Calc.Calc
{
    public sealed class CantCalcException : Exception
    {
        public int? Position { get; private set; }

        public CantCalcException(int position, Exception innerException) 
            : base("Невозможно вычислить выражение", innerException)
        {
            Position = position;
        }
    }
}
