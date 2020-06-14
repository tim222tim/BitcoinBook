using System;
using System.IO;

namespace BitcoinBook
{
    public class PongMessage : PingPongMessageBase
    {
        public override string Command => "pong";

        public PongMessage(ulong nonce) : base(nonce)
        {
        }

        public static PingMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return new PingMessage(reader.ReadUnsignedLong(8));
            }
            catch (EndOfStreamException ex)
            {
                throw new FormatException("Read past end of data", ex);
            }
        }
    }
}
