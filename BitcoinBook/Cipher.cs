using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using DevHawk.Security.Cryptography;

namespace BitcoinBook
{
    public static class Cipher
    {
        const string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        static readonly int[] values = InitValues();

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

        public static string ToBase58Check(byte[] bytes)
        {
            return ToBase58(Add(bytes, ComputeHash256Prefix(bytes)));
        }

        static byte[] ComputeHash256Prefix(byte[] bytes)
        {
            var hash = ComputeHash256(bytes);
            var hashPrefix = new byte[4];
            Array.Copy(hash, hashPrefix, 4);
            return hashPrefix;
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

        public static byte[] ComputeHash160(byte[] data)
        {
            using var algorithm = new RIPEMD160();
            return algorithm.ComputeHash(data);
        }

        public static byte[] ComputeHash160(string data)
        {
            return ComputeHash160(Encoding.UTF8.GetBytes(data));
        }

        public static byte[] ComputeHash256(byte[] data)
        {
            using var algorithm = SHA256.Create();
            return algorithm.ComputeHash(data);
        }

        public static string ComputeHash160String(byte[] data)
        {
            var hash = ComputeHash160(data);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static string ComputeHash160String(string data)
        {
            return ComputeHash160String(Encoding.UTF8.GetBytes(data));
        }

        public static BigInteger ComputeHash256Int(byte[] data)
        {
            var hash = ComputeHash256(data);
            Array.Reverse(hash);
            return new BigInteger(Add(hash, new byte[] {0, 0}));
        }

        public static BigInteger ComputeHash256Int(string data)
        {
            return ComputeHash256Int(Encoding.UTF8.GetBytes(data));
        }

        public static string ComputeHash256String(byte[] data)
        {
            var hash = ComputeHash256(data);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static string ComputeHash256String(string data)
        {
            return ComputeHash256String(Encoding.UTF8.GetBytes(data));
        }

        static byte[] Add(byte[] b1, byte[] b2)
        {
            var newBytes = new byte[b1.Length + b2.Length];
            b1.CopyTo(newBytes, 0);
            b2.CopyTo(newBytes, b1.Length);
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
    }
}
