using System.IO;

namespace BitcoinBook
{
    public abstract class PingPongMessageBase : MessageBase
    {
        public ulong Nonce { get; }

        protected PingPongMessageBase(ulong nonce)
        {
            Nonce = nonce;
        }

        public override byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new ByteWriter(stream);
            writer.Write(Nonce, 8);
            return stream.ToArray();
        }
    }
}
