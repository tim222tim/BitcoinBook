using Xunit;
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace BitcoinBook.Test
{
    public class BlockHeaderTests
    {
        const string blockId = "00000000000000000024fb37364cbf81fd49cc2d51c09c75c35433c3a1945d04";
        const string rawBlockHeader = "000000201929eb850a74427d0440cf6b518308837566cd6d0662790000000000000000001f6231ed3de07345b607ec2a39b2d01bec2fe10dfb7f516ba4958a42691c95316d0a385a459600185599fc5c";

        readonly BlockHeader header500K = new BlockHeader(
            0x20000000, 
            "0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919", 
            "31951c69428a95a46b517ffb0de12fec1bd0b2392aec07b64573e03ded31621f", 
            1513622125, 
            0x18009645, 
            0x5cfc9955);

        [Fact]
        public void PropertiesTest()
        {
            AssertBlockHeader(header500K);
        }

        [Fact]
        public void ParseBytesTest()
        {
            AssertBlockHeader(BlockHeader.Parse(Cipher.ToBytes(rawBlockHeader)));
        }

        void AssertBlockHeader(BlockHeader header)
        {
            Assert.Equal(0x20000000U, header.Version);
            Assert.Equal("0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919", header.PreviousBlock);
            Assert.Equal("31951c69428a95a46b517ffb0de12fec1bd0b2392aec07b64573e03ded31621f", header.MerkleRoot);
            Assert.Equal(1513622125U, header.Timestamp);
            Assert.Equal(0x18009645U, header.Bits);
            Assert.Equal(0x5cfc9955U, header.Nonce);
        }

        [Fact]
        public void ToBytesTest()
        {
            Assert.Equal(rawBlockHeader, header500K.ToBytes().ToHex());
        }

        [Fact]
        public void ComputeIdTest()
        {
            Assert.Equal(blockId, header500K.Id);
        }
    }
}
