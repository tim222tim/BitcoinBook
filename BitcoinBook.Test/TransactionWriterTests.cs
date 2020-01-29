using System;
using System.IO;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionWriterTests
    {
        [Theory]
        [InlineData("01", 1UL, 1)]
        [InlineData("01000000", 1UL, 4)]
        [InlineData("02000000", 2, 4)]
        [InlineData("120100", 274, 3)]
        [InlineData("EF01737A00000000", 2054357487, 8)]
        public void WriteIntTest(string expected, ulong i, int count)
        {
            Assert.Equal(Cipher.ToBytes(expected), GetResult(w => w.WriteInt(i, count)));
        }

        [Theory]
        [InlineData("00", 0)]
        [InlineData("01", 1)]
        [InlineData("FC", 252)]
        [InlineData("FDFF", 255)]
        [InlineData("FD0101", 257)]
        [InlineData("FEEF01737A", 2054357487)]
        [InlineData("FF0102030405060708", 578437695752307201)]
        public void WriteVarIntTests(string expected, ulong i)
        {
            Assert.Equal(Cipher.ToBytes(expected), GetResult(w => w.WriteVarInt(i)));
        }

        static byte[] GetResult(Action<TransactionWriter> action)
        {
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            action(writer);
            var array = stream.ToArray();
            return array;
        }
    }
}
