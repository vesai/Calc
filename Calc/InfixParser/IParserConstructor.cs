using System;
using System.Collections.Generic;

namespace Calc.InfixParser
{
    /// <summary> Конструктор для парсера инфиксной нотации </summary>
    /// <typeparam name="TRes"> Тип, который получается в результате преобразования </typeparam>
    public interface IParserConstructor<TRes>
    {
        /// <summary> Добавить унарную операцию </summary>
        /// <param name="operation"> Операция </param>
        /// <param name="operationRes"> Результат операции </param>
        /// <returns> Возвращает this для дальнейшей инициализации </returns>
        IParserConstructor<TRes> AddOperation(string operation, TRes operationRes);

        /// <summary> Добавить бинарную операцию </summary>
        /// <param name="operation"> Операция </param>
        /// <param name="operationRes"> Результат операции </param>
        /// <param name="afterValueOperation"> Операция пишется после значения </param>
        /// <returns> Возвращает this для дальнейшей инициализации </returns>
        IParserConstructor<TRes> AddUnaryOperation(string operation, TRes operationRes, bool afterValueOperation);
        /// <summary> Добавить действие при нахождении константы </summary>
        /// <param name="func"> Функция, вызываемая для значений </param>
        /// <returns> Возвращает this для дальнейшей инициализации </returns>
        IParserConstructor<TRes> SetConstAction(Func<double, TRes> func);
        /// <summary> Создать парсер </summary>
        /// <returns> Функция парсер </returns>
        Func<string, IEnumerable<TRes>> Create();
    }
}