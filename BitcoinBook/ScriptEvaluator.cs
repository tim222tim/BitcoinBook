using System;
using System.Collections.Generic;

namespace BitcoinBook
{
    public class ScriptEvaluator
    {
        readonly OperationSelector selector = new OperationSelector();

        public bool Evaluate(IEnumerable<object> scriptCommands, byte[] sigHash)
        {
            var commands = new Stack<object>(scriptCommands ?? throw new ArgumentNullException(nameof(scriptCommands)));
            var stack = new Stack<byte[]>();
            var altStack = new Stack<byte[]>();

            while (commands.Count > 0)
            {
                var command = commands.Pop();
                if (command is byte[] bytes)
                {
                    stack.Push(bytes);
                }
                else if (command is OpCode opCode)
                {
                    var operation = selector.GetOperation(opCode, stack, altStack, commands, sigHash);
                    if (!operation())
                    {
                        return false; // operation failed
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid command type");
                }
            }

            if (stack.Count == 0)
            {
                return false;
            }

            return stack.Pop().Length > 0;
        }
    }
}
