using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.InfixParser
{
    internal sealed class InfixParser
    {
        private bool nowInitialize = true;
        private Action<double> whenConstAction;
        private StateNode valueNode;
        private StateNode operationNode;

        #region Operations for init
        public void AddOperation(string operation, Action whenOperation)
        {
            TestNowInit();
        }

        public void AddUnaryOperation(string operation, Action whenOperation, bool afterValueOperation)
        {
            TestNowInit();
        }

        public Action<double> WhenConstAction {
            set
            {
                TestNowInit();
            }
        }

        private void TestNowInit()
        {
            if (!nowInitialize)
                throw new AggregateException("Объект был переведён в рабочее состояние при помощи GetParser, дальнейшее изменение правил невозможно");
        }
        #endregion

        public Action<string> GetParser()
        {
            nowInitialize = false;
            return Parse;
        }

        private void Parse(string str)
        {

        }
    }
}