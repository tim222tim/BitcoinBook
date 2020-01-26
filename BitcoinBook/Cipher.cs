using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BitcoinBook
{
    public static class Cipher
    {
        const string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        static readonly int[] values = InitValues();

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
                result = result * 58 + value;
            }

            return result.ToByteArray();
        }

        public static BigInteger ComputeHash256(byte[] data)
        {
            using var sha256Hash = SHA256.Create();
            return new BigInteger(SuffixZeros(sha256Hash.ComputeHash(data), 4));
        }

        public static BigInteger ComputeHash256(string data)
        {
            return ComputeHash256(Encoding.UTF8.GetBytes(data));
        }

        static byte[] PrefixZeros(byte[] bytes, int count)
        {
            var newBytes = new byte[bytes.Length + count];
            for (var i = 0; i < count; i++)
            {
                newBytes[i] = 0;
            }
            bytes.CopyTo(newBytes, 4);
            return newBytes;
        }

        static byte[] SuffixZeros(byte[] bytes, int count)
        {
            var newBytes = new byte[bytes.Length + count];
            bytes.CopyTo(newBytes, 0);
            for (var i = 0; i < count; i++)
            {
                newBytes[bytes.Length + i] = 0;
            }
            return newBytes;
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

        static int[] InitValues()
        {
            var array = new int[256];
            Array.Fill(array, -1);
            for (var i = 0; i < alphabet.Length; i++)
            {
                array[alphabet[i]] = i;
            }

            return array;
        }
    }
}
