using System;
using Calc.Exceptions.InfixCalc;
using Calc.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calc.Test
{
    [TestClass]
    public class InfixCalcTest
    {
        private readonly Func<string, double> process;

        public InfixCalcTest()
        {
            process = InfixCalc.Calc.StartCreate()
                .AddOperation("!", Factorial, true)
                .AddOperation("highPriorityPlus1", a => a + 1)
                .GoToLowPriorityGroup()
                .AddOperation("plusOne", a => a + 1, true)
                .AddOperation("+", a => a)
                .AddOperation("-", a => -a)
                .AddOperation("sqrt", Math.Sqrt)
                .GoToLowPriorityGroup()
                .AddOperation("**", Math.Pow, true)
                .GoToLowPriorityGroup()
                .AddOperation("*", (a, b) => a*b)
                .AddOperation("/", (a, b) => a/b)
                .GoToLowPriorityGroup()
                .AddOperation("+", (a, b) => a + b)
                .AddOperation("-", (a, b) => a - b)
                .GoToLowPriorityGroup()
                .AddOperation("lowPriorPower", Math.Pow, true)
                .Create();
        }

        private static double Factorial(double val)
        {
            return (val <= 1)
                ? 1
                : val*Factorial(val - 1);
        }

        [TestMethod]
        public void EsayConst()
        {
            Assert.AreEqual(2, process("2"));
        }

        [TestMethod]
        public void AllTypesConst()
        {
            Assert.AreEqual(2 + 2.0 + 2.0E12 + 2.0E-12 + 2.0e+12 + 2e4 + 2e-6,
                process("2+2.0+2.0E12+2.0E-12+2.0e+12+2e4+2e-6"));
        }

        [TestMethod]
        public void UnaryMinuse()
        {
            Assert.AreEqual(-2, process("-2"));
        }

        [TestMethod]
        public void UnaryPlusesAndMinuses()
        {
            Assert.AreEqual(2 - +3 + 2 + -+- -2, process("2-+3+2+-+--2"));
        }

        [TestMethod]
        public void Braces()
        {
            Assert.AreEqual((2 + 2)*2, process("(2+2)*2"));
        }

        [TestMethod]
        public void Plus()
        {
            Assert.AreEqual(2 + 2, process("2+2"));
        }

        [TestMethod]
        public void PlusAndMul()
        {
            Assert.AreEqual(2 + 2*2, process("2+2*2"));
        }

        [TestMethod]
        public void MulAndDiv()
        {
            Assert.AreEqual(2*4/2, process("2*4/2"));
        }

        [TestMethod]
        public void DivAndMul()
        {
            Assert.AreEqual(2.0/4*2, process("2/4*2"));
        }

        [TestMethod]
        public void Factorial()
        {
            Assert.AreEqual(2 + 4*3*2, process("2! + 4!"));
        }

        [TestMethod]
        public void Power()
        {
            Assert.AreEqual(65536*3 + 4, process("2+3*4**2**3+2"));
        }

        [TestMethod]
        public void Sqrt()
        {
            Assert.AreEqual(8, process("sqrt 64"));
        }

        [TestMethod]
        public void AfterValueUnarOperationOrder1()
        {
            Assert.AreEqual(4*3*2, process("3 plusOne !"));
        }

        [TestMethod]
        public void AfterValueUnarOperationOrder2()
        {
            Assert.AreEqual(3*2 + 1, process("3 ! plusOne"));
        }

        [TestMethod]
        public void UnarOperationOrder1()
        {
            Assert.AreEqual(-3*2, process("-(3!)"));
        }

        [TestMethod]
        public void UnarOperationOrder2()
        {
            Assert.AreEqual(6, process("-(-3!)"));
        }

        [TestMethod]
        public void UnarOperationOrder3()
        {
            Assert.AreEqual(-5, process("- highPriorityPlus1 2 ! plusOne"));
        }

        [TestMethod]
        public void ManySpaces()
        {
            Assert.AreEqual(4, process("   2 +    2   "));
        }

        [TestMethod]
        public void LowPriorityPower()
        {
            Assert.AreEqual(Math.Pow(4, 6), process("2+2 lowPriorPower 5 + 1"));
        }

        [TestMethod]
        public void BracketsException1()
        {
            try
            {
                process("2+(2+2");
            }
            catch (CantCalcException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void BracketsException2()
        {
            try
            {
                process("2+(2+2))");
            }
            catch (CantCalcException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void UnknownFunctionException()
        {
            try
            {
                process("wefiweiuf");
            }
            catch (CantCalcException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void UnknownOperatorException()
        {
            try
            {
                process("///");
            }
            catch (CantCalcException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void DivByZero()
        {
            Assert.AreEqual(double.PositiveInfinity, process("2/0-2"));
        }

        [TestMethod]
        public void InitException1()
        {
            try
            {
                InfixCalc.Calc.StartCreate()
                    .AddOperation("=a", (a, b) => a + b);
            }
            catch (InitCalcException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void InitException2()
        {
            try
            {
                InfixCalc.Calc.StartCreate()
                    .AddOperation("=a", a => a);
            }
            catch (InitCalcException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void InitException3()
        {
            try
            {
                ICalcConstructor creator = InfixCalc.Calc.StartCreate();
                creator.Create();
                creator.AddOperation("a", d => d);
            }
            catch (AggregateException)
            {
                return;
            }
            Assert.Fail();
        }
    }
}