using System;
using System.Linq;
using Xunit;

namespace BitcoinBook.Test
{
    public class MerkleTreeTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void LeafCountThrowsTest(int leafCount)
        {
            Assert.Throws<ArgumentException>(() => new MerkleTree(leafCount));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(3, 4)]
        public void DepthTest(int expectedDepth, int leafCount)
        {
            Assert.Equal(expectedDepth, new MerkleTree(leafCount).Depth);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void CountTest(int leafCount)
        {
            Assert.Equal(leafCount, new MerkleTree(leafCount).LeafCount);
        }

        [Theory]
        [InlineData("c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5"})]
        [InlineData("8b30c5ba100f6f2e5ad1e2a742e5020491240f8eb514fe97c713c31718ad7ecd", new[]
        {
            "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5"
        })]
        public void ConstructWithValuesTest(string expectedRootValue, string[] values)
        {
            Assert.Equal(expectedRootValue, new MerkleTree(values.Select(Cipher.ToBytes)).Root.Value.ToHex());
        }
    }
}
