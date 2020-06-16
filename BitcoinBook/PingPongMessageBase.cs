namespace BitcoinBook
{
    public abstract class PingPongMessageBase : MessageBase
    {
        public ulong Nonce { get; }

        protected PingPongMessageBase(ulong nonce)
        {
            Nonce = nonce;
        }

        public override void Write(ByteWriter writer)
        {
            writer.Write(Nonce, 8);
        }
    }
}
