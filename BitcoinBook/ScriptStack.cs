using System.Collections.Generic;
using System.Numerics;

namespace BitcoinBook
{
    public class ScriptStack : Stack<byte[]>
    {
        public void Push(BigInteger i)
        {
            Push(i == 0 ? new byte[0] : i.ToByteArray());
        }

        public void Push(bool b)
        {
            Push(b ? 1 : 0);
        }

        public BigInteger PopInt()
        {
            return new BigInteger(Pop());
        }
    }
}
