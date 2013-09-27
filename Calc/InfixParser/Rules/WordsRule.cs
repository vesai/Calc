using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Parser;

namespace Calc.InfixParser.Rules {
    /// <summary> Правило для разбора идентификаторов(слов) </summary>
    /// <typeparam name="ResultType"> Результат </typeparam>
    sealed class WordsRule<ResultType> : IParseRule<ResultType, EInfixStates> {
        /// <summary> Словарь слово - результат </summary>
        private IDictionary<string, ResultType> words;
        /// <summary> Создать правило для разбора слов </summary>
        /// <param name="words"> Словарь слово - результат </param>
        /// <param name="inState"> Входное состояние </param>
        /// <param name="outState"> Выходное состояние </param>
        public WordsRule(IDictionary<string, ResultType> words, EInfixStates inState, EInfixStates outState) {
            this.words = words;
            InState = inState;
            OutState = outState;
        }
        #region IParseRule<ResultType, EInfixStates>
        public EInfixStates InState { get; private set; }
        public EInfixStates OutState { get; private set; }
        public Tuple<ResultType> Parse(CharEnum data) {
            StringBuilder sb = new StringBuilder();
            if (!char.IsLetter(data.Current))
                return null;
            sb.Append(data.Current);
            while (data.MoveNext() && char.IsLetterOrDigit(data.Current)) {
                sb.Append(data.Current);
            }
            try {
                return Tuple.Create(words[sb.ToString()]);
            } catch {
                throw new CantFindFunctionException();
            }
        }
        #endregion

        /// <summary> Не удалось найти функцию </summary>
        public class CantFindFunctionException : Exception {
            public CantFindFunctionException() : base("Не удалось найти функцию") { }
        }
    }
}
