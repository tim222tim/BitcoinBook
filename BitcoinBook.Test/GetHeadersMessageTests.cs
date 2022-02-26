using Xunit;

namespace BitcoinBook.Test;

public class GetHeadersMessageTests
{
    readonly GetHeadersMessage message = new(
        8793487,
        2,
        "00000000000000000011421889a2d5d0f52f0e73decd0846167fc5e9011dd1cc",
        "00000000000000000008af94ff0f6f62457f781c1251a0e75672b1178eaa760d");

    const string messageHex = "8f2d860002ccd11d01e9c57f164608cdde730e2ff5d0d5a2891842110000000000000000000d76aa8e17b17256e7a051121c787f45626f0fff94af08000000000000000000";

    [Fact]
    public void ToBytesTest()
    {
        var actual = message.ToBytes().ToHex();
        Assert.Equal(messageHex, actual);
    }

    [Fact]
    public void ParseTest()
    {
        var newMessage = GetHeadersMessage.Parse(Cipher.ToBytes(messageHex));
        Assert.Equal(message.Version, newMessage.Version);
        Assert.Equal(message.Hashes, newMessage.Hashes);
        Assert.Equal(message.StartingBlock, newMessage.StartingBlock);
        Assert.Equal(message.EndingBlock, newMessage.EndingBlock);
    }

    [Fact]
    public void ConstructorDefaultsTest()
    {
        var newMessage = new GetHeadersMessage("00000000000000000011421889a2d5d0f52f0e73decd0846167fc5e9011dd1cc");
        Assert.Equal(GetHeadersMessage.DefaultVersion, newMessage.Version);
        Assert.Equal(1, newMessage.Hashes);
        Assert.Equal("00000000000000000011421889a2d5d0f52f0e73decd0846167fc5e9011dd1cc", newMessage.StartingBlock);
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", newMessage.EndingBlock);
    }
}