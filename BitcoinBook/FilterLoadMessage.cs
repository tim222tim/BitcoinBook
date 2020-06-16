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
            return Parse(bytes, reader =>
                new FilterLoadMessage(
                    reader.ReadVarBytes(),
                    reader.ReadInt(4),
                    reader.ReadUnsignedInt(4),
                    reader.ReadByte()));
        }

        public override void Write(ByteWriter writer)
        {
            writer.WriteVarBytes(Filter);
            writer.Write(HashCount, 4);
            writer.Write(Tweak, 4);
            writer.Write(Flags);
        }
    }
}
