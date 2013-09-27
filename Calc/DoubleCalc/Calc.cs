using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.InfixCalc;
using Calc.InfixCalc.Operations;
using Calc.Parser;

namespace Calc.DoubleCalc {
    /// <summary> Класс для вычисления выражения из предопределенных функций, записанных строками </summary>
    public class Calc {
        /// <summary> Объект для вычисления </summary>
        Calc<double> calc;

        /// <summary> Создать объект для вычисления выражения из предопределенных функций, записанных строками </summary>
        public Calc() {
            calc = new Calc<double>(
                new UnaryOperation<double> [] {
                    Calc<double>.CreateUnaryOperation("+", a => a),
                    Calc<double>.CreateUnaryOperation("-", a => -a),
                    Calc<double>.CreateUnaryOperation("sqrt", a => Math.Sqrt(a)),
                    Calc<double>.CreateUnaryOperation("!", a => {
                        int res = 1;
                        for (int i = 2; i <= a; i++) {
                            res *= i;
                        }
                        return res;
                    }, true),
                },
                new BinaryOperation<double> [][] {
                    new BinaryOperation<double> [] {
                        Calc<double>.CreateBinaryOperation("^", (a,b) => Math.Pow(a,b), true),
                    },
                    new BinaryOperation<double> [] {
                        Calc<double>.CreateBinaryOperation("*", (a,b) => a*b),
                        Calc<double>.CreateBinaryOperation("/", (a,b) => a/b),
                    },
                    new BinaryOperation<double> [] {
                        Calc<double>.CreateBinaryOperation("+", (a,b) => a+b),
                        Calc<double>.CreateBinaryOperation("-", (a,b) => a-b),
                    }
                }, new Func<CharEnum, Tuple<double>>[] { DoubleParser.Parse });
        }

        /// <summary> Вычислить </summary>
        /// <param name="str"> Строка для вычисления </param>
        /// <returns> Полученное значение </returns>
        /// <exception cref="CantCalcException"> Невозможно вычислить выражение </exception>
        public double Process(string str) {
            return calc.Process(str);
        }
    }
}
