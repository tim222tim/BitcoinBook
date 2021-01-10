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
            return Parse(bytes, r => new FeeFilterMessage(r.ReadUnsignedLong(8)));
        }

        public override void Write(ByteWriter writer)
        {
            writer.Write(FeeRate, 8);
        }
    }
}
