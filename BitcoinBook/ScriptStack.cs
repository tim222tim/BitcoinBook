using System.Collections.Generic;
using System.Numerics;

namespace BitcoinBook
{
    public class ScriptStack
    {
        readonly Stack<byte[]> stack = new Stack<byte[]>();

        public int Count => stack.Count;

        public bool Push(byte[] bytes)
        {
            stack.Push(bytes);
            return true;
        }

        public bool Push(BigInteger i)
        {
            return Push(i == 0 ? new byte[0] : i.ToLittleBytes());
        }

        public bool Push(bool b)
        {
            return Push(b ? 1 : 0);
        }

        public byte[] Pop()
        {
            return stack.Pop();
        }

        public BigInteger PopInt()
        {
            return new BigInteger(Pop());
        }

        public byte[] Peek()
        {
            return stack.Peek();
        }

        public BigInteger PeekInt()
        {
            return new BigInteger(Peek());
        }
    }
}
