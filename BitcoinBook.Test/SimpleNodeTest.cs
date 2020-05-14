using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Xunit;

namespace BitcoinBook.Test
{
    public class SimpleNodeTest
    {
        public static IEnumerable<object[]> HandshakeData => new[]
        {
            new object[] { Dns.GetHostEntry("testnet.programmingbitcoin.com").AddressList[0], true },
            new object[] { new IPAddress(new byte[] { 104, 62, 47, 181 }), false},
        };

        [Theory]
        [MemberData(nameof(HandshakeData))]
        public void HandshakeTest(IPAddress ipAddress, bool testnet)
        {
            using var node = new SimpleNode(ipAddress, testnet);
            node.Handshake();
            Assert.StartsWith("/Satoshi", node.RemoteUserAgent);
        }
    }
}
