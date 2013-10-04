using System;
using System.Collections.Generic;
using Calc.Exceptions.InfixToRpn;
using Calc.Interfaces;

namespace Calc.InfixToRpn
{
    public sealed class Converter<TRes> : IConverterConstructor<TRes>
    {
        public delegate void ConverterAction(List<TRes> items, ConverterStack<TRes> stack);

        private readonly IParserConstructor<ConverterAction> parserConstructor;

        private Converter(IParserConstructor<ConverterAction> parserConstructor)
        {
            this.parserConstructor = parserConstructor;
            parserConstructor.AddUnaryOperation("(",
                (list, stack) => stack.AddOpenBracket());
            parserConstructor.AddUnaryOperation(")",
                (list, stack) => list.AddRange(stack.AddCloseBracket()), true);
        }

        public static IConverterConstructor<TRes> StartCreate(IParserConstructor<ConverterAction> parserConstructor)
        {
            return new Converter<TRes>(parserConstructor);
        }

        private static IEnumerable<TRes> Convert(string str, Func<string, IEnumerable<ConverterAction>> parser)
        {
            var stack = new ConverterStack<TRes>();
            var list = new List<TRes>();
            stack.AddOpenBracket();
            foreach (ConverterAction action in parser(str))
                action(list, stack);
            list.AddRange(stack.AddCloseBracket());
            stack.EndWork();
            return list;
        }

        #region IConverterConstructor

        IConverterConstructor<TRes> IConverterConstructor<TRes>.AddBinaryOperation(string operation, int priority,
            TRes res, bool isRightAssociation)
        {
            try
            {
                parserConstructor.AddOperation(operation,
                    (items, stack) => items.AddRange(stack.AddBinaryOperation(res, priority, isRightAssociation)));
            }
            catch (Exception e)
            {
                throw new InitConverterException(e);
            }
            return this;
        }

        IConverterConstructor<TRes> IConverterConstructor<TRes>.AddUnaryOperation(string operation, int priority,
            TRes res, bool afterValue)
        {
            try
            {
                parserConstructor.AddUnaryOperation(operation,
                    (items, stack) => stack.AddUnaryOperation(res, priority, afterValue), afterValue);
            }
            catch (Exception e)
            {
                throw new InitConverterException(e);
            }
            return this;
        }

        IConverterConstructor<TRes> IConverterConstructor<TRes>.SetConstAction(Func<double, TRes> func)
        {
            try
            {
                parserConstructor.SetConstAction(val => ((items, stack) => items.Add(func(val))));
            }
            catch (Exception e)
            {
                throw new InitConverterException(e);
            }

            return this;
        }

        Func<string, IEnumerable<TRes>> IConverterConstructor<TRes>.Create()
        {
            Func<string, IEnumerable<ConverterAction>> parser = parserConstructor.Create();
            return str => Convert(str, parser);
        }

        #endregion
    }
}