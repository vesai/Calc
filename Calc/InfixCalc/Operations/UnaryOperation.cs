using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixCalc.Operations {
    /// <summary> Унарная операция </summary>
    /// <typeparam name="ValType"> Тип с которым работает операция </typeparam>
    public class UnaryOperation<ValType> : IOperation<ValType> {
        /// <summary> Операция </summary>
        private Func<ValType, ValType> operation;
        /// <summary> Записывается после числа </summary>
        private bool afterValue;

        /// <summary> Создать унарную операцию </summary>
        /// <param name="caption"> Название </param>
        /// <param name="operation"> Действие </param>
        /// <param name="isRightAssoc"> Записывается после числа </param>
        public UnaryOperation(string caption, Func<ValType, ValType> operation, bool afterValue = false) {
            this.operation = operation;
            this.Caption = caption;
            this.afterValue = afterValue;
        }

        #region IOperation<ValType>
        public EOperationType OperationType {
            get { return afterValue ? EOperationType.UnaryAfterValue : EOperationType.Unary; }
        }
        public void Do(CalcStack<ValType> stack) {
            stack.Push(operation(stack.Pop()));
        }
        public string Caption { get; private set; }
        #endregion
    }
}