using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class NetworkEnvelopeTests
    {
        [Fact]
        public void ToStringTest()
        {
            Assert.Equal("version: 010210", new NetworkEnvelope("version", new byte[] {1, 2, 16}, false).ToString());
        }

        [Fact]
        public void ParseNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => NetworkEnvelope.Parse(null, false));
        }

        [Theory]
        [InlineData("")]
        [InlineData("00")]
        [InlineData("01020304")]
        [InlineData("f9beb4d976657261636b000000000000000000005df6e0e1")]
        public void ParseBadFormatTest(string hex)
        {
            Assert.Throws<FormatException>(() => NetworkEnvelope.Parse(Cipher.ToBytes(hex), false));
        }

        [Fact]
        public void ParseTest()
        {
            var envelope = NetworkEnvelope.Parse(Cipher.ToBytes("f9beb4d976657261636b000000000000000000005df6e0e2"), false);
            Assert.Equal("verack", envelope.Command);
            Assert.Empty(envelope.Payload);
            Assert.False(envelope.Testnet);
        }
    }
}
