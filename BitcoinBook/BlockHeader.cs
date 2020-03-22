using System;
using System.Numerics;

namespace BitcoinBook
{
    public class BlockHeader
    {
        readonly byte[] id;
        readonly byte[] previousBlock;
        readonly byte[] merkleRoot;

        public string Id => id.ToHex();
        public BigInteger Target => (Bits & 0x00FFFFFF) * BigInteger.Pow(256, (int)(Bits >> 0x18) - 3);

        public uint Version { get; }
        public string PreviousBlock => previousBlock.ToReverseHex();
        public string MerkleRoot => merkleRoot.ToReverseHex();
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

        public static BlockHeader Parse(byte[] bytes)
        {
            if (bytes.Length != 80) throw new FormatException("must be 80 bytes");

            return new BlockHeader(
                BitConverter.ToUInt32(bytes, 0),
                bytes.Copy(4, 32).ToReverseHex(),
                bytes.Copy(36, 32).ToReverseHex(),
                BitConverter.ToUInt32(bytes, 68),
                BitConverter.ToUInt32(bytes, 72),
                BitConverter.ToUInt32(bytes, 76));
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

        public bool IsBip(int bip)
        {
            switch (bip)
            {
                case 9:  return Version >> 29 == 1;
                case 91:  return (Version >> 4 & 1) == 1;
                case 141:  return (Version >> 1 & 1) == 1;
                default:  throw new NotImplementedException();
            }
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
