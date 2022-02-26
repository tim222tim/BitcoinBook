using System.Linq;
using Xunit;

namespace BitcoinBook.Test;

public class GetDataMessageTests
{
    static readonly BlockDataItem[] items = 
    {
        new(BlockDataType.MerkleBlock, Cipher.ToBytes("00000000000000000011421889a2d5d0f52f0e73decd0846167fc5e9011dd1cc")),
        new(BlockDataType.Block, Cipher.ToBytes("0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919")),
    };
    readonly GetDataMessage message = new(items);
    const string messageHex = "020300000000000000000000000011421889a2d5d0f52f0e73decd0846167fc5e9011dd1cc020000000000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919";

    [Fact]
    public void ToBytesTest()
    {
        var actual = message.ToBytes().ToHex();
        Assert.Equal(messageHex, actual);
    }

    [Fact]
    public void ParseTest()
    {
        var newMessage = GetDataMessage.Parse(Cipher.ToBytes(messageHex));
        Assert.Equal(message.Items.Count, newMessage.Items.Count);
        for (var i = 0; i < message.Items.Count; i++)
        {
            Assert.Equal(message.Items[i].BlockDataType, newMessage.Items[i].BlockDataType);
            Assert.True(newMessage.Items[i].Hash.SequenceEqual(message.Items[i].Hash));
        }
    }
}