using System.Linq;
using System.Text;
using Xunit;

namespace BitcoinBook.Test
{
    public class FilterLoadMessageTests
    {
        readonly FilterLoadMessage message = new FilterLoadMessage(Cipher.ToBytes("4000600a080000010940"), 5, 99, 0);
        readonly string[] inputs = {"Hello World", "Goodbye!"};
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
            AssertMessage(FilterLoadMessage.Parse(Cipher.ToBytes(messageHex)));
        }

        [Fact]
        public void FilterConstructorTest()
        {
            var filter = new BloomFilter(message.Filter.Length, message.HashCount, message.Tweak, message.Flags);
            foreach (var value in inputs)
            {
                filter.Add(Encoding.ASCII.GetBytes(value));
            }
            AssertMessage(new FilterLoadMessage(filter));
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        void AssertMessage(FilterLoadMessage newMessage)
        {
            Assert.True(newMessage.Filter.SequenceEqual(message.Filter));
            Assert.Equal(message.HashCount, newMessage.HashCount);
            Assert.Equal(message.Tweak, newMessage.Tweak);
            Assert.Equal(message.Flags, newMessage.Flags);
        }
    }
}
