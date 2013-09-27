using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixCalc {
    /// <summary> Тип опреации </summary>
    public enum EOperationType {
        /// <summary> Унарная </summary>
        Unary,
        /// <summary> Унарная, следующая после числа </summary>
        UnaryAfterValue,
        /// <summary> Бинарная лево-ассоциированный </summary>
        BinaryLeftAssoc,
        /// <summary> Бинарная право-ассоциированный </summary>
        BinaryRightAssoc
    }
}