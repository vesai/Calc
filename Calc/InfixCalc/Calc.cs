using System;
using System.Collections.Generic;
using System.Linq;
using Calc.Exceptions.InfixCalc;
using Calc.Exceptions.InfixParser;
using Calc.InfixParser;
using Calc.InfixToRpn;
using Calc.Interfaces;

namespace Calc.InfixCalc
{
    /// <summary> Калькулятор </summary>
    public class Calc : ICalcConstructor
    {
        private readonly IConverterConstructor<StackOperation> converter;
        private bool nowInitialize = true;
        private int priority;

        private Calc()
        {
            IParserConstructor<Converter<StackOperation>.ConverterAction> parserCreate =
                Parser<Converter<StackOperation>.ConverterAction>.StartCreate();
            converter = Converter<StackOperation>.StartCreate(parserCreate);
            converter.SetConstAction(d => (stack => stack.Push(d)));
        }

        /// <summary> Начать инициализацию калькулятора </summary>
        /// <returns> Интерфейс для инициализации </returns>
        public static ICalcConstructor StartCreate()
        {
            return new Calc();
        }

        private static StackOperation ConvertOperation(Func<double, double, double> func)
        {
            return stack =>
            {
                double x = stack.Pop();
                stack.Push(func(stack.Pop(), x));
            };
        }

        private static StackOperation ConvertOperation(Func<double, double> func)
        {
            return stack => stack.Push(func(stack.Pop()));
        }

        private void TestNowInit()
        {
            if (!nowInitialize)
                throw new AggregateException(
                    "Объект был переведён в рабочее состояние при помощи Create, дальнейшее изменение правил невозможно");
        }

        private static double Calculate(string expression, Func<string, IEnumerable<StackOperation>> convertFunc)
        {
            try
            {
                return convertFunc(expression).Aggregate(new Stack<double>(),
                    (stack, operation) =>
                    {
                        operation(stack);
                        return stack;
                    }, stack => stack.Pop());
            }
            catch (CantParseException e)
            {
                throw new CantCalcException(e.Position, e);
            }
            catch (Exception e)
            {
                throw new CantCalcException(e);
            }
        }

        #region ICalcConstructor

        ICalcConstructor ICalcConstructor.AddOperation(string operation, Func<double, double, double> func,
            bool isRightAssociation)
        {
            TestNowInit();
            try
            {
                converter.AddBinaryOperation(operation, priority, ConvertOperation(func), isRightAssociation);
            }
            catch (Exception e)
            {
                throw new InitCalcException(e);
            }
            return this;
        }

        ICalcConstructor ICalcConstructor.AddOperation(string operation, Func<double, double> func, bool afterValue)
        {
            TestNowInit();
            try
            {
                converter.AddUnaryOperation(operation, priority, ConvertOperation(func), afterValue);
            }
            catch (Exception e)
            {
                throw new InitCalcException(e);
            }
            return this;
        }

        ICalcConstructor ICalcConstructor.GoToLowPriorityGroup()
        {
            TestNowInit();
            priority--;
            return this;
        }

        Func<string, double> ICalcConstructor.Create()
        {
            TestNowInit();
            nowInitialize = false;
            Func<string, IEnumerable<StackOperation>> convertFunc = converter.Create();
            return str => Calculate(str, convertFunc);
        }

        #endregion

        internal delegate void StackOperation(Stack<double> stack);
    }
}