using System;
using System.Numerics;

namespace BitcoinBook
{
    public class RandomBigInteger : Random
    {
        public RandomBigInteger()
        {
        }

        public RandomBigInteger(int seed) : base(seed)
        {
        }

        public BigInteger NextBigInteger(BigInteger n)
        {
            BigInteger result;
            do
            {
                int length = (int)Math.Ceiling(BigInteger.Log(n, 2));
                int numBytes = (int)Math.Ceiling(length / 8.0);
                byte[] data = new byte[numBytes];
                NextBytes(data);
                result = new BigInteger(data);
            } while (result >= n || result <= 0);
            return result;
        }
    }
}
