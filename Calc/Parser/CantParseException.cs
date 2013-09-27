using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Parser {
    /// <summary> Невозможно разобрать строку </summary>
    public class CantParseException : Exception {
        /// <summary> Положение ошибки </summary>
        public int Index { get; private set; }

        /// <summary> Создать объект для исключения </summary>
        /// <param name="en"> Перечислитель символов </param>
        public CantParseException(CharEnum en)
            : base("Невозможно разобрать строку, символ: " + en.Index) { Index = en.Index; }
        /// <summary> Создать объект для исключения </summary>
        /// <param name="en"> Перечислитель символов </param>
        /// <param name="innerException"> Внутреннее исключение </param>
        public CantParseException(CharEnum en, Exception innerException)
            : base("Невозможно разобрать строку, символ: " + en.Index + "(" + innerException.Message + ")", innerException) {
                Index = en.Index;
        }
    }
}
