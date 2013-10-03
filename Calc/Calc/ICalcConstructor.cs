using System;

namespace Calc.Calc
{
    public interface ICalcConstructor
    {
        ICalcConstructor AddOperation(string operation, Func<double, double, double> func, bool isRightAssociation = false);
        ICalcConstructor AddOperation(string operation, Func<double, double> func, bool afterValue = false);
        ICalcConstructor GoToLowPriorityGroup();
        Func<string,double> GetCalculator();
    }
}