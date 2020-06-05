using System;
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
    }
}
