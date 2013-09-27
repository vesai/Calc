using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Parser;

namespace Calc.InfixParser.Rules {
    /// <summary> Правило для разбора оператора </summary>
    /// <typeparam name="ResultType"> Тип результата </typeparam>
    sealed class OperatorRule<ResultType> : IParseRule<ResultType, EInfixStates> {
        /// <summary> Строковое написание оператора </summary>
        private string caption;
        /// <summary> Результат </summary>
        private ResultType result;

        /// <summary> Создать правило для разбора оператора </summary>
        /// <param name="caption"> Строковое написание оператора </param>
        /// <param name="inState"> Входное состояние </param>
        /// <param name="outState"> Выходное состояние </param>
        /// <param name="result"> Результат </param>
        public OperatorRule(string caption, EInfixStates inState, EInfixStates outState, ResultType result) {
            this.caption = caption;
            this.result = result;
            InState = inState;
            OutState = outState;
        }

        #region IParseRule<ResultType, EInfixStates>
        public EInfixStates InState { get; private set; }

        public EInfixStates OutState { get; private set; }

        public Tuple<ResultType> Parse(CharEnum data) {
            var strEn = caption.GetEnumerator();
            strEn.MoveNext();
            while (strEn.Current == data.Current) {
                data.MoveNext();
                if (!strEn.MoveNext()) {
                    return Tuple.Create(result);
                }
                if (data.IsEnd)
                    break;
            }
            return null;
        }
        #endregion
    }
}
