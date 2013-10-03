using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
