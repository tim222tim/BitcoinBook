using System;

namespace BitcoinBook
{
    public static class ByteArrayExtensions
    {
        public static byte[] Concat(this byte[] b1, params byte[] b2)
        {
            var newBytes = new byte[b1.Length + b2.Length];
            b1.CopyTo(newBytes, 0);
            b2.CopyTo(newBytes, b1.Length);
            return newBytes;
        }


        public static string ToHex(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
