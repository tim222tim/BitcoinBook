using System;
using System.Numerics;

namespace BitcoinBook
{
    public static class BigIntegerExtensions
    {
        public static byte[] ToBigBytes(this BigInteger i, int minLength = 0)
        {
            var rawBytes = i.ToByteArray();
            var rawLength = rawBytes.Length;
            if (rawBytes[rawLength - 1] == 0)
            {
                --rawLength;
            }

            var bytes = new byte[Math.Max(rawLength, minLength)];
            var bx = bytes.Length - 1;
            var rx = 0;
            while (rx < rawLength)
            {
                bytes[bx--] = rawBytes[rx++];
            }
            while (bx >= 0)
            {
                bytes[bx--] = 0;
            }

            return bytes;
        }

        public static byte[] ToSignedBigBytes(this BigInteger i)
        {
            var bytes = i.ToByteArray();
            Array.Reverse(bytes);
            return bytes;
        }
    }
}
