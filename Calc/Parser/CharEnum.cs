using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Parser {
    /// <summary> Для перебора символов в string + получение позиции </summary>
    public sealed class CharEnum {
        /// <summary> Стандартный объект для перебора символов </summary>
        private CharEnumerator enumerator;

        /// <summary> Создать объект для перебора символов </summary>
        /// <param name="str"> Строка, в которой перебираются символы </param>
        public CharEnum(string str) {
            enumerator = str.GetEnumerator();
            Index = -1;
            IsEnd = false;
        }
        /// <summary> Копировать объект из переданного </summary>
        /// <param name="copyFrom"> Объект, который копируется </param>
        private CharEnum(CharEnum copyFrom) {
            Index = copyFrom.Index;
            IsEnd = copyFrom.IsEnd;
            enumerator = (CharEnumerator)copyFrom.enumerator.Clone();
        }

        /// <summary> Идем к следующему символу </summary>
        /// <returns> Есть ли ещё символы </returns>
        public bool MoveNext() {
            if (enumerator.MoveNext()) {
                Index++;
                IsEnd = false;
                return true;
            } else {
                IsEnd = true;
                return false;
            }
        }

        /// <summary> Текущий индекс символа </summary>
        public int Index {
            get;
            private set;
        }

        /// <summary> Клонирует объект для перебора символов </summary>
        /// <returns> Клонированный объект </returns>
        public CharEnum Clone() {
            return new CharEnum(this);
        }

        /// <summary> Текущий символ </summary>
        public char Current {
            get {
                return enumerator.Current;
            }
        }
        /// <summary> Кончился ли перебор </summary>
        public bool IsEnd {
            get;
            private set;
        }
    }
}
