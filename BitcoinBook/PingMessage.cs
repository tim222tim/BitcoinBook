using System;
using System.IO;

namespace BitcoinBook
{
    public class PingMessage : PingPongMessageBase
    {
        public override string Command => "ping";

        public PingMessage(ulong nonce) : base(nonce)
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
