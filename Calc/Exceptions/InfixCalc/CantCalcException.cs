using System;
using System.Text;

namespace Calc.Exceptions.InfixCalc
{
    /// <summary> Невозможно вычислить выражение </summary>
    public sealed class CantCalcException : Exception
    {
        public CantCalcException(int position, Exception innerException)
            : base(GetErrorMessage(innerException.Message, position), innerException)
        {
            Position = position;
        }

        public CantCalcException(Exception innerException)
            : base(GetErrorMessage(innerException.Message), innerException)
        {
        }

        /// <summary> Позиция, предположительно в которой находится ошибка </summary>
        public int? Position { get; private set; }

        private static string GetErrorMessage(string innerExceptionMessage, int? position = null)
        {
            var sb = new StringBuilder("Невозможно вычислить выражение (");
            if (position.HasValue)
                sb.AppendFormat("Символ: {0}; ", position);
            sb.AppendFormat("Причина: \"{0}\")", innerExceptionMessage);
            return sb.ToString();
        }
    }
}