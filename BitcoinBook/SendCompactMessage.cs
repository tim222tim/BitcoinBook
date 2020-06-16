using System.IO;

namespace BitcoinBook
{
    public class SendCompactMessage : MessageBase
    {
        public byte Flag { get; }
        public ulong Version { get; }

        public override string Command => "sendcmpct";

        public SendCompactMessage(byte flag, ulong version)
        {
            Flag = flag;
            Version = version;
        }

        public static SendCompactMessage Parse(byte[] bytes)
        {
            return Parse(bytes, r => new SendCompactMessage(r.ReadByte(), r.ReadUnsignedLong(8)));
        }

        public override byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new ByteWriter(stream);
            writer.Write(Flag);
            writer.Write(Version, 8);
            return stream.ToArray();
        }
    }
}
