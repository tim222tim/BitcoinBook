using System;
using System.Numerics;
using System.Security.Cryptography;

namespace BitcoinBook
{
    public static class ByteArrayExtensions
    {
        public static byte[] Copy(this byte[] bytes, int index = 0, int length = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (index < 0)
            {
                index += bytes.Length;
            }
            if (length <= 0)
            {
                length = bytes.Length - index + length;
            }
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

        public static byte[] Reverse(this byte[] bytes)
        {
            var newBytes = bytes.Copy();
            Array.Reverse(newBytes);
            return newBytes;
        }

        public static string ToHex(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static string ToReverseHex(this byte[] bytes)
        {
            return bytes.Reverse().ToHex();
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
