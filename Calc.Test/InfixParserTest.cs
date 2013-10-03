using System;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Calc.InfixParser;
using Calc.InfixParser.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calc.Test
{
    [TestClass]
    public class InfixParserTest
    {
        private readonly StringBuilder sb = new StringBuilder();
        private readonly Action<string> parseAction;

        public InfixParserTest()
        {
            parseAction = Parser.StartCreate()
                .SetConstAction(val => sb.Append(val.ToString(CultureInfo.InvariantCulture) + "_"))
                .AddOperation("+", () => sb.Append("+_"))
                .AddOperation("+=", () => sb.Append("+=_"))
                .AddOperation("=", () => sb.Append("=_"))
                .AddOperation("sqrt", () => sb.Append("sqrt_"))
                .AddUnaryOperation("+", () => sb.Append("+_"), false)
                .AddUnaryOperation("-", () => sb.Append("-_"), false)
                .AddUnaryOperation("!", () => sb.Append("!_"), true)
                .AddUnaryOperation("x2", () => sb.Append("x2_"), false)
                .Create();
        }

        [TestMethod]
        public void OnlyConst1()
        {
            sb.Clear();
            parseAction("2");
            Assert.AreEqual("2_", sb.ToString());
        }
        [TestMethod]
        public void OnlyConst2()
        {
            sb.Clear();
            parseAction("2.1");
            Assert.AreEqual("2.1_", sb.ToString());
        }
        [TestMethod]
        public void OnlyConst3()
        {
            sb.Clear();
            parseAction("2.1E+1");
            Assert.AreEqual(2.1E+1.ToString(CultureInfo.InvariantCulture) + "_", sb.ToString());
        }
        [TestMethod]
        public void OnlyConst4()
        {
            sb.Clear();
            parseAction("2.1E1");
            Assert.AreEqual(2.1E+1.ToString(CultureInfo.InvariantCulture) + "_", sb.ToString());
        }
        [TestMethod]
        public void OnlyConst5()
        {
            sb.Clear();
            parseAction("2.1E-1");
            Assert.AreEqual(2.1E-1.ToString(CultureInfo.InvariantCulture) + "_", sb.ToString());
        }
        [TestMethod]
        public void OnlyConst6()
        {
            sb.Clear();
            parseAction("2E3");
            Assert.AreEqual(2E3.ToString(CultureInfo.InvariantCulture) + "_", sb.ToString());
        }

        [TestMethod]
        public void BinaryTest()
        {
            sb.Clear();
            parseAction("2+2");
            Assert.AreEqual("2_+_2_", sb.ToString());
        }

        [TestMethod]
        public void Unary()
        {
            sb.Clear();
            parseAction("2+-+2!");
            Assert.AreEqual("2_+_-_+_2_!_", sb.ToString());
        }

        [TestMethod]
        public void Complex()
        {
            sb.Clear();
            parseAction("2+=2+2=2 sqrt+2+x2-3!");
            Assert.AreEqual("2_+=_2_+_2_=_2_sqrt_+_2_+_x2_-_3_!_", sb.ToString());
        }

        [TestMethod]
        public void ExceptionLetterAfterConst()
        {
            sb.Clear();

            try
            {
                parseAction("2sqrt 2");
            }
            catch (CantParseException e)
            {
                Assert.AreEqual(1, e.Position);
                return;
            }
            catch
            {
                Assert.Fail("Неверный тип исключения");
            }

            Assert.Fail("Исключения не было");
        }

        [TestMethod]
        public void ExceptionConstAfterLetter()
        {
            sb.Clear();

            try
            {
                parseAction("2 sqrt2");
            } catch (CantParseException e)
            {
                Assert.AreEqual(6, e.Position);
                return;
            } catch
            {
                Assert.Fail("Неверный тип исключения");
            }

            Assert.Fail("Исключения не было");
        }
    }

    [TestClass]
    public class InfixParserCreateTest
    {

    }
}
