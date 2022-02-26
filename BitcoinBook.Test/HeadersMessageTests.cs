using Xunit;

namespace BitcoinBook.Test;

public class HeadersMessageTests
{
    const string blockId = "00000000000000000024fb37364cbf81fd49cc2d51c09c75c35433c3a1945d04";
    const string rawBlockHeader = "000000201929eb850a74427d0440cf6b518308837566cd6d0662790000000000000000001f6231ed3de07345b607ec2a39b2d01bec2fe10dfb7f516ba4958a42691c95316d0a385a459600185599fc5c";

    [Fact]
    public void NoBlocksParseTest()
    {
        var message = HeadersMessage.Parse(Cipher.ToBytes("00"));
        Assert.Equal("00", message.ToBytes().ToHex());
    }

    [Fact]
    public void NoBlocksToBytesTest()
    {
        var message = new HeadersMessage(new BlockHeader[0]);
        Assert.Equal("00", message.ToBytes().ToHex());
    }

    [Fact]
    public void OneBlockParseTest()
    {
        var messageHex = "01" + rawBlockHeader + "00";
        var message = HeadersMessage.Parse(Cipher.ToBytes(messageHex));
        Assert.Single(message.BlockHeaders);
        Assert.Equal(blockId, message.BlockHeaders[0].Id);
    }

    [Fact]
    public void TwoBlockParseTest()
    {
        var messageHex = "02" + rawBlockHeader + "00" + rawBlockHeader + "00";
        var message = HeadersMessage.Parse(Cipher.ToBytes(messageHex));
        Assert.Equal(2, message.BlockHeaders.Count);
        Assert.Equal(blockId, message.BlockHeaders[0].Id);
        Assert.Equal(blockId, message.BlockHeaders[1].Id);
    }

    [Fact]
    public void TwoBlocksToBytesTest()
    {
        var header = BlockHeader.Parse(Cipher.ToBytes(rawBlockHeader));
        var message = new HeadersMessage(new [] {header, header});
        Assert.Equal("02" + rawBlockHeader + "00" + rawBlockHeader + "00", message.ToBytes().ToHex());
    }
}