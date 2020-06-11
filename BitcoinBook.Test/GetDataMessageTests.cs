using System.Linq;
using Xunit;

namespace BitcoinBook.Test
{
    public class GetDataMessageTests
    {
        static readonly string[] items = 
        {
            "00000000000000000011421889a2d5d0f52f0e73decd0846167fc5e9011dd1cc",
            "0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919",
        };
        readonly GetDataMessage message = new GetDataMessage(BlockDataType.MerkleBlock, items.Select(Cipher.ToBytes));
        const string messageHex = "020300000000000000000000000011421889a2d5d0f52f0e73decd0846167fc5e9011dd1cc0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919";

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
            Assert.Equal(message.BlockDataType, newMessage.BlockDataType);
            Assert.Equal(message.Items.Count, newMessage.Items.Count);
            for (var i = 0; i < message.Items.Count; i++)
            {
                Assert.True(newMessage.Items[i].SequenceEqual(message.Items[i]));
            }
        }
    }
}
