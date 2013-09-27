using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixCalc {
    /// <summary> Стек для выполнения операций </summary>
    /// <typeparam name="ValType"> Тип значений для стека </typeparam>
    public sealed class CalcStack<ValType> {
        /// <summary> Стек для ханения промежуточных значений </summary>
        private Stack<ValType> stack = new Stack<ValType>();

        /// <summary> Положить объект в стек </summary>
        /// <param name="value"> То, что кладем в стек </param>
        public void Push(ValType value) {
            stack.Push(value);
        }

        /// <summary> Достать объект из стека </summary>
        /// <returns> Элемент, который достали из стека </returns>
        public ValType Pop() {
            return stack.Pop();
        }

        /// <summary> Получить результат </summary>
        /// <returns> Результат </returns>
        public ValType GetResult() {
            var val = stack.Pop();
            return val;
        }
    }
}
