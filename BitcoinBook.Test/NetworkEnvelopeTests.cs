using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class NetworkEnvelopeTests
    {
        const string noPayloadHex = "f9beb4d976657261636b000000000000000000005df6e0e2";

        [Fact]
        public void ToStringTest()
        {
            Assert.Equal("ping: 0102030405060708", new NetworkEnvelope("ping", new byte[] {1, 2, 3, 4, 5, 6, 7, 8}).ToString());
        }

        [Fact]
        public void ParseNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => NetworkEnvelope.Parse((byte[])null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData("00")]
        [InlineData("01020304")]
        [InlineData("f9beb4d976657261636b000000000000000000005df6e0e1")]
        public void ParseBadFormatTest(string hex)
        {
            Assert.Throws<FormatException>(() => NetworkEnvelope.Parse(Cipher.ToBytes(hex)));
        }

        [Fact]
        public void ParseTest()
        {
            var envelope = NetworkEnvelope.Parse(Cipher.ToBytes(noPayloadHex));
            Assert.Equal("verack", envelope.Command);
            Assert.Empty(envelope.Payload);
            Assert.False(envelope.Testnet);
        }

        [Fact]
        public void ToBytesTest()
        {
            var envelope = NetworkEnvelope.Parse(Cipher.ToBytes(noPayloadHex));
            Assert.Equal(noPayloadHex, envelope.ToBytes().ToHex());
        }

        [Fact]
        public void ToBytesWithPayloadTest()
        {
            var envelope = new NetworkEnvelope("ping", new byte[] {1, 2, 3, 4, 5, 6, 7, 8});
            var envelope2 = NetworkEnvelope.Parse(envelope.ToBytes());
            Assert.Equal(envelope.Command, envelope2.Command);
            Assert.Equal(envelope.Payload, envelope2.Payload);
        }
    }
}
