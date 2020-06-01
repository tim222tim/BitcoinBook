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
            var previousId = BlockHeader.Genesis.Id;
            timNode.Send(new GetHeadersMessage(previousId));
            var message = timNode.WaitFor<HeadersMessage>();

            var checker = new HeaderChecker();
            checker.Check(BlockHeader.Genesis);
            foreach (var header in message.BlockHeaders)
            {
                checker.Check(header);
            }
        }
    }
}
