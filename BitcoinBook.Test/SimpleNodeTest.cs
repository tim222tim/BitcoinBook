using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [Fact]
        public void WaitForVerakTest()
        {
            timNode.Send(new VersionMessage());
            var message = timNode.WaitFor<VerAckMessage>();
            Assert.NotNull(message);
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
        public void CompactHeadersTest()
        {
            timNode.Handshake();
            ProcessAllMessages(timNode);
            Assert.False(timNode.CompactHeaderFlags[1]);
            Assert.False(timNode.CompactHeaderFlags[2]);
        }

        void ProcessAllMessages(SimpleNode node)
        {
            while (true)
            {
                try
                {
                    node.WaitForMessage();
                }
                catch (IOException)
                {
                    break;
                }
            }
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

        // [Fact]
        // // bloom filters are removed
        // public void GetDataTest()
        // {
        //     timNode.Handshake();
        //     var filter = new BloomFilter(30, 5, 90210);
        //     filter.Add(Address.HashFromAddress("1CK6KHY6MHgYvmRQ4PAafKYDrg1ejbH1cE"));
        //     timNode.Send(new FilterLoadMessage(filter));
        //     var previousId = "0000000000000000001475235ddf33e99c1b560d20179392e3592546ca654552";
        //     timNode.Send(new GetHeadersMessage(previousId));
        //     var headersMessage = timNode.WaitFor<HeadersMessage>();
        //     Assert.NotNull(headersMessage);
        //     var blockDataItems = headersMessage.BlockHeaders.Select(h => new BlockDataItem(BlockDataType.MerkleBlock, Cipher.ToBytes(h.Id)));
        //     timNode.Send(new GetDataMessage(blockDataItems));
        //     while (true)
        //     {
        //         var message = timNode.WaitForMessage();
        //     }
        // }
        //
        // [Fact]
        // public void GetCompactFiltersTest()
        // {
        //     timNode.Handshake();
        //     timNode.Send(new GetCompactFiltersMessage(FilterType.Basic, 0, Cipher.ToBytes("000000004e833644bc7fb021abd3da831c64ec82bae73042cfa63923d47d3303")));
        //     while (true)
        //     {
        //         var message = timNode.WaitForMessage();
        //     }
        // }
    }
}
