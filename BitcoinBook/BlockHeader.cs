using System;

namespace BitcoinBook
{
    public class BlockHeader
    {
        public uint Version { get; }
        public byte[] PreviousBlock { get; }
        public byte[] MerkleRoot { get; }
        public uint Timestamp { get; }
        public uint Bits { get; }
        public uint Nonce { get; }

        public BlockHeader(uint version, byte[] previousBlock, byte[] merkleRoot, uint timestamp, uint bits, uint nonce)
        {
            Version = version;
            PreviousBlock = previousBlock;
            MerkleRoot = merkleRoot;
            Timestamp = timestamp;
            Bits = bits;
            Nonce = nonce;
        }
    }
}
