using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace BitcoinBook
{
    public class ScriptEvaluator
    {
        public bool Evaluate(IEnumerable<object> scriptCommands, byte[] sigHash)
        {
            if (scriptCommands == null) throw new ArgumentNullException(nameof(scriptCommands));
            var commands = new Stack<object>(scriptCommands.Reverse());
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
                    return Push(stack, 0);
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
                case OpCode.OP_DUP:
                    return Push(stack, stack.Peek());
                case OpCode.OP_EQUALVERIFY:
                    return stack.Pop().SequenceEqual(stack.Pop());
                case OpCode.OP_HASH160:
                    return Push(stack, Cipher.Hash160(stack.Pop()));
                case OpCode.OP_ADD:
                    return Push(stack, new BigInteger(stack.Pop()) + new BigInteger(stack.Pop()));
                case OpCode.OP_MUL:
                    return Push(stack, new BigInteger(stack.Pop()) * new BigInteger(stack.Pop()));
                case OpCode.OP_EQUAL:
                    return Push(stack, new BigInteger(stack.Pop()).Equals(new BigInteger(stack.Pop())) ? 1 : 0);
                default:
                    throw new InvalidOperationException("Unknown operation: " + opCode);
            }
        }

        bool Evaluate(OpCode opCode, Stack<byte[]> stack, byte[] sigHash)
        {
            switch (opCode)
            {
                case OpCode.OP_CHECKSIG:
                    var publicKey = PublicKey.FromSec(stack.Pop());
                    var signature = Signature.FromDer(stack.Pop());
                    var result = publicKey.Verify(sigHash, signature);
                    Push(stack, result);
                    return true;
                default:
                    throw new NotImplementedException();
            }
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

        bool Push(Stack<byte[]> stack, bool b)
        {
            return Push(stack, b ? 1 : 0);
        }

        bool Push(Stack<byte[]> stack, BigInteger i)
        {
            return Push(stack, i == 0 ? new byte[0] : i.ToByteArray());
        }

        bool Push(Stack<byte[]> stack, byte[] bytes)
        {
            stack.Push(bytes);
            return true;
        }
    }
}
