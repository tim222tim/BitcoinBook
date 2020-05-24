using System;

namespace BitcoinBook
{
    public class VerAckMessage : IMessage
    {
        public string Command => "verack";

        public static VerAckMessage Parse(byte[] bytes)
        {
            if (bytes == null) throw  new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 0) throw new FormatException("verack payload should be zero bytes");
            return new VerAckMessage();
        }

        public byte[] ToBytes()
        {
            return new byte[0];
        }
    }
}
