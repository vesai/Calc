using System.Collections.Generic;
using Calc.Exceptions.InfixToRpn;

namespace Calc.InfixToRpn
{
    public class ConverterStack<TRes>
    {
        private readonly Stack<NodeType> nodeStack = new Stack<NodeType>();
        private readonly Stack<int> priorityStack = new Stack<int>();
        private readonly Stack<TRes> resultStack = new Stack<TRes>();

        public void AddOpenBracket()
        {
            nodeStack.Push(NodeType.Bracket);
        }

        public IEnumerable<TRes> AddCloseBracket()
        {
            foreach (TRes r in EndUnary())
            {
                yield return r;
            }
            while (nodeStack.Count > 0 && nodeStack.Peek() == NodeType.Operation)
            {
                nodeStack.Pop();
                priorityStack.Pop();
                yield return resultStack.Pop();
            }
            if (nodeStack.Count == 0 || nodeStack.Pop() != NodeType.Bracket)
                throw new CantConvertException("Обнаружена непарная закрывающаяся скобка");
        }

        public IEnumerable<TRes> AddBinaryOperation(TRes res, int priority, bool isRightAssociation)
        {
            foreach (TRes r in EndUnary())
            {
                yield return r;
            }
            if (isRightAssociation)
            {
                while (nodeStack.Peek() == NodeType.Operation && priorityStack.Peek() > priority)
                {
                    priorityStack.Pop();
                    nodeStack.Pop();
                    yield return resultStack.Pop();
                }
            }
            else
            {
                while (nodeStack.Peek() == NodeType.Operation && priorityStack.Peek() >= priority)
                {
                    priorityStack.Pop();
                    nodeStack.Pop();
                    yield return resultStack.Pop();
                }
            }
            nodeStack.Push(NodeType.Operation);
            priorityStack.Push(priority);
            resultStack.Push(res);
        }

        public void AddUnaryOperation(TRes res, int priority, bool afterValueOperation)
        {
            nodeStack.Push(afterValueOperation ? NodeType.UnaryAfter : NodeType.UnaryBefore);
            priorityStack.Push(priority);
            resultStack.Push(res);
        }

        private IEnumerable<TRes> EndUnary()
        {
            if (nodeStack.Count == 0)
                yield break;

            var beforePriority = new Queue<int>();
            var beforeRes = new Queue<TRes>();

            var afterPriority = new Stack<int>();
            var afterRes = new Stack<TRes>();

            NodeType stackEl = nodeStack.Peek();
            while ((stackEl == NodeType.UnaryAfter) || (stackEl == NodeType.UnaryBefore))
            {
                nodeStack.Pop();
                switch (stackEl)
                {
                    case NodeType.UnaryAfter:
                        afterPriority.Push(priorityStack.Pop());
                        afterRes.Push(resultStack.Pop());
                        break;
                    case NodeType.UnaryBefore:
                        beforePriority.Enqueue(priorityStack.Pop());
                        beforeRes.Enqueue(resultStack.Pop());
                        break;
                }
                stackEl = nodeStack.Peek();
            }
            while ((beforePriority.Count != 0) && (afterPriority.Count != 0))
            {
                if (beforePriority.Peek() >= afterPriority.Peek())
                {
                    beforePriority.Dequeue();
                    yield return beforeRes.Dequeue();
                }
                else
                {
                    afterPriority.Pop();
                    yield return afterRes.Pop();
                }
            }
            while (beforeRes.Count != 0)
                yield return beforeRes.Dequeue();
            while (afterRes.Count != 0)
                yield return afterRes.Pop();
        }

        public void EndWork()
        {
            if (nodeStack.Count != 0)
                throw new CantConvertException("Обнаружена непарная открывающаяся скобка");
        }

        private enum NodeType
        {
            Operation,
            Bracket,
            UnaryBefore,
            UnaryAfter
        }
    }
}