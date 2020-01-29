using System;
using System.IO;

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

        public int ReadVersion()
        {
            return ReadInt(4);
        }

        int ReadInt(int length)
        {
            var i = 0;
            var factor = 1;
            while (length-- > 0)
            {
                i += reader.ReadByte() * factor;
                factor *= 256;
            }

            return i;
        }
    }
}
