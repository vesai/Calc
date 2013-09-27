using Calc.InfixParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Parser;
using Calc.InfixCalc.Operations;

namespace Calc.InfixCalc {
    /// <summary> Объект для вычисления выражения </summary>
    public class Calc<ValType> {
        /// <summary> Парсер </summary>
        private InfixParser<Action<Build<ValType>>> parser;

        /// <summary> Создать унарную операцию </summary>
        /// <param name="caption"> Написание операции </param>
        /// <param name="operation"> Действие </param>
        /// <param name="afterValue"> Записывается после значения </param>
        /// <returns> Унарная операция </returns>
        public static UnaryOperation<ValType> CreateUnaryOperation(string caption, Func<ValType, ValType> operation, bool afterValue = false) {
            return new UnaryOperation<ValType>(caption, operation, afterValue);
        }

        /// <summary> Создать бинарную операцию </summary>
        /// <param name="caption"> Написание операции </param>
        /// <param name="operation"> Действие </param>
        /// <param name="isRightAssoc"> Правоассоциативная? или левоассоциативная... </param>
        /// <returns> Бинарная операция </returns>
        public static BinaryOperation<ValType> CreateBinaryOperation(string caption, Func<ValType, ValType, ValType> operation, bool isRightAssoc = false) {
            return new BinaryOperation<ValType>(caption, operation, isRightAssoc);
        }

        /// <summary> Создать калькулятор </summary>
        /// <param name="unaryOperations"> Набор унарных операций </param>
        /// <param name="binaryOperations"> Набор бинарных операций по приоритету </param>
        /// <param name="valueParsers"> Парсеры для значений </param>
        public Calc(IEnumerable<UnaryOperation<ValType>> unaryOperations, IEnumerable<BinaryOperation<ValType>>[] binaryOperations, IEnumerable<Func<CharEnum, Tuple<ValType>>> valueParsers) {
            var uOperations = new Dictionary<string, Action<Build<ValType>>> ();
            var unaryAfterOperations = new Dictionary<string, Action<Build<ValType>>>();
            var bOperations = new Dictionary<string, Action<Build<ValType>>> ();
            var vParsers = new List<Func<CharEnum, Tuple<Action<Build<ValType>>>>>();

            uOperations.Add("(", Build<ValType>.GetAddOpenBracesAction());
            unaryAfterOperations.Add(")", Build<ValType>.GetAddCloseBracesAction());

            foreach (var operation in unaryOperations) {
                switch (operation.OperationType) {
                    case EOperationType.Unary:
                        uOperations.Add(operation.Caption, Build<ValType>.GetAddRightAssocAction(-1, operation.Do));
                        break;
                    case EOperationType.UnaryAfterValue:
                        unaryAfterOperations.Add(operation.Caption, Build<ValType>.GetAddLeftAssocAction(-1, operation.Do));
                        break;
                }
            }

            for (int i = 0; i < binaryOperations.Length; i++) {
                foreach (var operation in binaryOperations[i]) { 
                    switch (operation.OperationType) {
                        case EOperationType.BinaryLeftAssoc:
                            bOperations.Add(operation.Caption, Build<ValType>.GetAddLeftAssocAction(i, operation.Do));
                            break;
                        case EOperationType.BinaryRightAssoc:
                            bOperations.Add(operation.Caption, Build<ValType>.GetAddRightAssocAction(i, operation.Do));
                            break;
                    }
                }
            }
            foreach (var valueParser in valueParsers) {
                vParsers.Add(Build<ValType>.GetAddValueAction(valueParser));
            }

            var parserSettings = new InfixParser<Action<Build<ValType>>>.InfixParserSettings() {
                UnaryOperations = uOperations,
                BinaryOperations = bOperations,
                UnaryOperationsAfter = unaryAfterOperations,
                ConstParsing = vParsers,
                Spaces = null
            };
            parser = new InfixParser<Action<Build<ValType>>>(parserSettings);
        }

        /// <summary> Вычислить значение выражения </summary>
        /// <param name="str"> Выражение </param>
        /// <returns> Результат вычисления </returns>
        /// <exception cref="CantCalcException"> Невозможно вычислить выражение </exception>
        public ValType Process(string str) {
            var build = new Build<ValType>();
            try {
                foreach (var parseItem in parser.Parse(str)) {
                    if (parseItem != null)
                        parseItem(build);
                }
                return build.GetResult();
            } catch (CantCalcException cce) {
                throw cce;
            } catch (CantParseException cpe) {
                throw new CantCalcException("Невозможно разобрать выражение", cpe, cpe.Index);
            } catch (Exception e) {
                throw new CantCalcException("Ошибка", e);
            }
        }
    }
}