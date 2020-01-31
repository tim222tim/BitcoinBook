﻿using System;
using System.IO;

namespace BitcoinBook
{
    public class ReaderBase
    {
        protected BinaryReader Reader { get; }

        protected ReaderBase(BinaryReader reader)
        {
            Reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        protected ReaderBase(Stream stream) : this(new BinaryReader(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        protected ReaderBase(string hex) : this(new MemoryStream(Cipher.ToBytes(hex ?? throw new ArgumentNullException(nameof(hex)))))
        {
        }

        public uint ReadUnsignedInt(int length)
        {
            return (uint) ReadUnsignedLong(length);
        }

        public int ReadInt(int length)
        {
            return (int) ReadUnsignedLong(length);
        }

        protected long ReadLong(int length)
        {
            return (long) ReadUnsignedLong(length);
        }

        protected ulong ReadUnsignedLong(int length)
        {
            ulong i = 0L;
            ulong factor = 1L;
            while (length-- > 0)
            {
                i += Reader.ReadByte() * factor;
                factor *= 256;
            }

            return i;
        }

        public int ReadVarInt()
        {
            return (int) ReadVarLong();
        }

        public long ReadVarLong()
        {
            var i = ReadLong(1);
            var len = GetVarLength(i);
            return len == 1 ? i : ReadLong(len);
        }

        protected byte[] ReadBytesReverse(int count)
        {
            var bytes = Reader.ReadBytes(count);
            Array.Reverse(bytes);
            return bytes;
        }

        protected byte[] ReadVarBytes()
        {
            return Reader.ReadBytes((int) ReadVarLong());
        }

        protected int GetVarLength(long i)
        {
            switch (i)
            {
                case 0xFD:
                    return 2;
                case 0xFE:
                    return 4;
                case 0xFF:
                    return 8;
                default:
                    return 1;
            }
        }
    }
}