using System;
using System.Collections.Generic;
using System.Linq;
using Calc.InfixParser.Exceptions;

namespace Calc.InfixParser
{
    internal sealed class StateNode
    {
        private Action<string> endAction;
        private StateNode endNextNode;
        private readonly Dictionary<char, StateNode> charRules = new Dictionary<char, StateNode>();
        private readonly List<Func<char, StateNode>> otherRules = new List<Func<char, StateNode>>();

        public void SetEndState(Action<string> action, StateNode nextNode)
        {
            endAction = action;
            endNextNode = nextNode;
        }

        public StateNode RunEndAction(string str)
        {
            if (endAction == null)
                return null;
            endAction(str);
            return endNextNode;
        }

        public StateNode AddRule(char symbol)
        {
            StateNode state;
            if (charRules.TryGetValue(symbol, out state))
                return state;
            state = new StateNode();
            charRules.Add(symbol, state);
            return state;
        }

        public void AddRule(char symbol, StateNode nextNode)
        {
            charRules.Add(symbol, nextNode);
        }

        public void AddRule(Func<char, bool> condition, StateNode nextNode)
        {
            otherRules.Add(c => condition(c) ? nextNode : null);
        }

        public StateNode AddRule(Func<char, bool> condition)
        {
            var nextNode = new StateNode();
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

        public StateNode Next(char c)
        {
            StateNode state;
            if (charRules.TryGetValue(c, out state))
                return state;
            return otherRules.Select(func => func(c)).FirstOrDefault(x => x != null);
        }
    }
}