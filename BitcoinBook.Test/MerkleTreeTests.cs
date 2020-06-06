using System;
using System.Collections.Generic;
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

        [Fact]
        public void HashesNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => new MerkleTree(null));
        }

        [Theory]
        [InlineData(new object[] {new string[0]})]
        [InlineData(new object[] {new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", null}})]
        [InlineData(new object[] {new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", ""}})]
        [InlineData(new object[] {new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64c"}})]
        [InlineData(new object[] {new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5", "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5"}})]
        public void HashesBadTest(string[] hashes)
        {
            Assert.Throws<ArgumentException>(() => new MerkleTree(hashes.Select(s => s == null ? null : Cipher.ToBytes(s))));
        }

        [Theory]
        [InlineData("c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5"})]
        [InlineData("8b30c5ba100f6f2e5ad1e2a742e5020491240f8eb514fe97c713c31718ad7ecd", new[]
        {
            "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", 
            "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5"
        })]
        [InlineData("acbcab8bcc1af95d8d563b77d24c3d19b18f1486383d75a5085c4e86c86beed6", new[]
        {
            "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5",
            "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5",
            "f391da6ecfeed1814efae39e7fcb3838ae0b02c02ae7d0a5848a66947c0727b0",
            "3d238a92a94532b946c90e19c49351c763696cff3db400485b813aecb8a13181",
            "10092f2633be5f3ce349bf9ddbde36caa3dd10dfa0ec8106bce23acbff637dae",
            "7d37b3d54fa6a64869084bfd2e831309118b9e833610e6228adacdbd1b4ba161",
            "8118a77e542892fe15ae3fc771a4abfd2f5d5d5997544c3487ac36b5c85170fc",
            "dff6879848c2c9b62fe652720b8df5272093acfaa45a43cdb3696fe2466a3877",
            "b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59",
            "95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c",
            "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908",
            "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0",
        })]
        public void ConstructWithValuesTest(string expectedRootHash, string[] hashes)
        {
            Assert.Equal(expectedRootHash, new MerkleTree(hashes.Select(Cipher.ToBytes)).Root.Hash.ToHex());
        }

        [Theory]
        [InlineData("c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5", 1,
            new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5"}, null, new[] {false})]
        [InlineData("d20629030c7e48e778c1c837d91ebadc2f2ee319a0a0a461f4a9538b5cae2a69", 4,
            new[] {"2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
            new[] {"43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
            new[] {false, true, false, false, true})]
        [InlineData("d20629030c7e48e778c1c837d91ebadc2f2ee319a0a0a461f4a9538b5cae2a69", 4,
            new[] {"95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c", "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
            new[] {"b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
            new[] {false, false, true, false, false, false, true})]
        public void ConstructForProofTest(string expectedRootHash, int leafCount, IEnumerable<string> includedHashes, IEnumerable<string> proofHashes, IEnumerable<bool> flags)
        {
            var proof = new MerkleProof(includedHashes.Select(Cipher.ToBytes), (proofHashes ?? new string[0]).Select(Cipher.ToBytes), flags);
            var tree = new MerkleTree(leafCount, proof);
            Assert.Equal(expectedRootHash, tree.Root.Hash.ToHex());
        }

        [Theory]
        [InlineData(1, new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5"}, null, null)]
        [InlineData(4, new[] {"2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
            new[] {"43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27"}, 
            new[] {false, true, false, false, true})]
        [InlineData(4, new[] {"95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c"}, 
            new[] {"b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
            new[] {false, false, true, false, false, false, true})]
        public void QueueAreTooSmallTest(int leafCount, IEnumerable<string> includedHashes, IEnumerable<string> proofHashes, IEnumerable<bool> flags)
        {
            var proof = new MerkleProof(includedHashes.Select(Cipher.ToBytes), (proofHashes ?? new string[0]).Select(Cipher.ToBytes), flags ?? new bool[0]);
            Assert.Throws<InvalidOperationException>(() => new MerkleTree(leafCount, proof));
        }
    
        [Theory]
        [InlineData(1, new[] {"c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5"}, null, new[] {false, false})]
        [InlineData(4, new[] {"2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908", "aaaa750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
            new[] {"43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
            new[] {false, true, false, false, true})]
        [InlineData(4, new[] {"95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c", "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
            new[] {"b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0", "aaaa722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
            new[] {false, false, true, false, false, false, true})]
        public void QueueAreTooBigTest(int leafCount, IEnumerable<string> includedHashes, IEnumerable<string> proofHashes, IEnumerable<bool> flags)
        {
            var proof = new MerkleProof(includedHashes.Select(Cipher.ToBytes), (proofHashes ?? new string[0]).Select(Cipher.ToBytes), flags ?? new bool[0]);
            Assert.Throws<InvalidOperationException>(() => new MerkleTree(leafCount, proof));
        }

        void GenerateHashes()
        {
            var hash = GetHash("b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c");
            Assert.Equal("43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27", hash);

            hash = GetHash("2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0");
            Assert.Equal("4f492e893bf854111c36cb5eff4dccbdd51b576e1cfdc1b84b456cd1c0403ccb", hash);

            hash = GetHash("43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27", "4f492e893bf854111c36cb5eff4dccbdd51b576e1cfdc1b84b456cd1c0403ccb");
            Assert.Equal("d20629030c7e48e778c1c837d91ebadc2f2ee319a0a0a461f4a9538b5cae2a69", hash);
        }

        static string GetHash(string hash1, string hash2)
        {
            return Merkler.ComputeParent(Cipher.ToBytes(hash1), Cipher.ToBytes(hash2)).ToHex();
        }
    }
}
