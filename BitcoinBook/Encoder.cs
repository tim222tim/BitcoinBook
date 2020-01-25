using System;
using System.Numerics;
using System.Text;

namespace BitcoinBook
{
    public static class Encoder
    {
        const string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        static readonly int[] values = new int[256];

        static Encoder()
        {
            Array.Fill(values, -1);
            for (var i = 0; i < alphabet.Length; i++)
            {
                values[alphabet[i]] = i;
            }
        }

        public static string ToBase58(byte[] bytes)
        {
            var builder = new StringBuilder();
            var number = new BigInteger(bytes);
            while (number > 0)
            {
                number = BigInteger.DivRem(number, 58, out var remainder);
                builder.Insert(0, alphabet[(int)remainder]);
            }

            builder.Insert(0, new string('1', CountZeros(bytes)));
            return builder.ToString();
        }

        public static byte[] FromBase58(string base58)
        {
            if (string.IsNullOrEmpty(base58))
            {
                base58 = " ";
            }
            BigInteger result = 0;
            foreach (var c in base58)
            {
                var value = values[c];
                if (value == -1)
                {
                    throw new FormatException("Invalid Base58 format");
                }
                result *= 58;
                result += value;
            }

            return result.ToByteArray();
        }

        static int CountZeros(byte[] bytes)
        {
            var index = 0;
            while (index < bytes.Length && bytes[index] == 0)
            {
                ++index;
            }

            return index;
        }
    }
}
