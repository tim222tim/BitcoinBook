using System;
using System.IO;

namespace BitcoinBook
{
    public class FeeFilterMessage : MessageBase
    {
        public override string Command => "feefilter";

        public ulong FeeRate { get; }

        public FeeFilterMessage(ulong feeRate)
        {
            FeeRate = feeRate;
        }

        public static FeeFilterMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return new FeeFilterMessage(reader.ReadUnsignedLong(8));
            }
            catch (EndOfStreamException ex)
            {
                throw new FormatException("Read past end of data", ex);
            }
        }

        public override byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new ByteWriter(stream);
            writer.Write(FeeRate, 8);
            return stream.ToArray();
        }
    }
}
