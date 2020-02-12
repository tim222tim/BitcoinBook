using System;
using System.Numerics;

namespace BitcoinBook
{
    public static class ByteArrayExtensions
    {
        public static byte[] Copy(this byte[] bytes, int index, int length)
        {
            var newBytes = new byte[length];
            Array.Copy(bytes, index, newBytes, 0, length);
            return newBytes;
        }

        public static byte[] Concat(this byte[] bytes, params byte[] bytes2)
        {
            var newBytes = new byte[bytes.Length + bytes2.Length];
            bytes.CopyTo(newBytes, 0);
            bytes2.CopyTo(newBytes, bytes.Length);
            return newBytes;
        }

        public static string ToHex(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static BigInteger ToBigInteger(this byte[] bytes)
        {
            var i = new BigInteger();
            foreach (var b in bytes)
            {
                i *= 256;
                i += b;
            }

            return i;
        }
    }
}
