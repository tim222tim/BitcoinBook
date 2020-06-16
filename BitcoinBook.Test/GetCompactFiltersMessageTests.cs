using System.Linq;
using Xunit;

namespace BitcoinBook.Test
{
    public class GetCompactFiltersMessageTests
    {
        readonly GetCompactFiltersMessage message = new GetCompactFiltersMessage(FilterType.Basic, 0x01020304, Cipher.ToBytes("0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919"));
        const string messageHex = "00040302010000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919";
        
        [Fact]
        public void ToBytesTest()
        {
            var actual = message.ToBytes().ToHex();
            Assert.Equal(messageHex, actual);
        }

        [Fact]
        public void ParseTest()
        {
            var newMessage = GetCompactFiltersMessage.Parse(Cipher.ToBytes(messageHex));
            Assert.Equal(message.FilterType, newMessage.FilterType);
            Assert.Equal(message.StartHeight, newMessage.StartHeight);
            Assert.True(newMessage.StopHash.SequenceEqual(message.StopHash));
        }
    }
}
