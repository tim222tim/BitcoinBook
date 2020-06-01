using System.Collections.Generic;
using System.Net;
using Xunit;

namespace BitcoinBook.Test
{
    public class SimpleNodeTest
    {
        static readonly IPAddress homeIpAddress = new IPAddress(new byte[] { 104, 62, 47, 181 });

        public static IEnumerable<object[]> HandshakeData => new[]
        {
            new object[] { Dns.GetHostEntry("testnet.programmingbitcoin.com").AddressList[0], true },
            new object[] { homeIpAddress, false},
        };

        [Theory]
        [MemberData(nameof(HandshakeData))]
        public void HandshakeTest(IPAddress ipAddress, bool testnet)
        {
            using var node = new SimpleNode(ipAddress, testnet);
            node.Handshake();
            Assert.StartsWith("/Satoshi", node.RemoteUserAgent);
        }

        [Fact]
        public void WaitForVerakTest()
        {
            using var node = new SimpleNode(homeIpAddress);
            node.Send(new VersionMessage());
            var message = node.WaitFor<VerAckMessage>();
            Assert.NotNull(message);
        }

        [Fact]
        public void GetHeadersTest()
        {
            using var node = new SimpleNode(homeIpAddress);
            node.Handshake();
            node.Send(new GetHeadersMessage(BlockHeader.Genesis.Id));
            var message = node.WaitFor<HeadersMessage>();
            Assert.NotNull(message);
            Assert.Equal(2000, message.BlockHeaders.Count);
        }
    }
}
