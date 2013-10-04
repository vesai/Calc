using System;
using Calc.Exceptions.InfixCalc;

namespace Calc
{
    internal class Program
    {
        private static Func<string, double> CreateCalc()
        {
            return InfixCalc.Calc.StartCreate()
                .AddOperation("+", a => a)
                .AddOperation("-", a => -a)
                .AddOperation("^", Math.Pow, true)
                .AddOperation("sqrt", Math.Sqrt)
                .GoToLowPriorityGroup()
                .AddOperation("*", (a, b) => a*b)
                .AddOperation("/", (a, b) => a/b)
                .GoToLowPriorityGroup()
                .AddOperation("+", (a, b) => a + b)
                .AddOperation("-", (a, b) => a - b)
                .Create();
        }

        private static void Main()
        {
            Func<string, double> calc = CreateCalc();
            string inExpression;
            do
            {
                Console.Write("Введите выражение для вычисления:\n  ");
                inExpression = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(inExpression));
            try
            {
                Console.WriteLine("Результат: {0}", calc(inExpression));
            }
            catch (CantCalcException e)
            {
                if (e.Position.HasValue)
                {
                    for (int i = 0; i < e.Position.Value + 2; i++)
                        Console.Write(' ');
                    Console.WriteLine('^');
                }
                Console.WriteLine("Ошибка: {0}", e.Message);
            }
            Console.ReadLine();
        }
    }
}