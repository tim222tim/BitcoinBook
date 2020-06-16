using System;
using System.IO;

namespace BitcoinBook
{
    public class GetCompactFiltersMessage : MessageBase
    {
        public FilterType FilterType { get; }
        public uint StartHeight { get; }
        public byte[] StopHash { get; }

        public override string Command => "getcfilters";

        public GetCompactFiltersMessage(FilterType filterType, uint startHeight, byte[] stopHash)
        {
            FilterType = filterType;
            StartHeight = startHeight;
            StopHash = stopHash ?? throw new ArgumentNullException(nameof(stopHash));
        }

        public static GetCompactFiltersMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return new GetCompactFiltersMessage(
                    (FilterType) reader.ReadByte(),
                    reader.ReadUnsignedInt(4),
                    reader.ReadBytes(32));
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
            writer.Write((byte)FilterType);
            writer.Write(StartHeight, 4);
            writer.Write(StopHash);
            return stream.ToArray();
        }
    }
}
