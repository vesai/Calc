using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Calc.Exceptions.InfixParser;
using Calc.Interfaces;

namespace Calc.InfixParser
{
    public sealed class Parser<TRes> : IParserConstructor<TRes>
    {
        private readonly StateNode<TRes> operationNode = new StateNode<TRes>();
        private readonly StateNode<TRes> valueNode = new StateNode<TRes>();
        private bool nowInitialize = true;
        private Func<double, TRes> whenConstFunc;

        private Parser()
        {
            AddParseDouble();
            AddSkipSpaces();
        }

        /// <summary> Начать создание парсера </summary>
        /// <returns> Объект для создания парсера </returns>
        public static IParserConstructor<TRes> StartCreate()
        {
            return new Parser<TRes>();
        }

        private TRes ConstSend(string value)
        {
            return whenConstFunc(double.Parse(value, CultureInfo.InvariantCulture));
        }

        private void AddParseDouble()
        {
            const string exceptionText = "После числа не может стоять буква";
            StateNode<TRes> digit = operationNode.AddRule(char.IsDigit);

            StateNode<TRes> afterE = digit.AddRule(c => char.ToLowerInvariant(c) == 'e');
            StateNode<TRes> afterPoint = digit.AddRule('.').AddRule(char.IsDigit);
            digit.AddRule(char.IsDigit, digit);
            digit.SetEndState(ConstSend, valueNode);
            digit.AddException(char.IsLetter, exceptionText);

            afterPoint.AddRule(char.IsDigit, afterPoint);
            afterPoint.AddRule(c => char.ToLowerInvariant(c) == 'e', afterE);
            afterPoint.SetEndState(ConstSend, valueNode);
            afterPoint.AddException(char.IsLetter, exceptionText);

            StateNode<TRes> eValue = afterE.AddRule('+');
            afterE.AddRule('-', eValue);
            StateNode<TRes> eDigits = afterE.AddRule(char.IsDigit);

            eValue.AddRule(char.IsDigit, eDigits);

            eDigits.AddRule(char.IsDigit, eDigits);
            eDigits.SetEndState(ConstSend, valueNode);
            eDigits.AddException(char.IsLetter, exceptionText);
        }

        private void AddSkipSpaces()
        {
            valueNode.AddRule(' ').SetEndState(null, valueNode);
            operationNode.AddRule(' ').SetEndState(null, operationNode);
        }

        private IEnumerable<TRes> Parse(string str)
        {
            StateNode<TRes> currentNode = operationNode;
            var currentStr = new StringBuilder();
            TRes res;
            for (int index = 0; index < str.Length; index++)
            {
                char currentSymbol = str[index];
                StateNode<TRes> nextNode;
                try
                {
                    nextNode = currentNode.Next(currentSymbol);
                }
                catch (StateException e)
                {
                    throw new CantParseException(e.Message, index);
                }
                if (nextNode == null)
                {
                    if (currentNode.RunEndAction(currentStr.ToString(), out res, out currentNode))
                        yield return res;
                    if (currentNode == null)
                        throw new CantParseException("Неожиданный символ", index);
                    currentStr.Clear();
                    currentNode = currentNode.Next(currentSymbol);
                    if (currentNode == null)
                        throw new CantParseException("Неожиданный символ", index);
                }
                else
                    currentNode = nextNode;
                currentStr.Append(currentSymbol);
            }
            if (currentNode.RunEndAction(currentStr.ToString(), out res, out currentNode))
                yield return res;
            if (currentNode != valueNode)
                throw new CantParseException("Неожиданный конец выражения", str.Length - 1);
        }

        private void TestNowInit()
        {
            if (!nowInitialize)
                throw new AggregateException(
                    "Объект был переведён в рабочее состояние при помощи GetParser, дальнейшее изменение правил невозможно");
        }

        private static bool TestIdentificator(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new InitParserException("Операция не может быть пустой или состоять их пробелов");
            if (str.Any(c => c == ' '))
                throw new InitParserException("Операция не может содержать пробелов");
            if (char.IsDigit(str[0]))
                throw new InitParserException("Невозможно добавить операцию, начинающуюся с цифры: \"" + str + "\"");

            string noIdExceptionText =
                "Невозможно добавить операцию, должен быть либо идентификатор, либо набор символов: \"" + str + "\"";

            if (char.IsLetter(str[0]))
            {
                if (str.All(char.IsLetterOrDigit))
                    return true;
                throw new InitParserException(noIdExceptionText);
            }
            if (str.Any(char.IsLetterOrDigit))
                throw new InitParserException(noIdExceptionText);
            return false;
        }

        private static void AddChainNodes(StateNode<TRes> fromNode, StateNode<TRes> toNode, string chain, TRes result)
        {
            bool ident = TestIdentificator(chain);
            StateNode<TRes> lastState = chain.Aggregate(fromNode, (node, c) => node.AddRule(c));
            if (ident)
                lastState.AddException(char.IsLetterOrDigit, "Невозможно распознать идентификатор");
            lastState.SetEndState(c => result, toNode);
        }

        #region IParserConstructor

        IParserConstructor<TRes> IParserConstructor<TRes>.AddOperation(string operation, TRes operationRes)
        {
            TestNowInit();
            AddChainNodes(valueNode, operationNode, operation, operationRes);
            return this;
        }

        IParserConstructor<TRes> IParserConstructor<TRes>.AddUnaryOperation(string operation, TRes operationRes,
            bool afterValueOperation)
        {
            TestNowInit();
            if (afterValueOperation)
                AddChainNodes(valueNode, valueNode, operation, operationRes);
            else
                AddChainNodes(operationNode, operationNode, operation, operationRes);
            return this;
        }

        IParserConstructor<TRes> IParserConstructor<TRes>.SetConstAction(Func<double, TRes> func)
        {
            TestNowInit();
            whenConstFunc = func;
            return this;
        }

        Func<string, IEnumerable<TRes>> IParserConstructor<TRes>.Create()
        {
            TestNowInit();
            nowInitialize = false;
            if (whenConstFunc == null)
                throw new AggregateException("Необходимо задать SetConstAction перед вызовом Create");
            return Parse;
        }

        #endregion
    }
}