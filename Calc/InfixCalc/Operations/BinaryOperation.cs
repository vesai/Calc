using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixCalc.Operations {
    /// <summary> Бинарная операция </summary>
    /// <typeparam name="ValType"> Тип с которым работает операция </typeparam>
    public class BinaryOperation<ValType> : IOperation<ValType> {
        /// <summary> Операция </summary>
        private Func<ValType, ValType, ValType> operation;
        /// <summary> Является ли операция правоассоциированной </summary>
        private bool isRightAssoc;

        /// <summary> Создать бинарную операцию </summary>
        /// <param name="caption"> Название </param>
        /// <param name="operation"> Действие </param>
        /// <param name="isRightAssoc"> Является ли операция правоассоциированной </param>
        internal BinaryOperation(string caption, Func<ValType, ValType, ValType> operation, bool isRightAssoc = false) {
            this.operation = operation;
            this.isRightAssoc = isRightAssoc;
            this.Caption = caption;
        }

        #region IOperation<ValType>
        public EOperationType OperationType {
            get { return isRightAssoc ? EOperationType.BinaryRightAssoc : EOperationType.BinaryLeftAssoc; }
        }
        public void Do(CalcStack<ValType> stack) {
            var op2 = stack.Pop();
            var op1 = stack.Pop();
            stack.Push(operation(op1, op2));
        }
        public string Caption { get; private set; }
        #endregion
    }
}
