using System;
using System.IO;
using System.Numerics;

namespace BitcoinBook
{
    public class TransactionReader
    {
        readonly BinaryReader reader;

        public TransactionReader(BinaryReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public TransactionReader(Stream stream) : this(new BinaryReader(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        public uint ReadVersion()
        {
            return ReadInt(4);
        }

        public ulong ReadVarInt()
        {
            var i = ReadLong(1);
            var len = GetVarLength(i);
            return len == 1 ? i : ReadLong(len);
        }

        int GetVarLength(ulong i)
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

        uint ReadInt(int length)
        {
            uint i = 0;
            uint factor = 1;
            while (length-- > 0)
            {
                i += reader.ReadByte() * factor;
                factor *= 256;
            }

            return i;
        }

        ulong ReadLong(int length)
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
    }
}
