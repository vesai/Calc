using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Calc.InfixParser;
using Calc.InfixParser.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calc.Test
{
    [TestClass]
    public class InfixParserTest
    {
        private readonly Func<string, IEnumerable<string>> parseAction;

        public InfixParserTest()
        {
            parseAction = Parser<string>.StartCreate()
                .SetConstAction(val => val.ToString(CultureInfo.InvariantCulture) + "_")
                .AddOperation("+", "+_")
                .AddOperation("+=", "+=_")
                .AddOperation("=", "=_")
                .AddOperation("sqrt", "sqrt_")
                .AddOperation("sqr", "sqr_")
                .AddUnaryOperation("+", "+_", false)
                .AddUnaryOperation("-",  "-_", false)
                .AddUnaryOperation("!",  "!_", true)
                .AddUnaryOperation("x2",  "x2_", false)
                .Create();
        }

        private string Unite(IEnumerable<string> strings)
        {
            return strings.Aggregate(new StringBuilder(), 
                (sb, s) => sb.Append(s), 
                sb => sb.ToString());
        }

        [TestMethod]
        public void OnlyConst1()
        {
            Assert.AreEqual("2_", Unite(parseAction("2")));
        }
        [TestMethod]
        public void OnlyConst2()
        {
            Assert.AreEqual("2.1_", Unite(parseAction("2.1")));
        }
        [TestMethod]
        public void OnlyConst3()
        {
            Assert.AreEqual(2.1E+1.ToString(CultureInfo.InvariantCulture) + "_", Unite(parseAction("2.1E+1")));
        }
        [TestMethod]
        public void OnlyConst4()
        {
            Assert.AreEqual(2.1E+1.ToString(CultureInfo.InvariantCulture) + "_", Unite(parseAction("2.1E1")));
        }
        [TestMethod]
        public void OnlyConst5()
        {
            Assert.AreEqual(2.1E-1.ToString(CultureInfo.InvariantCulture) + "_", Unite(parseAction("2.1E-1")));
        }
        [TestMethod]
        public void OnlyConst6()
        {
            Assert.AreEqual(2E3.ToString(CultureInfo.InvariantCulture) + "_", Unite(parseAction("2E3")));
        }

        [TestMethod]
        public void BinaryTest()
        {
            Assert.AreEqual("2_+_2_", Unite(parseAction("2+2")));
        }

        [TestMethod]
        public void Unary()
        {
            Assert.AreEqual("2_+_-_+_2_!_", Unite(parseAction("2+-+2!")));
        }

        [TestMethod]
        public void Complex()
        {
            Assert.AreEqual("2_+=_2_+_2_=_2_sqrt_+_2_sqr_+_2_+_x2_-_3_!_", 
                Unite(parseAction("2+=2+2=2 sqrt+2 sqr+2+x2-3!")));
        }

        [TestMethod]
        public void ComplexWithSpaces()
        {
            Assert.AreEqual("2_+_2_+_2_=_2_sqrt_+_2_sqr_+_2_+_x2_-_3_!_", 
                Unite(parseAction("2  +     2  +   2   =   2    sqrt  + 2   sqr  +    2  +  x2-  3    !")));
        }

        [TestMethod]
        public void ExceptionLetterAfterConst()
        {
            try
            {
                Unite(parseAction("2sqrt 2"));
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
            try
            {
                Unite(parseAction("2 sqrt2"));
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

        [TestMethod]
        public void ExceptionBadOperator()
        {
            try
            {
                Unite(parseAction("2 sqra 2"));
            } catch (CantParseException e)
            {
                Assert.AreEqual(5, e.Position);
                return;
            } catch
            {
                Assert.Fail("Неверный тип исключения");
            }

            Assert.Fail("Исключения не было");
        }
        [TestMethod]
        public void ExceptionShortOperator()
        {
            try
            {
                Unite(parseAction("2 sq 2"));
            } catch (CantParseException e)
            {
                Assert.AreEqual(4, e.Position);
                return;
            } catch
            {
                Assert.Fail("Неверный тип исключения");
            }

            Assert.Fail("Исключения не было");
        }

        [TestMethod]
        public void ExceptionIncorrectOperator()
        {
            try
            {
                Unite(parseAction("2 sqr! 2"));
            } catch (CantParseException e)
            {
                Assert.AreEqual(5, e.Position);
                return;
            } catch
            {
                Assert.Fail("Неверный тип исключения");
            }

            Assert.Fail("Исключения не было");
        }

        [TestMethod]
        public void ExceptionBadEndState()
        {
            try
            {
                Unite(parseAction("2 sqrt 2 +"));
            } catch (CantParseException e)
            {
                Assert.AreEqual(9, e.Position);
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
        [TestMethod]
        public void NormalOperationsAdd()
        {
            Parser<string>.StartCreate()
                .SetConstAction(val => null)
                .AddOperation("+", null)
                .AddOperation("+=", null)
                .AddOperation("=", null)
                .AddOperation("sqrt", null)
                .AddUnaryOperation("+", null, false)
                .AddUnaryOperation("-", null, false)
                .AddUnaryOperation("!", null, true)
                .AddUnaryOperation("x2", null, false)
                .Create();
        }


        [TestMethod]
        public void IncorrectHasSpace()
        {
            try
            {
                Parser<string>.StartCreate()
                    .SetConstAction(val => null)
                    .AddOperation("sqrt ", null)
                    .Create();
            } catch (ParserCreateException)
            {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void IncorrectNull()
        {
            try
            {
                Parser<string>.StartCreate()
                    .SetConstAction(val => null)
                    .AddOperation(null, null)
                    .Create();
            } catch (ParserCreateException)
            {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void IncorrectBeginDigit()
        {
            try
            {
                Parser<string>.StartCreate()
                    .SetConstAction(val => null)
                    .AddOperation("12+", null)
                    .Create();
            } catch (ParserCreateException)
            {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void IncorrectSymbolOperation()
        {
            try
            {
                Parser<string>.StartCreate()
                    .SetConstAction(val => null)
                    .AddOperation("+a", null)
                    .Create();
            }
            catch (ParserCreateException)
            {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void IncorrectLiteralOperation()
        {
            try
            {
                Parser<string>.StartCreate()
                    .SetConstAction(val => null)
                    .AddOperation("a+", null)
                    .Create();
            } catch (ParserCreateException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void TryChangeAfterCreate()
        {
            try
            {
                var p = Parser<string>.StartCreate()
                    .SetConstAction(val => null)
                    .AddOperation("+", null);
                p.Create();
                p.AddOperation("+", null);
            } catch (AggregateException)
            {
                return;
            }
            Assert.Fail();
        }
    }
}