using System.Linq;
using Xunit;

namespace BitcoinBook.Test
{
    public class MerkleTests
    {
        [Fact]
        public void ComputerParentTest()
        {
            var hash1 = Cipher.ToBytes("c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5");
            var hash2 = Cipher.ToBytes("c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5");
            Assert.Equal("8b30c5ba100f6f2e5ad1e2a742e5020491240f8eb514fe97c713c31718ad7ecd", Merkle.ComputeParent(hash1, hash2).ToHex());
        }

        [Theory]
        [InlineData(new[]
            {
                "8b30c5ba100f6f2e5ad1e2a742e5020491240f8eb514fe97c713c31718ad7ecd",
            }, 
            new[]
            {
                "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", 
                "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5",
            })]
        [InlineData(new[]
            {
                "8b30c5ba100f6f2e5ad1e2a742e5020491240f8eb514fe97c713c31718ad7ecd",
                "7f4e6f9e224e20fda0ae4c44114237f97cd35aca38d83081c9bfd41feb907800",
                "3ecf6115380c77e8aae56660f5634982ee897351ba906a6837d15ebc3a225df0",
            }, 
            new[]
            {
                "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", 
                "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5", 
                "f391da6ecfeed1814efae39e7fcb3838ae0b02c02ae7d0a5848a66947c0727b0", 
                "3d238a92a94532b946c90e19c49351c763696cff3db400485b813aecb8a13181", 
                "10092f2633be5f3ce349bf9ddbde36caa3dd10dfa0ec8106bce23acbff637dae", 
            })]
        public void ComputerParentLevelTest(string[] expected, string[] hashes)
        {
            Assert.True(expected.SequenceEqual(Merkle.ComputeLevelParent(hashes.Select(Cipher.ToBytes)).Select(b => b.ToHex())));
        }
    }
}
