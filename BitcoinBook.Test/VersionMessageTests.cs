using System.Net;
using Xunit;

namespace BitcoinBook.Test
{
    public class VersionMessageTests
    {
        readonly VersionMessage message = new VersionMessage(VersionMessage.DefaultVersion, 0, 0x5b8317ad,
            0, new IPAddress(new byte[] {0, 0, 0, 0}), 8333,
            0, new IPAddress(new byte[] {0, 0, 0, 0}), 8333,
            0xa127ec40a4d7a8f6, "/rossitertest:0.1/", 0, true);

        const string messageHex = "7f1101000000000000000000ad17835b00000000000000000000000000000000000000000000ffff000000008d20000000000000000000000000000000000000ffff000000008d20f6a8d7a440ec27a1122f726f737369746572746573743a302e312f0000000001";

        [Fact]
        public void ToBytesTest()
        {
            Assert.Equal(messageHex, message.ToBytes().ToHex());
        }

        [Fact]
        public void ParseTest()
        {
            var newMessage = VersionMessage.Parse(Cipher.ToBytes(messageHex));
            Assert.Equal(message.Version, newMessage.Version);
            Assert.Equal(message.Services, newMessage.Services);
            Assert.Equal(message.Timestamp, newMessage.Timestamp);
            Assert.Equal(message.ReceiverServices, newMessage.ReceiverServices);
            Assert.Equal(message.ReceiverAddress, newMessage.ReceiverAddress);
            Assert.Equal(message.ReceiverPort, newMessage.ReceiverPort);
            Assert.Equal(message.SenderServices, newMessage.SenderServices);
            Assert.Equal(message.SenderAddress, newMessage.SenderAddress);
            Assert.Equal(message.SenderPort, newMessage.SenderPort);
            Assert.Equal(message.Nonce, newMessage.Nonce);
            Assert.Equal(message.UserAgent, newMessage.UserAgent);
            Assert.Equal(message.Height, newMessage.Height);
            Assert.Equal(message.RelayFlag, newMessage.RelayFlag);
        }
    }
}
