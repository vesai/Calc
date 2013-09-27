using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixParser {
    /// <summary> Состояния парсера для инфиксной нотации </summary>
    enum EInfixStates {
        /// <summary> Операция, ставится после бинарной или унарной операции, а также открывающейся скобкой </summary>
        Operation, 
        /// <summary> Значение, ставится после значения или закрывающейся скобки </summary>
        Value
    }
}
