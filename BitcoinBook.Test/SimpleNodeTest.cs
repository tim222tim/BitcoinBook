using System;
using System.Collections.Generic;
using Xunit;

namespace BitcoinBook.Test
{
    public class SimpleNodeTest : IDisposable
    {
        readonly SimpleNode timNode = new SimpleNode(IntegrationSetup.TimNode.Address);

        public static IEnumerable<object[]> NodeData => new[]
        {
            // new object[] {IntegrationSetup.BookNode},
            new object[] {IntegrationSetup.TimNode},
        };

        public void Dispose()
        {
            timNode?.Dispose();
        }

        [Theory]
        [MemberData(nameof(NodeData))]
        public void HandshakeTest(NodeSetting setting)
        {
            using var node = new SimpleNode(setting.Address);
            node.Handshake();
            Assert.StartsWith("/Satoshi", node.RemoteUserAgent);
        }

        [Fact]
        public void WaitForVerakTest()
        {
            timNode.Send(new VersionMessage());
            var message = timNode.WaitFor<VerAckMessage>();
            Assert.NotNull(message);
        }

        [Fact]
        public void GetHeadersTest()
        {
            timNode.Handshake();
            var previousId = BlockHeader.Genesis.Id;
            timNode.Send(new GetHeadersMessage(previousId));
            var message = timNode.WaitFor<HeadersMessage>();
            Assert.NotNull(message);
            Assert.Equal(2000, message.BlockHeaders.Count);
            Assert.Equal(previousId, message.BlockHeaders[0].PreviousBlock);
        }
    }
}
