using Calc.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.InfixParser.Rules;

namespace Calc.InfixParser {
    /// <summary> Парсер инфиксного выражения </summary>
    /// <typeparam name="ResultType"> Тип результата </typeparam>
    public sealed class InfixParser<ResultType> {
        /// <summary> Парсер </summary>
        private Parser<ResultType, EInfixStates> parser;
        
        /// <summary> Создать парсер для инфиксного выражения </summary>
        /// <param name="settings"> Настройки парсера </param>
        public InfixParser(InfixParserSettings settings) {
            var parseItems = new List<IParseRule<ResultType, EInfixStates>>();
            parseItems.AddRange(settings.ConstParsing.Select(x => new ValueRule<ResultType>(x)));

            parseItems.Add(new SpaceRule<ResultType>(EInfixStates.Operation, settings.Spaces));
            parseItems.Add(new SpaceRule<ResultType>(EInfixStates.Value, settings.Spaces));

            // Unary
            var unGr = settings.UnaryOperations.GroupBy(x => char.IsLetter(x.Key[0]));
            var letGr = unGr.FirstOrDefault(gr => gr.Key);
            if (letGr != null)
                parseItems.Add(new WordsRule<ResultType>(letGr.ToDictionary(el => el.Key, el => el.Value), EInfixStates.Operation, EInfixStates.Operation));
            letGr = unGr.FirstOrDefault(gr => !gr.Key);
            if (letGr != null)
                parseItems.AddRange(letGr.Select(el => new OperatorRule<ResultType>(el.Key, EInfixStates.Operation, EInfixStates.Operation, el.Value)));

            // UnaryOperationsAfter
            unGr = settings.UnaryOperationsAfter.GroupBy(x => char.IsLetter(x.Key[0]));
            letGr = unGr.FirstOrDefault(gr => gr.Key);
            if (letGr != null)
                parseItems.Add(new WordsRule<ResultType>(letGr.ToDictionary(el => el.Key, el => el.Value), EInfixStates.Value, EInfixStates.Value));
            letGr = unGr.FirstOrDefault(gr => !gr.Key);
            if (letGr != null)
                parseItems.AddRange(letGr.Select(el => new OperatorRule<ResultType>(el.Key, EInfixStates.Value, EInfixStates.Value, el.Value)));
            
            // Binary
            unGr = settings.BinaryOperations.GroupBy(x => char.IsLetter(x.Key[0]));
            letGr = unGr.FirstOrDefault(gr => gr.Key);
            if (letGr != null)
                parseItems.Add(new WordsRule<ResultType>(letGr.ToDictionary(el => el.Key, el => el.Value), EInfixStates.Value, EInfixStates.Operation));
            letGr = unGr.FirstOrDefault(gr => !gr.Key);
            if (letGr != null)
                parseItems.AddRange(letGr.Select(el => new OperatorRule<ResultType>(el.Key, EInfixStates.Value, EInfixStates.Operation, el.Value)));

            parser = new Parser<ResultType, EInfixStates>(parseItems, EInfixStates.Operation);
        }

        public IEnumerable<ResultType> Parse(string str) {
            return parser.Parse(str);
        }

        /// <summary> Настройки для создания  </summary>
        public class InfixParserSettings {
            /// <summary> Набор функций для парсинга констант </summary>
            public IEnumerable<Func<CharEnum, Tuple<ResultType>>> ConstParsing { get; set; }
            /// <summary> Набор функций для парсинга унарных операций </summary>
            public IEnumerable<KeyValuePair<string, ResultType>> UnaryOperations { get; set; }
            /// <summary> Набор функций для парсинга унарных операций, стоящих после числа </summary>
            public IEnumerable<KeyValuePair<string, ResultType>> UnaryOperationsAfter { get; set; }
            /// <summary> Набор функций для парсинга бинарных операций </summary>
            public IEnumerable<KeyValuePair<string, ResultType>> BinaryOperations { get; set; }
            /// <summary> Во что парсить группу пробельных символов </summary>
            public ResultType Spaces { get; set; }
        }
    }
}