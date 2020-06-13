using System;
using System.Collections;
using Murmur;

namespace BitcoinBook
{
    public class BloomFilter
    {
        const uint bip37Constant = 0xfba4c795;

        public int HashCount { get; }
        public uint Tweak { get; }
        public byte Flags { get; }

        readonly int bitCount;
        readonly BitArray bits;

        public BloomFilter(int size, int hashCount, uint tweak, byte flags = 0)
        {
            if (size < 1 || size > 36000) throw new ArgumentException("size must be less than 36,000", nameof(size));
            if (hashCount < 1 || hashCount > 50) throw new ArgumentException("size must be less than 50", nameof(hashCount));

            HashCount = hashCount;
            Tweak = tweak;
            Flags = flags;

            bitCount = size * 8;
            bits = new BitArray(bitCount, false);
        }

        public void Add(byte[] data)
        {
            for (var i = 0; i < HashCount; i++)
            {
                var seed = (uint)i * bip37Constant + Tweak;
                var hash = BitConverter.ToUInt32(MurmurHash.Create32(seed).ComputeHash(data));
                var bit = (int)(hash % bitCount);
                bits[bit] = true;
            }
        }

        public byte[] Result => bits.ToBytes();
    }
}
