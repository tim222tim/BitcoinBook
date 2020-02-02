using System;
using System.IO;
using System.Numerics;

namespace BitcoinBook
{
    public class WriterBase
    {
        readonly BinaryWriter writer;

        public WriterBase(BinaryWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public WriterBase(Stream stream) : this(new BinaryWriter(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        public void Write(int i, int length)
        {
            Write((ulong)i, length);
        }

        public void Write(long i, int length)
        {
            Write((ulong)i, length);
        }

        protected void Write(byte b)
        {
            writer.Write(b);
        }

        public void Write(byte[] bytes)
        {
            writer.Write(bytes);
        }

        public void Write(OpCode opCode)
        {
            writer.Write((byte)opCode);
        }

        public void Write(ulong i, int length)
        {
            var bytes = new BigInteger(i).ToByteArray();
            var bx = 0;
            var wx = 0;
            while (bx < bytes.Length && wx++ < length)
            {
                Write(bytes[bx++]);
            }
            while (wx++ < length)
            {
                Write((byte)0);
            }
        }

        public void WriteVar(int i)
        {
            WriteVar((ulong)i);
        }

        public void WriteVar(ulong i)
        {
            var length = GetVarLength(i);
            if (length > 1)
            {
                Write(GetVarPrefix(i));
            }
            Write(i, length);
        }

        public void WriteVarBytes(byte[] bytes)
        {
            WriteVar((ulong) bytes.Length);
            writer.Write(bytes);
        }

        protected void WriteReverse(byte[] bytes)
        {
            var newBytes = (byte[])bytes.Clone();
            Array.Reverse(newBytes);
            writer.Write(newBytes);
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