using System;
using System.IO;

namespace BitcoinBook
{
    public class SendCompactMessage : IMessage
    {
        public byte Flag { get; }
        public ulong Version { get; }

        public string Command => "sendcmpct";

        public SendCompactMessage(byte flag, ulong version)
        {
            Flag = flag;
            Version = version;
        }

        public static SendCompactMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return new SendCompactMessage(
                    reader.ReadByte(),
                    reader.ReadUnsignedLong(8));
            }
            catch (EndOfStreamException ex)
            {
                throw new FormatException("Read past end of data", ex);
            }
        }

        public byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new ByteWriter(stream);
            writer.Write(Flag);
            writer.Write(Version, 8);
            return stream.ToArray();
        }
    }
}
