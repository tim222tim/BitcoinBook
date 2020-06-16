using System;
using System.IO;
using System.Net;
using System.Numerics;
using System.Text;

namespace BitcoinBook
{
    public class ByteWriter
    {
        readonly BinaryWriter writer;

        public ByteWriter(BinaryWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public ByteWriter(Stream stream) : this(new BinaryWriter(stream ?? throw new ArgumentNullException(nameof(stream))))
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

        public void Write(byte b)
        {
            writer.Write(b);
        }

        public void Write(byte[] bytes)
        {
            writer.Write(bytes);
        }

        protected void Write(OpCode opCode)
        {
            writer.Write((byte)opCode);
        }

        public void Write(ulong i, int length)
        {
            var bytes = new BigInteger(i).ToLittleBytes();
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
            writer.Write(bytes.Reverse());
        }

        public void Write(string s, int length)
        {
            Write(Encoding.ASCII.GetBytes(s.PadRight(length, '\0')));
        }

        int GetVarLength(ulong i)
        {
            return i < 0xfd ? 1 : i < 0x10000 ? 2 : i < 0x100000000 ? 4 : 8;
        }

        byte GetVarPrefix(ulong i)
        {
            return i < 0x10000 ? (byte)0xfd : i < 0x100000000 ? (byte)0xfe : (byte)0xff;
        }

        public void Write(IPAddress ipAddress)
        {
            var bytes = ipAddress.GetAddressBytes();
            if (bytes.Length != 4)
            {
                throw new FormatException("Expecting IPv4");
            }

            Write(0, 10);
            Write(0xffff, 2);
            Write(bytes);
        }

        public void Write(NetworkAddress address)
        {
            Write(address.Services, 8);
            Write(address.IPAddress);
            Write(address.Port, 2);
        }

        public void Write(string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            WriteVarBytes(bytes);
        }

        public void Write(bool b)
        {
            Write(b ? 1 : 0, 1);
        }
    }
}