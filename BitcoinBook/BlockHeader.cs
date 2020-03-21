using System;
using System.ComponentModel;

namespace BitcoinBook
{
    public class BlockHeader
    {
        readonly byte[] id;
        readonly byte[] previousBlock;
        readonly byte[] merkleRoot;

        public string Id => id.ToHex();
        public uint Version { get; }
        public string PreviousBlock => previousBlock.ToHex(); // TODO should be reversed
        public string MerkleRoot => merkleRoot.ToHex(); // TODO should be reversed
        public uint Timestamp { get; }
        public uint Bits { get; }
        public uint Nonce { get; }


        public BlockHeader(uint version, string previousBlock, string merkleRoot, uint timestamp, uint bits, uint nonce)
        {
            Version = version;
            this.previousBlock = Cipher.ToReverseBytes(previousBlock);
            this.merkleRoot = Cipher.ToReverseBytes(merkleRoot);
            Timestamp = timestamp;
            Bits = bits;
            Nonce = nonce;
            id = ComputeId();
        }

        byte[] ComputeId()
        {
            return Cipher.ReverseHash256(ToBytes());
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[80];
            CopyBytes(Version, bytes, 0);
            Array.Copy(previousBlock, 0, bytes, 4, 32);
            Array.Copy(merkleRoot, 0, bytes, 36, 32);
            CopyBytes(Timestamp, bytes, 68);
            CopyBytes(Bits, bytes, 72);
            CopyBytes(Nonce, bytes, 76);
            return bytes;
        }

        void CopyBytes(uint value, byte[] bytes, int index)
        {
            bytes[index] = (byte)value;
            bytes[++index] = (byte)(value >> 8);
            bytes[++index] = (byte)(value >> 0x10);
            bytes[++index] = (byte)(value >> 0x18);
        }
    }
}
