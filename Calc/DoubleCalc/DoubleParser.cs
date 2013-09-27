using Calc.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.DoubleCalc {
    /// <summary> Парсер для типа double </summary>
    class DoubleParser {
        public static Tuple<double> Parse(CharEnum en) {
            var parser = new DoubleParser();
            double res;
            if (parser.TryParse(en, out res)) {
                return Tuple.Create(res);
            }
            return null;
        }

         /// <summary> Выполнить парсинг строки </summary>
         /// <param name="str"> Строка </param>
         /// <param name="result"> Результат выполнения операции </param>
         /// <returns> Успешен ли парсинг </returns>
         private bool TryParseString(StringBuilder str, out double result) {
             double value;
             if (double.TryParse(str.ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out value)) {
                 result = value;
                 return true;
             }
             result = default(double);
             return false;
         }

         /// <summary> Является ли текущий символ разделителем </summary>
         /// <param name="ce"> Объект для перебора строки </param>
         /// <returns> Является ли разделителем </returns>
         private bool IsSeparator(CharEnum ce) {
             var current = ce.Current;
             return !(char.IsLetterOrDigit(current) || current == '.');
         }

         /// <summary> Добавить цифры в построитель строки </summary>
         /// <param name="sb"> Построитель строки </param>
         /// <param name="chEnum"> Перечислитель исходной строки </param>
         /// <returns> true - встретился разделитель, false - встретился следующий символ числа, null - первая цифра - не число </returns>
         private bool? AddAllDigits(StringBuilder sb, CharEnum chEnum) {
             if (chEnum.IsEnd || !char.IsDigit(chEnum.Current))
                 return null;
             do {
                 sb.Append(chEnum.Current);
             } while (chEnum.MoveNext() && (char.IsDigit(chEnum.Current)));
             return chEnum.IsEnd || IsSeparator(chEnum);
         }

         /// <summary> Попытаться выполнить парсинг </summary>
         /// <param name="inputEnum"> Входное положение объекта для прохождения строки </param>
         /// <param name="result"> Результат текущей операции </param>
         /// <returns> Был ли удачен парсинг </returns>
         private bool TryParse(CharEnum inputEnum, out double result) {
             var sb = new StringBuilder();
             result = default(double);
             // Число до точки
             switch (AddAllDigits(sb, inputEnum)) {
                 case null: return false;
                 case true: return TryParseString(sb, out result);
             }
             // Если есть точка
             if (inputEnum.Current == '.') {
                 sb.Append('.');
                 inputEnum.MoveNext();
                 // Число после точки
                 switch (AddAllDigits(sb, inputEnum)) {
                     case null: return false;
                     case true: return TryParseString(sb, out result);
                 }
             }
             // Буква e после числа
             if (char.ToLower(inputEnum.Current) == 'e') {
                 sb.Append('e');
                 if (!inputEnum.MoveNext()) {
                     // Кончилась после буквы e
                     return false;
                 }
                 if (inputEnum.Current == '+' || inputEnum.Current == '-') {
                     sb.Append(inputEnum.Current);
                     inputEnum.MoveNext();
                 }
                 if (AddAllDigits(sb, inputEnum) == true)
                     return TryParseString(sb, out result);
             }
             return false;
         }
    }
}
