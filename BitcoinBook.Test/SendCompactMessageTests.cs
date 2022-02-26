using Xunit;

namespace BitcoinBook.Test;

public class SendCompactMessageTests
{
    readonly SendCompactMessage message = new(1, 1);

    const string messageHex = "010100000000000000";

    [Fact]
    public void ToBytesTest()
    {
        var actual = message.ToBytes().ToHex();
        Assert.Equal(messageHex, actual);
    }

    [Fact]
    public void ParseTest()
    {
        var newMessage = SendCompactMessage.Parse(Cipher.ToBytes(messageHex));
        Assert.Equal(message.Flag, newMessage.Flag);
        Assert.Equal(message.Version, newMessage.Version);
    }
}