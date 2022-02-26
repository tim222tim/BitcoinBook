using System;
using Xunit;

namespace BitcoinBook.Test;

public class HeaderCheckerTests : IDisposable
{
    readonly SimpleNode timNode = new(IntegrationSetup.TimNode.Address);

    public void Dispose()
    {
        timNode.Dispose();
    }

    [Fact(Skip = "Takes a long time")]
    public void CheckFirstHeadersThroughDifficultyChangeTest()
    {
        timNode.Handshake();

        var checker = new HeaderChecker(BlockHeader.Genesis);
        for (var i = 0; i < 20; i++)
        {
            timNode.Send(new GetHeadersMessage(checker.PreviousHeader.Id));
            var message = timNode.WaitFor<HeadersMessage>();
            foreach (var header in message.BlockHeaders)
            {
                checker.Check(header);
            }
        }
    }
}