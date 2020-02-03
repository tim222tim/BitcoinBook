using System;
using System.Collections.Generic;
using System.Numerics;

namespace BitcoinBook
{
    public class OperationSelector
    {
        public Func<bool> GetOperation(OpCode opCode, Stack<byte[]> stack, Stack<byte[]> altStack, Stack<object> commands, byte[] sigHash)
        {
            switch (opCode)
            {
                case OpCode.OP_0:
                    return () => TrueOp(() => stack.Push(new byte[0]));
                case OpCode.OP_1NEGATE:
                    return () => TrueOp(() => stack.Push(new BigInteger(-1).ToByteArray()));
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
                    return () => TrueOp(() => stack.Push(new BigInteger((int)opCode - (int)OpCode.OP_1 + 1).ToByteArray()));
                case OpCode.OP_NOP:
                case OpCode.OP_NOP1:
                case OpCode.OP_NOP4:
                case OpCode.OP_NOP5:
                case OpCode.OP_NOP6:
                case OpCode.OP_NOP7:
                case OpCode.OP_NOP8:
                case OpCode.OP_NOP9:
                case OpCode.OP_NOP10:
                    return () => true;
            }
            throw new InvalidOperationException("Unknown opcode");
        }

        bool TrueOp(Action action)
        {
            action();
            return true;
        }
    }
}
