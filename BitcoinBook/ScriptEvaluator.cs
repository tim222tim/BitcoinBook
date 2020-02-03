using System;
using System.Collections.Generic;
using System.Numerics;

namespace BitcoinBook
{
    public class ScriptEvaluator
    {
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
                    var result = false;
                    switch (GetOpertationType(opCode))
                    {
                        case OpertationType.Stack:
                            result = Evaluate(opCode, stack);
                            break;
                        case OpertationType.SigHash:
                            result = Evaluate(opCode, stack, sigHash);
                            break;
                        case OpertationType.Commands:
                            result = Evaluate(opCode, stack, commands);
                            break;
                        case OpertationType.AltStack:
                            result = Evaluate(opCode, stack, altStack);
                            break;
                    }
                    if (!result)
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

        bool Evaluate(OpCode opCode, Stack<byte[]> stack)
        {
            switch (opCode)
            {
                case OpCode.OP_0:
                    return Push(stack, new byte[0]);
                case OpCode.OP_1NEGATE:
                    return Push(stack, -1);
                case OpCode.OP_1:
                case OpCode.OP_2:
                case OpCode.OP_3:
                case OpCode.OP_4:
                case OpCode.OP_5:
                case OpCode.OP_6:
                case OpCode.OP_7:
                case OpCode.OP_8:
                case OpCode.OP_9:
                case OpCode.OP_10:
                case OpCode.OP_11:
                case OpCode.OP_12:
                case OpCode.OP_13:
                case OpCode.OP_14:
                case OpCode.OP_15:
                case OpCode.OP_16:
                    return Push(stack, (int)opCode - (int)OpCode.OP_1 + 1);
                case OpCode.OP_NOP:
                case OpCode.OP_NOP1:
                case OpCode.OP_NOP4:
                case OpCode.OP_NOP5:
                case OpCode.OP_NOP6:
                case OpCode.OP_NOP7:
                case OpCode.OP_NOP8:
                case OpCode.OP_NOP9:
                case OpCode.OP_NOP10:
                    return true;
                default:
                    throw new InvalidOperationException("Unknown operation");
            }
        }

        bool Evaluate(OpCode opCode, Stack<byte[]> stack, byte[] sigHash)
        {
            throw new NotImplementedException();
        }

        bool Evaluate(OpCode opCode, Stack<byte[]> stack, Stack<object> commands)
        {
            throw new NotImplementedException();
        }

        bool Evaluate(OpCode opCode, Stack<byte[]> stack, Stack<byte[]> altStack)
        {
            throw new NotImplementedException();
        }

        OpertationType GetOpertationType(OpCode opCode)
        {
            switch (opCode)
            {
                case OpCode.OP_CHECKSIG:
                case OpCode.OP_CHECKSIGVERIFY:
                case OpCode.OP_CHECKMULTISIG:
                case OpCode.OP_CHECKMULTISIGVERIFY:
                    return OpertationType.SigHash;
                case OpCode.OP_IF:
                case OpCode.OP_NOTIF:
                    return OpertationType.Commands;
                case OpCode.OP_TOALTSTACK:
                case OpCode.OP_FROMALTSTACK:
                    return OpertationType.AltStack;
                default:
                    return OpertationType.Stack;
            }
        }

        bool Push(Stack<byte[]> stack, BigInteger i)
        {
            return Push(stack, i.ToByteArray());
        }

        bool Push(Stack<byte[]> stack, byte[] bytes)
        {
            stack.Push(bytes);
            return true;
        }


    }
}
