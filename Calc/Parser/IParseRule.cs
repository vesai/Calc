using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Parser {
    /// <summary> Правило для парсинга </summary>
    /// <typeparam name="ResultType"> Тип, результат парсинга </typeparam>
    /// <typeparam name="StateType"> Тип состояния </typeparam>
    public interface IParseRule<ResultType, StateType> {
        /// <summary> Входное состояние </summary>
        StateType InState { get; }
        /// <summary> Выходное состояние </summary>
        StateType OutState { get; }
        /// <summary> Операция по разбору </summary>
        /// <param name="data"> Данные </param>
        /// <returns> null - если разбор неуспешен или Tupe из результата </returns>
        Tuple<ResultType> Parse(CharEnum data);
    }
}