using System;
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

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public void IsBipThrowsNotImplementedTest(int bip)
        {
            Assert.Throws<NotImplementedException>(() => header500K.IsBip(bip));
        }

        [Theory]
        [InlineData(false, 9, 0x10000000)]
        [InlineData(true, 9, 0x20000000)]
        [InlineData(true, 9, 0x30000000)]
        [InlineData(false, 9, 0x40000000)]
        [InlineData(false, 91, 0x20000000)]
        [InlineData(true, 91, 0x00000010)]
        [InlineData(false, 141, 0x20000000)]
        [InlineData(true, 141, 0x00000002)]
        public void IsBipTest(bool expected, int bip, uint version)
        {
            Assert.Equal(expected, GetHeaderWithVersion(version).IsBip(bip));
        }

        [Fact]
        public void TargetTest()
        {
            var header = GetHeaderWithBits(0x18013ce9);
            Assert.Equal("0000000000000000013ce9000000000000000000000000000000000000000000", header.Target.ToHex32());
        }

        [Fact]
        public void ProofOfWorkTest()
        {
            Assert.True(header500K.IsValidProofOfWork());
        }

        [Fact]
        public void ProofOfWorkBadTest()
        {
            Assert.False(GetHeaderWithBits(0x19013ce9).IsValidProofOfWork());
        }

        [Fact]
        public void DifficultyTest()
        {
            var difficulty = GetHeaderWithBits(0x18013ce9).Difficulty;
            Assert.Equal(888171856257.3206, difficulty);
        }

        [Fact]
        public void GenesisBlockTest()
        {
            var header = BlockHeader.GenesisBlockHeader;
            Assert.Equal("000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f", header.Id);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", header.PreviousBlock);
        }

        BlockHeader GetHeaderWithVersion(uint version)
        {
            return new BlockHeader(
                version,
                "0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919",
                "31951c69428a95a46b517ffb0de12fec1bd0b2392aec07b64573e03ded31621f",
                1513622125,
                0x18009645,
                0x5cfc9955);
        }

        BlockHeader GetHeaderWithBits(uint bits)
        {
            return new BlockHeader(
                0x20000000,
                "0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919",
                "31951c69428a95a46b517ffb0de12fec1bd0b2392aec07b64573e03ded31621f",
                1513622125,
                bits,
                0x5cfc9955);
        }

    }
}
