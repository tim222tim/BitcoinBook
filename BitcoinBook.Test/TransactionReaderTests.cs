using System.IO;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionReaderTests
    {
        [Theory]
        [InlineData(0, "00000000")]
        [InlineData(1, "01000000")]
        [InlineData(1, "01000000934893478953F4")]
        [InlineData(2, "02000000")]
        [InlineData(274, "12010000")]
        [InlineData(2054357487, "EF01737A")]
        public void ReadVersionTests(uint expected, string input)
        {
            Assert.Equal(expected, GetReader(input).ReadVersion());
        }

        [Theory]
        [InlineData("")]
        [InlineData("01")]
        [InlineData("000000")]
        public void ReadVersionThrowsTests(string input)
        {
            Assert.Throws<EndOfStreamException>(() => GetReader(input).ReadVersion());
        }

        [Theory]
        [InlineData(0, "00")]
        [InlineData(1, "0134")]
        [InlineData(252, "FC75")]
        [InlineData(255, "FDFF00")]
        [InlineData(257, "FD010166")]
        [InlineData(2054357487, "FEEF01737A7464")]
        [InlineData(578437695752307201, "FF0102030405060708DDEE")]
        public void ReadVarIntTests(ulong expected, string input)
        {
            Assert.Equal(expected, GetReader(input).ReadVarInt());
        }

        static TransactionReader GetReader(string input)
        {
            return new TransactionReader(new MemoryStream(Cipher.ToBytes(input)));
        }
    }
}
