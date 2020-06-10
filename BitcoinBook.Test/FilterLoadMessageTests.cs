using System.Linq;
using Xunit;

namespace BitcoinBook.Test
{
    public class FilterLoadMessageTests
    {
        readonly FilterLoadMessage message = new FilterLoadMessage(Cipher.ToBytes("4000600a080000010940"), 5, 99, 0);
        const string messageHex = "0a4000600a080000010940050000006300000000";

        [Fact]
        public void ToBytesTest()
        {
            var actual = message.ToBytes().ToHex();
            Assert.Equal(messageHex, actual);
        }

        [Fact]
        public void ParseTest()
        {
            var newMessage = FilterLoadMessage.Parse(Cipher.ToBytes(messageHex));
            Assert.True(newMessage.Filter.SequenceEqual(message.Filter));
            Assert.Equal(message.HashCount, newMessage.HashCount);
            Assert.Equal(message.Tweak, newMessage.Tweak);
            Assert.Equal(message.Flags, newMessage.Flags);
        }
    }
}
