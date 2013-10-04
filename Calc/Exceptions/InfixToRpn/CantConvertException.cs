﻿using System;

namespace Calc.Exceptions.InfixToRpn
{
    public sealed class CantConvertException : Exception
    {
        public CantConvertException(string message)
            : base(message)
        {
        }
    }
}