using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Parser;

namespace Calc.InfixParser.Rules {
    /// <summary> Правило для разбора оператора </summary>
    /// <typeparam name="ResultType"> Тип результата </typeparam>
    sealed class SpaceRule<ResultType> : IParseRule<ResultType, EInfixStates> {
        /// <summary> Состояние </summary>
        private EInfixStates inOutState;
        /// <summary> Результат </summary>
        private ResultType result;

        /// <summary> Создать правило для разбора последовательности пробелов </summary>
        /// <param name="inOutState"> Входное и выходное состояния </param>
        /// <param name="result"> Результат </param>
        public SpaceRule(EInfixStates inOutState, ResultType result) {
            this.inOutState = inOutState;
            this.result = result;
        }

        #region IParseRule<ResultType, EInfixStates>
        public EInfixStates InState {
            get { return inOutState; }
        }

        public EInfixStates OutState {
            get { return inOutState; }
        }

        public Tuple<ResultType> Parse(CharEnum data) {
            if (!char.IsWhiteSpace(data.Current)) {
                return null;
            }
            while (data.MoveNext() && char.IsWhiteSpace(data.Current)) ;
            return Tuple.Create(result);
        }
        #endregion
    }
   
}
