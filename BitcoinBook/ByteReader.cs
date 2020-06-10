using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BitcoinBook
{
    public class ByteReader
    {
        readonly BinaryReader reader;

        public ByteReader(BinaryReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public ByteReader(Stream stream) : this(new BinaryReader(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        public ByteReader(byte[] bytes) : this(new MemoryStream(bytes))
        {
        }

        public ByteReader(string hex) : this(Cipher.ToBytes(hex ?? throw new ArgumentNullException(nameof(hex))))
        {
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            var bytes = reader.ReadBytes(count);
            if (bytes.Length != count)
            {
                throw new EndOfStreamException($"Could not read {count} bytes");
            }
            return bytes;
        }

        public uint ReadUnsignedInt(int length)
        {
            return (uint) ReadUnsignedLong(length);
        }

        public int ReadInt(int length)
        {
            return (int) ReadUnsignedLong(length);
        }

        public long ReadLong(int length)
        {
            return (long) ReadUnsignedLong(length);
        }

        public ulong ReadUnsignedLong(int length)
        {
            ulong i = 0L;
            ulong factor = 1L;
            while (length-- > 0)
            {
                i += reader.ReadByte() * factor;
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
            return reader.ReadBytes(count).Reverse();
        }

        public byte[] ReadVarBytes()
        {
            return reader.ReadBytes(ReadVarInt());
        }

        public string ReadString(int length)
        {
            var builder = new StringBuilder();
            builder.Append(reader.ReadChars(length));
            return builder.ToString().TrimEnd('\0');
        }

        public string ReadString()
        {
            return ReadString(ReadVarInt());
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

        public IPAddress ReadAddress()
        {
            var bytesZero = reader.ReadBytes(10);
            var marker = ReadInt(2);
            var addressBytes = reader.ReadBytes(4);
            if (bytesZero.Any(b => b != 0) || marker != 0xffff)
            {
                return new IPAddress(0);
                // throw new FormatException("Expected IPv4 prefix");
            }

            var address = new IPAddress(addressBytes);
            return address;
        }

        public bool ReadBool()
        {
            return reader.ReadByte() != 0;
        }
    }
}