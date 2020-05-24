using System;
using System.Numerics;

namespace BitcoinBook
{
    public static class BlockMath
    {
        const int twoWeeks = 60 * 60* 24 * 14;

        public static BigInteger ComputeNewTarget(BlockHeader firstBlockHeader, BlockHeader lastBlockHeader)
        {
            var timeDifferential = (int)lastBlockHeader.Timestamp - (int)firstBlockHeader.Timestamp;
            timeDifferential = Math.Min(timeDifferential, twoWeeks * 4);
            timeDifferential = Math.Max(timeDifferential, twoWeeks / 4);
            return lastBlockHeader.Target * timeDifferential / twoWeeks;
        }

        public static uint TargetToBits(BigInteger target)
        {
            var bytes = target.ToBigBytes();
            var highBit = bytes[0] > 0x7f;
            var exponent = (byte)(highBit ? bytes.Length + 1 : bytes.Length);
            var coefficient = highBit ? new byte[] {0x00, bytes[0], bytes[1]} : bytes.Copy(0, 3);
            return BitConverter.ToUInt32(coefficient.Reverse().Concat(exponent));
        }
    }
}
