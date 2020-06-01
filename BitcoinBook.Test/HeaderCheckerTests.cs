using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class HeaderCheckerTests : IDisposable
    {
        readonly SimpleNode timNode = new SimpleNode(IntegrationSetup.TimNode.Address);

        public void Dispose()
        {
            timNode?.Dispose();
        }

        [Fact]
        public void CheckFirstHeadersTest()
        {
            timNode.Handshake();

            var previousHeader = BlockHeader.Genesis;
            timNode.Send(new GetHeadersMessage(previousHeader.Id));
            var message = timNode.WaitFor<HeadersMessage>();

            var checker = new HeaderChecker();
            checker.Check(previousHeader);
            foreach (var header in message.BlockHeaders)
            {
                checker.Check(header);
            }
        }
    }
}
