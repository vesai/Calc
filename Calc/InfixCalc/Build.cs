using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Parser;

namespace Calc.InfixCalc {
    /// <summary> Стек для построения обратной польской нотацией </summary>
    /// <typeparam name="ValType"> Тип переменных </typeparam>
    class Build<ValType> {
        /// <summary> Стек для типов элементов </summary>
        private Stack<EElementType> elements = new Stack<EElementType>();
        /// <summary> Стек для действий операций </summary>
        private Stack<Action<CalcStack<ValType>>> actions = new Stack<Action<CalcStack<ValType>>>();
        /// <summary> Стек для приоритетов </summary>
        private Stack<int> priority = new Stack<int>();
        /// <summary> Стек для вычисления </summary>
        private CalcStack<ValType> calcStack = new CalcStack<ValType>();

        #region Create static actions and function for work with Build
        public static Action<Build<ValType>> GetAddLeftAssocAction(int priority, Action<CalcStack<ValType>> action) {
             return stack => {
                while (stack.elements.Count > 0 && stack.elements.Peek() != EElementType.OpenBrace && priority >= stack.priority.Peek()) {
                    stack.elements.Pop();
                    stack.priority.Pop();
                    stack.actions.Pop()(stack.calcStack);
                }
                stack.elements.Push(EElementType.Operation);
                stack.actions.Push(action);
                stack.priority.Push(priority);
            };
        }
        public static Action<Build<ValType>> GetAddRightAssocAction(int priority, Action<CalcStack<ValType>> action) {
            return stack => {
                while (stack.elements.Count > 0 && stack.elements.Peek() != EElementType.OpenBrace && priority > stack.priority.Peek()) {
                    stack.elements.Pop();
                    stack.priority.Pop();
                    stack.actions.Pop()(stack.calcStack);
                }
                stack.elements.Push(EElementType.Operation);
                stack.actions.Push(action);
                stack.priority.Push(priority);
            };
        }
        public static Action<Build<ValType>> GetAddOpenBracesAction() {
            return stack => {
                stack.elements.Push(EElementType.OpenBrace);
            };
        }
        public static Action<Build<ValType>> GetAddCloseBracesAction() {
            return stack => {
                try {
                    var x = stack.elements.Pop();
                    while (x != EElementType.OpenBrace) {
                        stack.priority.Pop();
                        stack.actions.Pop()(stack.calcStack);
                        x = stack.elements.Pop();
                    }
                } catch (InvalidOperationException) {
                    throw new CantCalcException("Несогласованные скобки, найдена лишняя закрывающаяся скобка");
                }
            };
        }
        public static Func<CharEnum, Tuple<Action<Build<ValType>>>> GetAddValueAction(Func<CharEnum, Tuple<ValType>> parser) {
            return ce => {
                var resVal = parser(ce);
                if (resVal == null) {
                    return null;
                }
                return Tuple.Create<Action<Build<ValType>>>(build => {
                    build.calcStack.Push(resVal.Item1);
                });
            };
        }
        #endregion

        /// <summary> Получить результат </summary>
        /// <returns> Результат </returns>
        public ValType GetResult() {
            while (actions.Count != 0) {
                if (elements.Pop() == EElementType.OpenBrace)
                    throw new CantCalcException("Не все скобки были закрыты");
                actions.Pop()(this.calcStack);
            }
            return calcStack.GetResult();
        }

        /// <summary> Тип элемента </summary>
        private enum EElementType {
            /// <summary> Операция </summary>
            Operation,
            /// <summary> Открывающаяся скобка </summary>
            OpenBrace
        }
    }
}
