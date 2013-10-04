using System;

namespace Calc.Exceptions.InfixToRpn
{
    public sealed class InitConverterException : Exception
    {
        public InitConverterException(Exception innerException)
            : base("Ошибка инициализации конвертера (Причина: " + innerException.Message + ")", innerException)
        {
        }
    }
}