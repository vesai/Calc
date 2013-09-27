using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Parser {
    /// <summary> Для перебора парсеров определенного состояния </summary>
    /// <typeparam name="ResultType"> Тип результата </typeparam>
    /// <typeparam name="StateType"> Тип состояния </typeparam>
    sealed class ParseDictionary<ResultType, StateType> {
        /// <summary> Словарь состояние - набор парсеров </summary>
        private IDictionary<StateType, IEnumerable<IParseRule<ResultType, StateType>>> dictionary;

        /// <summary> Создать объект для перебора парсеров определенного состояния </summary>
        /// <param name="rules"> Набор правил </param>
        public ParseDictionary(IEnumerable<IParseRule<ResultType, StateType>> rules) {
            dictionary = rules.GroupBy(x => x.InState)
                .Aggregate(
                    new Dictionary<StateType, IEnumerable<IParseRule<ResultType, StateType>>>(), 
                    (dic, val) => { 
                        dic.Add(val.Key, val); 
                        return dic; 
                    }
                );
        }

        /// <summary> Получить набор правил для определенного состояния </summary>
        /// <param name="state"> Состояние </param>
        /// <returns> Набор правил </returns>
        public IEnumerable<IParseRule<ResultType, StateType>> this[StateType state] {
            get {
                return dictionary[state];
            }
        } 
    }
}
