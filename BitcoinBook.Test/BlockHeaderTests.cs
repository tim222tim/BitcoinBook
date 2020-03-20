using Xunit;

namespace BitcoinBook.Test
{
    public class BlockHeaderTests
    {
        [Fact]
        public void ComputeIdTest()
        {
            var header = new BlockHeader(2, "0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919", 
                "31951c69428a95a46b517ffb0de12fec1bd0b2392aec07b64573e03ded31621f", 1513622125, 0x18009645, 0x5cfc9955);
            Assert.Equal("00000000000000000024fb37364cbf81fd49cc2d51c09c75c35433c3a1945d04", header.Id);
        }
    }
}
