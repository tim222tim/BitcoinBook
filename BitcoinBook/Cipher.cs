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
        static readonly int[] values = InitReverseAlphabet();

        static int[] InitReverseAlphabet()
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
            var number = ToBigInteger(bytes);
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
            return ToBase58(Concat(bytes, ComputeHash256Prefix(bytes)));
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

            return ToBytes(result);
        }

        public static byte[] ComputeHash256(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(sha256.ComputeHash(data));
        }

        public static BigInteger ComputeHash256Int(byte[] data)
        {
            return ToBigInteger(ComputeHash256(data));
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

        public static byte[] ComputeHash160(byte[] data)
        {
            using var sha256 = SHA256.Create();
            using var rip160 = new RIPEMD160();
            return rip160.ComputeHash(sha256.ComputeHash(data));
        }

        public static byte[] ComputeHash160(string data)
        {
            return ComputeHash160(Encoding.UTF8.GetBytes(data));
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

        public static byte[] ToBytes(BigInteger i, int minLength = 0)
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

        public static string ToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static string ToHex(BigInteger i)
        {
            return ToHex(ToBytes(i));
        }

        public static byte[] ToBytes32(BigInteger i)
        {
            return ToBytes(i, 32);
        }

        public static string ToHex32(BigInteger i)
        {
            return ToHex(ToBytes32(i));
        }

        public static BigInteger ToBigInteger(byte[] data)
        {
            var i = new BigInteger();
            foreach (var b in data)
            {
                i = i * 256 + b;
            }

            return i;
        }

        static byte[] Concat(byte[] b1, byte[] b2)
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
