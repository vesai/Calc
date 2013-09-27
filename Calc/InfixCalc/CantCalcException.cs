using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixCalc {
    /// <summary> Невозможно вычислить выражение </summary>
    public class CantCalcException : Exception {
        /// <summary> Позиция в которой произведена ошибка или null </summary>
        public int? ErrorPosition { get; private set; }

        /// <summary> Создать исключение невозможно вычислить выражение </summary>
        /// <param name="message"> Сообщение </param>
        /// <param name="postion"> Позиция </param>
        public CantCalcException(string message, int? postion = null)
            : base(message) {
                ErrorPosition = postion;
        }
        /// <summary> Создать исключение невозможно вычислить выражение </summary>
        /// <param name="message"> Сообщение </param>
        /// <param name="innerException"> Внутреннее исключение </param>
        /// <param name="postion"> Позиция </param>
        public CantCalcException(string message, Exception innerException, int? postion = null)
            : base(message, innerException) {
            ErrorPosition = postion;
        }
    }
}
