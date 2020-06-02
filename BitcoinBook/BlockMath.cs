using System;
using System.Numerics;

namespace BitcoinBook
{
    public static class BlockMath
    {
        const int twoWeeks = 60 * 60* 24 * 14;
        public static readonly BigInteger MaxTarget = 0xffff * BigInteger.Pow(256, 0x1d - 3);

        public static BigInteger BitsToTarget(uint bits)
        {
            return (bits & 0x00ffffff) * BigInteger.Pow(256, (int)(bits >> 0x18) - 3);
        }

        public static double TargetToDifficulty(BigInteger target)
        {
            return 0xffff * Math.Pow(256, 0x1d - 3) / (double)target;
        }

        public static BigInteger ComputeNewTarget(BlockHeader firstBlockHeader, BlockHeader lastBlockHeader)
        {
            return ComputeNewTarget(firstBlockHeader.Timestamp, lastBlockHeader.Timestamp, firstBlockHeader.Target);
        }

        public static BigInteger ComputeNewTarget(uint firstTimestamp, uint lastTimestamp, BigInteger previousTarget)
        {
            var timeDifferential = (int) lastTimestamp - (int) firstTimestamp;
            timeDifferential = Math.Min(timeDifferential, twoWeeks * 4);
            timeDifferential = Math.Max(timeDifferential, twoWeeks / 4);
            var newTarget = previousTarget * timeDifferential / twoWeeks;
            return BigInteger.Min(newTarget, MaxTarget);
        }

        public static uint ComputeNewBits(uint firstTimestamp, uint lastTimestamp, uint previousBits)
        {
            return TargetToBits(ComputeNewTarget(firstTimestamp, lastTimestamp, BitsToTarget(previousBits)));
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
