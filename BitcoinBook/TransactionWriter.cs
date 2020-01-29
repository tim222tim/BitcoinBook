using System;
using System.IO;
using System.Numerics;

namespace BitcoinBook
{
    public class TransactionWriter
    {
        readonly BinaryWriter writer;

        public TransactionWriter(BinaryWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public TransactionWriter(Stream stream) : this(new BinaryWriter(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        public void WriteInt(ulong i, int length)
        {
            var bytes = new BigInteger(i).ToByteArray();
            var bx = 0;
            var wx = 0;
            while (bx < bytes.Length && wx++ < length)
            {
                writer.Write(bytes[bx++]);
            }
            while (wx++ < length)
            {
                writer.Write((byte)0);
            }
        }

        public void WriteVarInt(ulong i)
        {
            var length = GetVarLength(i);
            if (length > 1)
            {
                writer.Write(GetVarPrefix(i));
            }
            WriteInt(i, length);
        }

        int GetVarLength(ulong i)
        {
            return i < 0xfd ? 1 : i < 0x10000 ? 2 : i < 0x100000000 ? 4 : 8;
        }

        byte GetVarPrefix(ulong i)
        {
            return i < 0x10000 ? (byte)0xfd : i < 0x100000000 ? (byte)0xfe : (byte)0xff;
        }
    }
}
