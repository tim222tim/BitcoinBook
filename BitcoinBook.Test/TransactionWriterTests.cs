using System.IO;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionWriterTests
    {
        [Theory]
        [InlineData("01000000", 1UL, 4)]
        public void WriteIntTest(string expected, ulong i, int count)
        {
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.WriteInt(i, count);
            var array = stream.ToArray();
            Assert.Equal(Cipher.ToBytes(expected), array);
        }
    }
}
