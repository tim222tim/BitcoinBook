using System;
using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
using Murmur;

namespace BitcoinBook
{
    public class BloomFilter
    {
        const uint bip37Constant = 0xfba4c795;

        readonly int size;
        readonly int hashCount;
        readonly uint tweak;
        readonly byte flags;

        readonly int bitCount;
        readonly BitArray bits;

        public BloomFilter(int size, int hashCount, uint tweak, byte flags = 0)
        {
            if (size < 1 || size > 36000) throw new ArgumentException("size must be less than 36,000", nameof(size));
            if (hashCount < 1 || hashCount > 50) throw new ArgumentException("size must be less than 50", nameof(hashCount));

            this.size = size;

            this.hashCount = hashCount;
            this.tweak = tweak;
            this.flags = flags;

            bitCount = size * 8;
            bits = new BitArray(bitCount, false);
        }

        public void Add(byte[] data)
        {
            for (var i = 0; i < hashCount; i++)
            {
                var murmur3 = MurmurHash.Create128((uint)i * bip37Constant + tweak);
                var hash = murmur3.ComputeHash(data).ToBigInteger();
                var bit = (int)(hash % bitCount);
                bits[bit] = true;
            }
        }

        public byte[] Result
        {
            get
            {
                var bytes = new byte[size];
                bits.CopyTo(bytes, 0);
                return bytes;
            }
        }
    }
}
