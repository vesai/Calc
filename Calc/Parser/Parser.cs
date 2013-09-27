using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Parser {
    /// <summary> Класс для разбора строки </summary>
    /// <typeparam name="ResultType"> Тип, в который делается разбор </typeparam>
    /// <typeparam name="StateType"> Тип, используемый для состояния </typeparam>
    public class Parser<ResultType, StateType> {
        /// <summary> Объект для перебора парсеров определенного состояния </summary>
        private ParseDictionary<ResultType, StateType> parseDictionary;
        /// <summary> Начальное состояние при разборе </summary>
        private StateType initialState;

        /// <summary> Создать объект для разбора строки на объекты </summary>
        /// <param name="rules"> Набор правил </param>
        /// <param name="initialState"> Начальное состояние </param>
        public Parser(IEnumerable<IParseRule<ResultType, StateType>> rules, StateType initialState) {
            parseDictionary = new ParseDictionary<ResultType, StateType>(rules);
            this.initialState = initialState;
        }

        /// <summary> Выполнить парсинг очередного элемента </summary>
        /// <param name="enumerator"> Объект для работы со входной строкой </param>
        /// <param name="state"> Текущее состояние </param>
        /// <returns> Результата разбора </returns>
        private ResultType ParseOne(ref CharEnum enumerator, ref StateType state) {
            List<Exception> exception = new List<Exception>();
            foreach (var x in parseDictionary[state]) {
                try {
                    var en = enumerator.Clone();
                    var pRes = x.Parse(en);
                    if (pRes == null)
                        continue;
                    enumerator = en;
                    state = x.OutState;
                    return pRes.Item1;
                } catch (Exception e) {
                    exception.Add(e);
                }
            }
            if (exception.Count != 0) {
                throw new CantParseException(enumerator, new AggregateException(exception));
            }
            throw new CantParseException(enumerator);
        }

        /// <summary> Сделать разбор строки </summary>
        /// <param name="str"> Разбираемая строка </param>
        /// <returns> Объекты, созданные при разборе </returns>
        /// <exception cref="CantParseException"> Невозможно разобрать строку </exception>
        public IEnumerable<ResultType> Parse(string str) {
            var ce = new CharEnum(str);
            var nowState = initialState;
            ce.MoveNext();
            while (!ce.IsEnd) {
                yield return ParseOne(ref ce, ref nowState);
            }
            yield break;
        }
    }
}