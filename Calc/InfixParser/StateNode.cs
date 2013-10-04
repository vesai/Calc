using System;
using System.Collections.Generic;
using System.Linq;
using Calc.InfixParser.Exceptions;

namespace Calc.InfixParser
{
    internal sealed class StateNode<TRes>
    {
        private Func<string, TRes> endFunc;
        private StateNode<TRes> endNextNode;
        private readonly Dictionary<char, StateNode<TRes>> charRules = new Dictionary<char, StateNode<TRes>>();
        private readonly List<Func<char, StateNode<TRes>>> otherRules = new List<Func<char, StateNode<TRes>>>();

        public void SetEndState(Func<string, TRes> func, StateNode<TRes> nextNode)
        {
            endFunc = func;
            endNextNode = nextNode;
        }

        public bool RunEndAction(string str, out TRes result, out StateNode<TRes> nextNode)
        {
            result = default(TRes);
            nextNode = endNextNode;
            if (endFunc == null)
                return false;
            result = endFunc(str);
            return true;
        }

        public StateNode<TRes> AddRule(char symbol)
        {
            StateNode<TRes> state;
            if (charRules.TryGetValue(symbol, out state))
                return state;
            state = new StateNode<TRes>();
            charRules.Add(symbol, state);
            return state;
        }

        public void AddRule(char symbol, StateNode<TRes> nextNode)
        {
            charRules.Add(symbol, nextNode);
        }

        public void AddRule(Func<char, bool> condition, StateNode<TRes> nextNode)
        {
            otherRules.Add(c => condition(c) ? nextNode : null);
        }

        public StateNode<TRes> AddRule(Func<char, bool> condition)
        {
            var nextNode = new StateNode<TRes>();
            AddRule(condition, nextNode);
            return nextNode;
        }

        public void AddException(Func<char, bool> condition, string message)
        {
            otherRules.Add(c => { 
                if (condition(c))
                    throw new StateException(message);
                return null;
            });
        }

        public StateNode<TRes> Next(char c)
        {
            StateNode<TRes> state;
            if (charRules.TryGetValue(c, out state))
                return state;
            return otherRules.Select(func => func(c)).FirstOrDefault(x => x != null);
        }
    }
}