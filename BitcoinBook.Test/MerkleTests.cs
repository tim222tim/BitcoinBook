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
    }
}
