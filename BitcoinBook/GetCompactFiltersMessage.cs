using System;

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
            return Parse(bytes, reader =>
                new GetCompactFiltersMessage(
                    (FilterType) reader.ReadByte(),
                    reader.ReadUnsignedInt(4),
                    reader.ReadBytes(32)));
        }

        public override void Write(ByteWriter writer)
        {
            writer.Write((byte)FilterType);
            writer.Write(StartHeight, 4);
            writer.Write(StopHash);
        }
    }
}
