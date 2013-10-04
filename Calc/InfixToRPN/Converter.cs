using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.InfixParser;

namespace Calc.InfixToRPN
{
    class Converter : IConverterConstructor
    {
        private readonly IParserConstructor parserConstructor;
        private readonly List<Action> parsedActions = new List<Action>(); 

        public static IConverterConstructor ConverterCreate(IParserConstructor parserConstructor)
        {
            return new Converter(parserConstructor);
        }

        private Converter(IParserConstructor parserConstructor)
        {
            this.parserConstructor = parserConstructor;
        }

        #region IConverterConstructor
        IConverterConstructor IConverterConstructor.AddBinaryOperation(string operation, int priority, Action action, bool isRightAssociation)
        {
            parserConstructor.AddOperation(operation, () => parsedActions.Add(action));
            return this;
        }

        IConverterConstructor IConverterConstructor.AddUnaryOperation(string operation, int priority, Action action, bool afterValue)
        {
            parserConstructor.AddUnaryOperation(operation, () => parsedActions.Add(action), afterValue);
            return this;
        }

        Func<string, IEnumerable<Action>> IConverterConstructor.Create()
        {
            return 
        }
        #endregion 
    }
}
