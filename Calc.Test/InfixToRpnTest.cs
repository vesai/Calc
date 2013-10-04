using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Calc.Exceptions.InfixToRpn;
using Calc.InfixParser;
using Calc.InfixToRpn;
using Calc.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calc.Test
{
    [TestClass]
    public class InfixToRpnTest
    {
        private readonly Func<string, IEnumerable<string>> converter;

        public InfixToRpnTest()
        {
            converter = CreateConstructor()
                .AddBinaryOperation("+", -1, "+ ")
                .AddBinaryOperation("-", -1, "- ")
                .AddBinaryOperation("*", 0, "* ")
                .AddBinaryOperation("/", 0, "/ ")
                .AddUnaryOperation("+", 2, "+u ")
                .AddUnaryOperation("-", 2, "-u ")
                .AddUnaryOperation("!", 3, "! ", true)
                .AddUnaryOperation("#", 2, "# ", true)
                .AddBinaryOperation("^", 1, "^ ", true)
                .AddBinaryOperation("lowPriorityPower", -4, "lowPriorityPower ", true)
                .SetConstAction(val => val.ToString(CultureInfo.InvariantCulture) + " ")
                .Create();
        }

        private IConverterConstructor<string> CreateConstructor()
        {
            return Converter<string>.StartCreate(Parser<Converter<string>.ConverterAction>.StartCreate());
        }

        private string Unite(IEnumerable<string> strings)
        {
            return strings.Aggregate(new StringBuilder(),
                (sb, s) => sb.Append(s),
                sb => sb.ToString());
        }

        [TestMethod]
        public void Esay()
        {
            Assert.AreEqual("2 2 + ", Unite(converter("2+2")));
        }

        [TestMethod]
        public void SomeOperations1()
        {
            Assert.AreEqual("2 2 2 * + ", Unite(converter("2+2*2")));
        }

        [TestMethod]
        public void SomeOperations2()
        {
            Assert.AreEqual("2 2 * 2 + ", Unite(converter("2*2+2")));
        }

        [TestMethod]
        public void SomeOperationsWithBraces()
        {
            Assert.AreEqual("2 2 + 2 * ", Unite(converter("(2+2)*2")));
        }

        [TestMethod]
        public void SomePluses()
        {
            Assert.AreEqual("1 2 + 3 + 4 + ", Unite(converter("1+2+3+4")));
        }

        [TestMethod]
        public void SomePower()
        {
            Assert.AreEqual("1 2 3 4 ^ ^ ^ ", Unite(converter("1^2^3^4")));
        }

        [TestMethod]
        public void PowerAndPluses()
        {
            Assert.AreEqual("0 1 + 2 3 + 4 lowPriorityPower lowPriorityPower ",
                Unite(converter("0+1 lowPriorityPower 2+3 lowPriorityPower 4")));
        }

        [TestMethod]
        public void Unary()
        {
            Assert.AreEqual("2 ! ! -u -u +u +u # # ", Unite(converter("++--2!!##")));
        }

        [TestMethod]
        public void ManyUnary()
        {
            Assert.AreEqual("5 ! -u 4 2 * + ! +u -u ! -u ", Unite(converter("-(-+(-5!+4*2)!)!")));
        }

        [TestMethod]
        public void CantFindOpenBraces()
        {
            try
            {
                Unite(converter("2+2)"));
            }
            catch (CantConvertException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void CantFindCloseBraces()
        {
            try
            {
                Unite(converter("(2+2"));
            }
            catch (CantConvertException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void InitError1()
        {
            try
            {
                CreateConstructor()
                    .Create();
            }
            catch (AggregateException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void InitError2()
        {
            try
            {
                CreateConstructor()
                    .SetConstAction(a => null)
                    .AddBinaryOperation("=a", 1, null)
                    .Create();
            }
            catch (InitConverterException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void InitError3()
        {
            try
            {
                CreateConstructor()
                    .SetConstAction(a => null)
                    .AddUnaryOperation("=a", 1, null)
                    .Create();
            }
            catch (InitConverterException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void InitError4()
        {
            try
            {
                IConverterConstructor<string> constr = CreateConstructor()
                    .SetConstAction(a => null)
                    .AddUnaryOperation("a", 1, null);
                constr.Create();
                constr.SetConstAction(a => null);
            }
            catch (InitConverterException)
            {
                return;
            }
            Assert.Fail();
        }
    }
}