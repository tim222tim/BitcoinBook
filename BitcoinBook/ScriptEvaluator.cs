using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class ScriptEvaluator
    {
        public bool Evaluate(IEnumerable<object> scriptCommands, byte[] sigHash = null)
        {
            if (scriptCommands == null) throw new ArgumentNullException(nameof(scriptCommands));
            var commands = new Stack<object>(scriptCommands.Reverse());
            var stack = new ScriptStack();
            var altStack = new ScriptStack();

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
                    try
                    {
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
                    }
                    catch (FormatException)
                    {
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    if (!result)
                    {
                        return false;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid command type");
                }
            }

            return stack.Count > 0 && stack.Pop().Length > 0;
        }

        bool Evaluate(OpCode opCode, ScriptStack stack)
        {
            switch (opCode)
            {
                case OpCode.OP_0:
                    return stack.Push(0);
                case OpCode.OP_1NEGATE:
                    return stack.Push(-1);
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
                    return stack.Push((int)opCode - (int)OpCode.OP_1 + 1);
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
                    return stack.Push(stack.Peek());
                case OpCode.OP_2DUP:
                    var d1 = stack.Pop();
                    var d2 = stack.Peek();
                    stack.Push(d1);
                    stack.Push(d2);
                    return stack.Push(d1);
                case OpCode.OP_SWAP:
                    var s1 = stack.Pop();
                    var s2 = stack.Pop();
                    stack.Push(s1);
                    return stack.Push(s2);
                case OpCode.OP_VERIFY:
                    return stack.Pop().Length > 0;
                case OpCode.OP_EQUALVERIFY:
                    return stack.Pop().SequenceEqual(stack.Pop());
                case OpCode.OP_HASH160:
                    return stack.Push(Cipher.Hash160(stack.Pop()));
                case OpCode.OP_SHA1:
                    return stack.Push(Cipher.Sha1(stack.Pop()));
                case OpCode.OP_ADD:
                    return stack.Push(stack.PopInt() + stack.PopInt());
                case OpCode.OP_MUL:
                    return stack.Push(stack.PopInt() * stack.PopInt());
                case OpCode.OP_EQUAL:
                    return stack.Push(stack.Pop().SequenceEqual(stack.Pop()) ? 1 : 0);
                case OpCode.OP_NOT:
                    return stack.Push(stack.Pop().Length == 0 ? 1 : 0);
                default:
                    throw new InvalidOperationException("Unknown operation: " + opCode);
            }
        }

        bool Evaluate(OpCode opCode, ScriptStack stack, byte[] sigHash)
        {
            switch (opCode)
            {
                case OpCode.OP_CHECKSIG:
                    var publicSec = stack.Pop();
                    var publicKey = PublicKey.FromSec(publicSec);
                    var sigDerWithHashType = stack.Pop();
                    // last byte is hash type!
                    var sigDer = sigDerWithHashType.Copy(0, sigDerWithHashType.Length - 1);
                    var signature = Signature.FromDer(sigDer);
                    var result = publicKey.Verify(sigHash, signature);
                    return stack.Push(result);
                default:
                    throw new NotImplementedException();
            }
        }

        bool Evaluate(OpCode opCode, ScriptStack stack, Stack<object> commands)
        {
            throw new NotImplementedException();
        }

        bool Evaluate(OpCode opCode, ScriptStack stack, ScriptStack altStack)
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
    }
}
