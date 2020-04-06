using Xunit;

namespace BitcoinBook.Test
{
    public class NetworkEnvelopeTests
    {
        [Fact]
        public void ToStringTest()
        {
            Assert.Equal("VERSION: 010210", new NetworkEnvelope("VERSION", new byte[] {1, 2, 16}, false).ToString());
        }
    }
}
