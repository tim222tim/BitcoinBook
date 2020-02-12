﻿using System;
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
            var number = bytes.ToBigInteger();
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
            return ToBase58(bytes.Concat(Hash256Prefix(bytes)));
        }

        static byte[] Hash256Prefix(byte[] bytes)
        {
            return Hash256(bytes).Copy(0, 4);
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

            return result.ToBigBytes();
        }

        public static byte[] Hash256(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(sha256.ComputeHash(data));
        }

        public static BigInteger Hash256Int(byte[] data)
        {
            return Hash256(data).ToBigInteger();
        }

        public static BigInteger Hash256Int(string data)
        {
            return Hash256Int(Encoding.UTF8.GetBytes(data));
        }

        public static string Hash256String(byte[] data)
        {
            var hash = Hash256(data);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static string Hash256String(string data)
        {
            return Hash256String(Encoding.UTF8.GetBytes(data));
        }

        public static byte[] Hash160(byte[] data)
        {
            using var sha256 = SHA256.Create();
            using var rip160 = new RIPEMD160();
            return rip160.ComputeHash(sha256.ComputeHash(data));
        }

        public static byte[] Hash160(string data)
        {
            return Hash160(Encoding.UTF8.GetBytes(data));
        }

        public static string Hash160String(byte[] data)
        {
            var hash = Hash160(data);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static string Hash160String(string data)
        {
            return Hash160String(Encoding.UTF8.GetBytes(data));
        }

        public static byte[] ToBytesSigned(BigInteger i)
        {
            var bytes = i.ToByteArray();
            Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] ToBytes32(BigInteger i)
        {
            return i.ToBigBytes(32);
        }

        public static byte[] ToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new FormatException("Hex string must have an even number of bytes");
            }
            byte[] bytes = new byte[hex.Length / 2];
            for (var i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string ToHex(BigInteger i)
        {
            return i.ToBigBytes().ToHex();
        }

        public static string ToHex32(BigInteger i)
        {
            return ToBytes32(i).ToHex();
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

        public static byte[] Sha1(byte[] data)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(data);
        }
    }
}
