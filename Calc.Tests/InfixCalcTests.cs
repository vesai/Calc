using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.InfixCalc;
using Calc.InfixCalc.Operations;
using Calc.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calc.Tests {
    [TestClass]
    public class InfixCalcTests {
        private Calc<double> calc;

        public InfixCalcTests() {
            calc = new Calc<double>(
                new UnaryOperation<double>[] {
                    Calc<double>.CreateUnaryOperation("+", a => a),
                    Calc<double>.CreateUnaryOperation("-", a => -a),
                    Calc<double>.CreateUnaryOperation("sqrt", a => Math.Sqrt(a)),
                    Calc<double>.CreateUnaryOperation("!:", a => {
                        int res = 1;
                        for (int i = 2; i <= a; i++) {
                            res += i;
                        }
                        return res;
                    }, true),
                    Calc<double>.CreateUnaryOperation("!", a => {
                        int res = 1;
                        for (int i = 2; i <= a; i++) {
                            res *= i;
                        }
                        return res;
                    }, true),
                    Calc<double>.CreateUnaryOperation("plusOne", a => a+1, true),
                    Calc<double>.CreateUnaryOperation("x2", a => a*2),
                    Calc<double>.CreateUnaryOperation("p3", a => a+3),
                },
                new BinaryOperation<double>[][] {
                    new BinaryOperation<double> [] {
                        Calc<double>.CreateBinaryOperation("**", (a,b) => Math.Pow(a,b), true),
                    },
                    new BinaryOperation<double> [] {
                        Calc<double>.CreateBinaryOperation("*", (a,b) => a*b),
                        Calc<double>.CreateBinaryOperation("/", (a,b) => a/b),
                    },
                    new BinaryOperation<double> [] {
                        Calc<double>.CreateBinaryOperation("+", (a,b) => a+b),
                        Calc<double>.CreateBinaryOperation("-", (a,b) => a-b),
                    },
                    new BinaryOperation<double> [] {
                        Calc<double>.CreateBinaryOperation("lowPriorPower", (a,b) => Math.Pow(a,b), true),
                    },
                }, new Func<CharEnum, Tuple<double>>[] { DoubleParser.Parse });
        }

        [TestMethod]
        public void EsayConst() {
            Assert.AreEqual(2, calc.Process("2"));
        }
        [TestMethod]
        public void AllTypesConst() {
            Assert.AreEqual(2 + 2.0 + 2.0E12 + 2.0E-12 + 2.0e+12 + 2e4 + 2e-6, calc.Process("2+2.0+2.0E12+2.0E-12+2.0e+12+2e4+2e-6"));
        }
        [TestMethod]
        public void UnaryMinuse() {
            Assert.AreEqual(-2, calc.Process("-2"));
        }
        [TestMethod]
        public void UnaryPlusesAndMinuses() {
            Assert.AreEqual(2 - + 3 + 2 + - + - -2, calc.Process("2-+3+2+-+--2"));
        }
        [TestMethod]
        public void Braces() {
            Assert.AreEqual((2 + 2) * 2, calc.Process("(2+2)*2"));
        }
        [TestMethod]
        public void Plus() {
            Assert.AreEqual(2 + 2, calc.Process("2+2"));
        }
        [TestMethod]
        public void PlusAndMul() {
            Assert.AreEqual(2 + 2 * 2, calc.Process("2+2*2"));
        }
        [TestMethod]
        public void MulAndDiv() {
            Assert.AreEqual(2 * 4 / 2, calc.Process("2*4/2"));
        }
        [TestMethod]
        public void DivAndMul() {
            Assert.AreEqual(2.0 / 4 * 2, calc.Process("2/4*2"));
        }
        [TestMethod]
        public void Factorial() {
            Assert.AreEqual(2+4*3*2, calc.Process("2! + 4!"));
        }
        [TestMethod]
        public void Power() {
            Assert.AreEqual(65536*3+4, calc.Process("2+3*4**2**3+2"));
        }
        [TestMethod]
        public void Sqrt() {
            Assert.AreEqual(8, calc.Process("sqrt 64"));
        }
        [TestMethod]
        public void AfterValueUnarOperationOrder1() {
            Assert.AreEqual(4*3*2, calc.Process("3 plusOne !"));
        }
        [TestMethod]
        public void AfterValueUnarOperationOrder2() {
            Assert.AreEqual(3 * 2 + 1, calc.Process("3 ! plusOne"));
        }
        [TestMethod]
        public void UnarOperationOrder1() {
            Assert.AreEqual(-3*2, calc.Process("-(3!)"));
        }
        [TestMethod]
        public void UnarOperationOrder2() {
            Assert.AreEqual(-1, calc.Process("-(-3!)"));
        }
        [TestMethod]
        public void BeforeValueUnarOperationOrder() {
            Assert.AreEqual(5, calc.Process("p3 x2 1"));
        }
        [TestMethod]
        public void ManySpaces() {
            Assert.AreEqual(4, calc.Process("   2 +    2   "));
        }
        [TestMethod]
        public void LowPriorityPower() {
            Assert.AreEqual(Math.Pow(4,6), calc.Process("2+2 lowPriorPower 5 + 1"));
        }
        [TestMethod]
        public void BracketsException1() {
            try {
                calc.Process("2+(2+2");
            } catch (CantCalcException) {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void BracketsException2() {
            try {
                calc.Process("2+(2+2))");
            } catch (CantCalcException) {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void UnknownFunctionException() {
            try {
                calc.Process("wefiweiuf");
            } catch (CantCalcException) {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void UnknownOperatorException() {
            try {
                calc.Process("///");
            } catch (CantCalcException) {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void DivByZero() {
            Assert.AreEqual(double.PositiveInfinity, calc.Process("2/0-2"));
        }
    }
}
