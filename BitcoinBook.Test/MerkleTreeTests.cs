using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BitcoinBook.Test;

public class MerkleTreeTests
{
    const string c117 = "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64cf5";

    static readonly Random random = new();

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(3, 4)]
    public void DepthTest(int expectedDepth, int leafCount)
    {
        Assert.Equal(expectedDepth, new MerkleTree(GetRandomHashes(leafCount)).Depth);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void CountTest(int leafCount)
    {
        Assert.Equal(leafCount, new MerkleTree(GetRandomHashes(leafCount)).LeafCount);
    }

    [Theory]
    [InlineData(new object[] {new string[0]})]
    [InlineData(new object[] {new[] {c117, null}})]
    [InlineData(new object[] {new[] {c117, ""}})]
    [InlineData(new object[] {new[] {c117, "c117ea8ec828342f4dfb0ad6bd140e03a50720ece40169ee38bdc15d9eb64c"}})]
    [InlineData(new object[] {new[] {c117, "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5", c117}})]
    public void HashesBadTest(string?[] hashes)
    {
        Assert.Throws<ArgumentException>(() => new MerkleTree(hashes.Select(s => s == null ? null : Cipher.ToBytes(s))!));
    }

    [Theory]
    [InlineData(c117, new[] {c117})]
    [InlineData("8b30c5ba100f6f2e5ad1e2a742e5020491240f8eb514fe97c713c31718ad7ecd", new[]
    {
        c117, 
        "c131474164b412e3406696da1ee20ab0fc9bf41c8f05fa8ceea7a08d672d7cc5"
    })]
    [InlineData("acbcab8bcc1af95d8d563b77d24c3d19b18f1486383d75a5085c4e86c86beed6", new[]
    {
        c117,
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
    [InlineData(c117, 1,
        new[] {c117}, null, new[] {false})]
    [InlineData("d20629030c7e48e778c1c837d91ebadc2f2ee319a0a0a461f4a9538b5cae2a69", 4,
        new[] {"2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
        new[] {"43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
        new[] {false, true, false, false, true})]
    [InlineData("d20629030c7e48e778c1c837d91ebadc2f2ee319a0a0a461f4a9538b5cae2a69", 4,
        new[] {"95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c", "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
        new[] {"b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
        new[] {false, false, true, false, false, false, true})]
    public void ConstructForProofTest(string expectedRootHash, int leafCount, IEnumerable<string> includedHashes, IEnumerable<string>? proofHashes, IEnumerable<bool> flags)
    {
        var proof = new MerkleProof(leafCount, includedHashes.Select(Cipher.ToBytes), (proofHashes ?? new string[0]).Select(Cipher.ToBytes), flags);
        var tree = new MerkleTree(proof);
        Assert.Equal(expectedRootHash, tree.Root.Hash.ToHex());
    }

    [Theory]
    [InlineData(1, new[] {c117}, null, null)]
    [InlineData(4, new[] {"2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
        new[] {"43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27"}, 
        new[] {false, true, false, false, true})]
    [InlineData(4, new[] {"95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c"}, 
        new[] {"b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
        new[] {false, false, true, false, false, false, true})]
    public void QueueAreTooSmallTest(int leafCount, IEnumerable<string> includedHashes, IEnumerable<string>? proofHashes, IEnumerable<bool>? flags)
    {
        var proof = new MerkleProof(leafCount, includedHashes.Select(Cipher.ToBytes), (proofHashes ?? new string[0]).Select(Cipher.ToBytes), flags ?? new bool[0]);
        Assert.Throws<InvalidOperationException>(() => new MerkleTree(proof));
    }

    [Theory]
    [InlineData(1, new[] {c117}, null, new[] {false, false})]
    [InlineData(4, new[] {"2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908", "aaaa750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
        new[] {"43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
        new[] {false, true, false, false, true})]
    [InlineData(4, new[] {"95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c", "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
        new[] {"b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0", "aaaa722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"}, 
        new[] {false, false, true, false, false, false, true})]
    public void QueueAreTooBigTest(int leafCount, IEnumerable<string> includedHashes, IEnumerable<string>? proofHashes, IEnumerable<bool>? flags)
    {
        var proof = new MerkleProof(leafCount, includedHashes.Select(Cipher.ToBytes), (proofHashes ?? new string[0]).Select(Cipher.ToBytes), flags ?? new bool[0]);
        Assert.Throws<InvalidOperationException>(() => new MerkleTree(proof));
    }

    [Theory]
    [InlineData(new[] {"43e7274e77fbe8e5a42a8fb58f7decdb04d521f319f332d88e6b06f8e6c09e27", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
        new[] {false, true, false, false, true}, new[] {"2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"})]
    [InlineData(new[] {"b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59", "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"}, 
        new[] {false, false, true, false, false, false, true},
        new[] {"95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c", "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908"})]
    public void CreateProofTest(string[] expectedProofHashes, bool[] expectedFlags, string[] includedHashes)
    {
        var tree = new MerkleTree(new[]
        {
            "b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59",
            "95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c",
            "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908",
            "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"
        }.Select(Cipher.ToBytes));
        var proof = tree.CreateProof(includedHashes.Select(Cipher.ToBytes));
        Assert.True(includedHashes.SequenceEqual(proof.IncludedHashes.Select(h => h.ToHex())));
        Assert.True(expectedProofHashes.SequenceEqual(proof.ProofHashes.Select(h => h.ToHex())));
        Assert.True(expectedFlags.SequenceEqual(proof.Flags));
    }

    [Fact]
    public void IncludedHashNotInTreeTest()
    {
        var tree = new MerkleTree(new[]
        {
            "b825c0745f46ac58f7d3759e6dc535a1fec7820377f24d4c2c6ad2cc55c0cb59",
            "95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c",
            "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908",
            "b13a750047bc0bdceb2473e5fe488c2596d7a7124b4e716fdd29b046ef99bbf0"
        }.Select(Cipher.ToBytes));

        var includedHashes = new[]
        {
            "95513952a04bd8992721e9b7e2937f1c04ba31e0469fbe615a78197f68f52b7c",
            "2e6d722e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908",
            "aaaaa22e5e4dbdf2447ddecc9f7dabb8e299bae921c99ad5b0184cd9eb8e5908",
        };

        Assert.Throws<InvalidOperationException>(() => tree.CreateProof(includedHashes.Select(Cipher.ToBytes)));
    }

    [Theory]
    [InlineData(new object[] {new [] {"822305b3ce53de4b63a1eb3cc6b873ce32bdf14737a30440646c80f321dd0de3"}})]
    [InlineData(new object[] {new [] {"822305b3ce53de4b63a1eb3cc6b873ce32bdf14737a30440646c80f321dd0de3", "49e64527efba5f8406178ef4adacb7348f8a82d25ab92a18910b9446916ff321"}})]
    [InlineData(new object[] {new [] {"e49764e53fa8ff12183005d69bb4515b5f28b5a93b42ddc1793e03cf702aa818", "971760e3ee5e381bf161bca5d8c6dc6061b239a0ec0ef0f60d6c80d110e42b76", "6ffe50262fa2860b589f24b709595ca79db0bb2aff05fde293acd8ca59987cee"}})]
    public void ProofAndVerifyTest(string[] includedHashes)
    {
        var tree = new MerkleTree(new[]
        {
            "822305b3ce53de4b63a1eb3cc6b873ce32bdf14737a30440646c80f321dd0de3",
            "c93bc57922f9f3efc3f0c6284ecbe8ca255a56c151525c265e2af9b8c82427e4",
            "1148ac093c5b9b179cec064fab73af632d3885983e63256f19ae37386a1b948f",
            "c2011c634ef083e1fb8578b07dac24bf0db38bfd177b6581652d3e887f8d0364",
            "b43e686197f2c34f096828daaaa3bfc600dc069f3b7237145d398dc00df01b44",
            "43ee349da93736ddb177d9cd99c162094a2b1be0cc992abc3069f079c54e0fa5",
            "ecc49063e23ac53fc166fd554b69aa9a31142a6952d4d70692acec5c4fca1a11",
            "49e64527efba5f8406178ef4adacb7348f8a82d25ab92a18910b9446916ff321",
            "93669e5589966137dad16296518f6190c007d8fa37c98b3f31b401478e172dc0",
            "971760e3ee5e381bf161bca5d8c6dc6061b239a0ec0ef0f60d6c80d110e42b76",
            "30ccea783fa1c0c2b7c1b448cd33f615579baff2cd51912beefe16d8921732a0",
            "3cf3180450ef1a12b3cd3af83c923c1b75c382ec0ea01273b76bbdbb58018480",
            "e49764e53fa8ff12183005d69bb4515b5f28b5a93b42ddc1793e03cf702aa818",
            "a8be33a077e95d640cb81844e78289fc076a59c9975ecef0c2cb788401733d11",
            "6ffe50262fa2860b589f24b709595ca79db0bb2aff05fde293acd8ca59987cee",}.Select(Cipher.ToBytes));
        var proof = tree.CreateProof(includedHashes.Select(Cipher.ToBytes));
        Assert.Equal(tree.Root.Hash.ToHex(), new MerkleTree(proof).Root.Hash.ToHex());
    }

    IEnumerable<byte[]> GetRandomHashes(int count)
    {
        var hashes = new byte[count][];
        for (var i = 0; i < count; i++)
        {
            hashes[i] = new byte[32];
            random.NextBytes(hashes[i]);
        }
        return hashes;
    }
}