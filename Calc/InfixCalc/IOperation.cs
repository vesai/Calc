using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixCalc {
    /// <summary> Интерфейс операции </summary>
    /// <typeparam name="ValType"> Тип значений </typeparam>
    public interface IOperation<ValType> {
        /// <summary> Тип операции </summary>
        EOperationType OperationType { get; }
        /// <summary> Написание операции </summary>
        string Caption { get; }
        /// <summary> Действие операции </summary>
        /// <param name="stack"> Стек </param>
        void Do(CalcStack<ValType> stack);
    }
}
