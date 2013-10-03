using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Calc;

namespace Calc.InfixCalc
{
    class Calc : ICalcConstructor
    {
        private bool nowInitialize = true;

        public static ICalcConstructor StartCreate()
        {
            return  new Calc();
        }

        private Calc()
        {
        }

        #region ICalcConstructor

        ICalcConstructor ICalcConstructor.AddOperation(string operation, Func<double, double, double> func, bool isRightAssociation)
        {
            TestNowInit();
            return this;
        }

        ICalcConstructor ICalcConstructor.AddOperation(string operation, Func<double, double> func, bool afterValue)
        {
            TestNowInit();
            return this;
        }

        ICalcConstructor ICalcConstructor.GoToLowPriorityGroup()
        {
            TestNowInit();
            return this;
        }

        Func<string, double> ICalcConstructor.Create()
        {
            nowInitialize = false;
            return Calculate;
        }

        #endregion

        private void TestNowInit()
        {
            if (!nowInitialize)
                throw new AggregateException("Объект был переведён в рабочее состояние при помощи Create, дальнейшее изменение правил невозможно");
        }

        private double Calculate(string expression)
        {
            return 0;
        }
    }
}
