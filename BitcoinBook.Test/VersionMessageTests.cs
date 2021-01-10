using System.Net;
using Xunit;

namespace BitcoinBook.Test
{
    public class VersionMessageTests
    {
        readonly VersionMessage message = new(VersionMessage.DefaultVersion, 0, 0x5b8317ad,
            new NetworkAddress(0, new IPAddress(new byte[] {0, 0, 0, 0}), 8333),
            new NetworkAddress(0, new IPAddress(new byte[] {0, 0, 0, 0}), 8333),
            0xa127ec40a4d7a8f6, "/rossitertest:0.1/", 0, true);

        const string messageHex = "7f1101000000000000000000ad17835b00000000000000000000000000000000000000000000ffff00000000208d000000000000000000000000000000000000ffff00000000208df6a8d7a440ec27a1122f726f737369746572746573743a302e312f0000000001";

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
            Assert.Equal(message.ServiceFlags, newMessage.ServiceFlags);
            Assert.Equal(message.Timestamp, newMessage.Timestamp);
            Assert.Equal(message.ReceiverAddress.Services, newMessage.ReceiverAddress.Services);
            Assert.Equal(message.ReceiverAddress.IPAddress, newMessage.ReceiverAddress.IPAddress);
            Assert.Equal(message.ReceiverAddress.Port, newMessage.ReceiverAddress.Port);
            Assert.Equal(message.SenderAddress.Services, newMessage.SenderAddress.Services);
            Assert.Equal(message.SenderAddress.IPAddress, newMessage.SenderAddress.IPAddress);
            Assert.Equal(message.SenderAddress.Port, newMessage.SenderAddress.Port);
            Assert.Equal(message.Nonce, newMessage.Nonce);
            Assert.Equal(message.UserAgent, newMessage.UserAgent);
            Assert.Equal(message.Height, newMessage.Height);
            Assert.Equal(message.RelayFlag, newMessage.RelayFlag);
        }
    }
}
