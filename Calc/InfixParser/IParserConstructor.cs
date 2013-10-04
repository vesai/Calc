using System;

namespace Calc.InfixParser
{
    public interface IParserConstructor
    {
        IParserConstructor AddOperation(string operation, Action whenOperation);
        IParserConstructor AddUnaryOperation(string operation, Action whenOperation, bool afterValueOperation);
        IParserConstructor SetConstAction(Action<double> action);

        Action<string> Create();
    }
}
