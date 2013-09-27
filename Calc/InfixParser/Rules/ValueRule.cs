using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Parser;

namespace Calc.InfixParser.Rules {
    /// <summary> Правило для разбора значений </summary>
    /// <typeparam name="ResultType"> Тип результата </typeparam>
    sealed class ValueRule<ResultType> : IParseRule<ResultType, EInfixStates> {
        /// <summary> Функция для получения результата </summary>
        Func<CharEnum, Tuple<ResultType>> parse;

        /// <summary> Создать объект для разбора значений  </summary>
        /// <param name="parseFunc"> Функция для получения результата </param>
        public ValueRule(Func<CharEnum, Tuple<ResultType>> parseFunc) {
            parse = parseFunc;
        }
        #region IParseRule<ResultType, EInfixStates>
        public EInfixStates InState { get { return EInfixStates.Operation; } }
        public EInfixStates OutState { get { return EInfixStates.Value; } }
        
        public Tuple<ResultType> Parse(CharEnum data) {
            return parse(data);
        }
        #endregion
    }
}
