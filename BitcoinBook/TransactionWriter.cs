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
                writer.Write(0b0);
            }
        }
    }
}
