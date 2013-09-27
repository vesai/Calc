using Calc.InfixParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.InfixCalc;
using System.Globalization;

namespace Calc {
    class Program {
        static void Main(string[] args) {
            DoubleCalc.Calc calc = new DoubleCalc.Calc();
            Console.WriteLine("Введите выражение:");
            Console.Write("  ");
            string str = Console.ReadLine();
            try {
                double res = calc.Process(str);
                Console.WriteLine("Результат:");
                Console.Write("  " + res.ToString(CultureInfo.InvariantCulture));
            } catch (CantCalcException e) {
                if (e.ErrorPosition != null) {
                    Console.WriteLine(str);
                    for (int i = 0; i < e.ErrorPosition.Value; i++) {
                        Console.Write(" ");
                    }
                    Console.WriteLine("^");
                    Console.WriteLine("Символ: " + (e.ErrorPosition.Value+1));
                }
                Console.WriteLine("Ошибка: " + e.Message);
            }
            Console.ReadLine();
        }
    }
}