using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixToRPN
{
    interface IConverterConstructor
    {
        IConverterConstructor AddBinaryOperation(string operation, int priority, Action action, bool isRightAssociation = false);
        IConverterConstructor AddUnaryOperation(string operation, int priority, Action action, bool afterValue = false);
        Func<string, IEnumerable<Action>> Create();
    }
}
