using System;
using System.IO;

namespace BitcoinBook
{
    public class FilterLoadMessage : MessageBase
    {
        public override string Command => "filterload";

        public byte[] Filter { get; }
        public int HashCount { get; }
        public uint Tweak { get; }
        public byte Flags { get; }

        public FilterLoadMessage(byte[] filter, int hashCount, uint tweak, byte flags)
        {
            Filter = filter;
            HashCount = hashCount;
            Tweak = tweak;
            Flags = flags;
        }

        public FilterLoadMessage(BloomFilter filter) : this(filter.Result, filter.HashCount, filter.Tweak, filter.Flags)
        {
        }

        public static FilterLoadMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return new FilterLoadMessage(
                    reader.ReadVarBytes(),
                    reader.ReadInt(4),
                    reader.ReadUnsignedInt(4),
                    reader.ReadByte());
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
            writer.WriteVarBytes(Filter);
            writer.Write(HashCount, 4);
            writer.Write(Tweak, 4);
            writer.Write(Flags);
            return stream.ToArray();
        }
    }
}
