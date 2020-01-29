using System;
using System.IO;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionReaderTests
    {
        [Theory]
        [InlineData(0, "00000000")]
        [InlineData(1, "01000000")]
        [InlineData(1, "010000009348934789534")]
        [InlineData(2, "02000000")]
        [InlineData(274, "12010000")]
        [InlineData(2054357487, "EF01737A")]
        public void ReadTransactionTests(int expected, string input)
        {
            Assert.Equal(expected, GetReader(input).ReadVersion());
        }

        [Theory]
        [InlineData("")]
        [InlineData("01")]
        [InlineData("000000")]
        public void ReadTransactionThrowsTests(string input)
        {
            Assert.Throws<EndOfStreamException>(() => GetReader(input).ReadVersion());
        }

        static TransactionReader GetReader(string input)
        {
            return new TransactionReader(new MemoryStream(Cipher.ToBytes(input)));
        }
    }
}
