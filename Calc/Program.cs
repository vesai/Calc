using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Calc;

namespace Calc
{
    class Program 
    {
        private static Func<string, double> CreateCalc()
        {
            return InfixCalc.Calc.StartCreate()
                .AddOperation("+", a => a)
                .AddOperation("-", a => -a)
                .GoToLowPriorityGroup()
                .AddOperation("*", (a, b) => a*b)
                .AddOperation("/", (a, b) => a/b)
                .GoToLowPriorityGroup()
                .AddOperation("+", (a,b) =>  a+b)
                .AddOperation("-", (a,b) => a-b)
                .Create();
        }

        private static void Main(string[] args)
        {
            var calc = CreateCalc();
            string inExpression;
            do
            {
                Console.Write("Введите выражение для вычисления:\n  ");
                inExpression = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(inExpression));
            try
            {
                Console.WriteLine("Результат: {0}", calc(inExpression));
            } catch(CantCalcException e)
            {
                if (e.Position.HasValue)
                {
                    for (var i = 0; i < e.Position.Value+1; i++)
                        Console.Write(' ');
                    Console.WriteLine('^');
                }
                Console.WriteLine("Ошибка: {0}", e.Message);
            }
            Console.ReadLine();
        }
    }
}