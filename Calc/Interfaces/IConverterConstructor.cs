using System;
using System.Collections.Generic;

namespace Calc.Interfaces
{
    public interface IConverterConstructor<TRes>
    {
        IConverterConstructor<TRes> AddBinaryOperation(string operation, int priority, TRes res,
            bool isRightAssociation = false);

        IConverterConstructor<TRes> AddUnaryOperation(string operation, int priority, TRes res, bool afterValue = false);
        IConverterConstructor<TRes> SetConstAction(Func<double, TRes> func);

        Func<string, IEnumerable<TRes>> Create();
    }
}