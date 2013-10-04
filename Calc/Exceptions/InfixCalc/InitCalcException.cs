using System;

namespace Calc.Exceptions.InfixCalc
{
    /// <summary> Неверные данные инициализации  </summary>
    public sealed class InitCalcException : Exception
    {
        public InitCalcException(Exception innerException)
            : base("Ошибка инициализации калькулятора (Причина: " + innerException.Message + ")", innerException)
        {
        }
    }
}