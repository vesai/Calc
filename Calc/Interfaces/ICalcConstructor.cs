using System;
using Calc.Exceptions.InfixCalc;

namespace Calc.Interfaces
{
    /// <summary> Интфейс для создания калькулятора </summary>
    public interface ICalcConstructor
    {
        /// <summary> Добавить бинарную операцию </summary>
        /// <param name="operation"> Операция </param>
        /// <param name="func"> Действие </param>
        /// <param name="isRightAssociation"> Является ли операция правоассоциированной </param>
        /// <returns> this для дальнейшей инициализации </returns>
        /// <exception cref="InitCalcException"> Неверные данные инициализации </exception>
        /// <exception cref="AggregateException"> Объект находится в инициализированном состоянии </exception>
        ICalcConstructor AddOperation(string operation, Func<double, double, double> func,
            bool isRightAssociation = false);

        /// <summary> Добавить бинарную операцию </summary>
        /// <param name="operation"> Операция </param>
        /// <param name="func"> Действие </param>
        /// <param name="afterValue"> Операция идет после значения или наоборот </param>
        /// <returns> this для дальнейшей инициализации </returns>
        /// <exception cref="InitCalcException"> Неверные данные инициализации </exception>
        /// <exception cref="AggregateException"> Объект находится в инициализированном состоянии </exception>
        ICalcConstructor AddOperation(string operation, Func<double, double> func, bool afterValue = false);

        /// <returns> Переход к инициализации операций, приоритет которых ниже </returns>
        /// <returns> this для дальнейшей инициализации </returns>
        /// <exception cref="AggregateException"> Объект находится в инициализированном состоянии </exception>
        ICalcConstructor GoToLowPriorityGroup();

        /// <summary> Создать функцию-калькулятор </summary>
        /// <returns> Функция (Может вызывать исключение CantCalcException) </returns>
        Func<string, double> Create();
    }
}