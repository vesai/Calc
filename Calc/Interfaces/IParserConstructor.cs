using System;
using System.Collections.Generic;

namespace Calc.Interfaces
{
    public interface IParserConstructor<TRes>
    {
        IParserConstructor<TRes> AddOperation(string operation, TRes operationRes);
        IParserConstructor<TRes> AddUnaryOperation(string operation, TRes operationRes, bool afterValueOperation = false);
        IParserConstructor<TRes> SetConstAction(Func<double, TRes> func);

        Func<string, IEnumerable<TRes>> Create();
    }
}