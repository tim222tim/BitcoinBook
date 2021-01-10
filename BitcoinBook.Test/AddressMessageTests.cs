using System.Net;
using Xunit;

namespace BitcoinBook.Test
{
    public class AddressMessageTests
    {
        static readonly TimestampedNetworkAddress[] addresses = 
        {
            new(1, new IPAddress(new byte[] {10, 0, 0, 1}), 8333, 0x4D1015E2),
        };
        readonly AddressMessage message = new(addresses);
        const string messageHex = "01e215104d010000000000000000000000000000000000ffff0a000001208d";

        [Fact]
        public void ToBytesTest()
        {
            var actual = message.ToBytes().ToHex();
            Assert.Equal(messageHex, actual);
        }

        [Fact]
        public void ParseTest()
        {
            var newMessage = AddressMessage.Parse(Cipher.ToBytes(messageHex));
            Assert.Equal(message.Addresses.Count, newMessage.Addresses.Count);
            for (var i = 0; i < message.Addresses.Count; i++)
            {
                Assert.Equal(message.Addresses[i].Services, newMessage.Addresses[i].Services);
                Assert.Equal(message.Addresses[i].IPAddress, newMessage.Addresses[i].IPAddress);
                Assert.Equal(message.Addresses[i].Port, newMessage.Addresses[i].Port);
                Assert.Equal(message.Addresses[i].Timestamp, newMessage.Addresses[i].Timestamp);
            }
        }
    }
}
