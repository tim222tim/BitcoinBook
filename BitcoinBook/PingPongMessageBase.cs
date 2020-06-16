using System.IO;

namespace BitcoinBook
{
    public abstract class PingPongMessageBase : IMessage
    {
        public ulong Nonce { get; }

        public abstract string Command { get; }

        protected PingPongMessageBase(ulong nonce)
        {
            Nonce = nonce;
        }

        public byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new ByteWriter(stream);
            writer.Write(Nonce, 8);
            return stream.ToArray();
        }
    }
}
