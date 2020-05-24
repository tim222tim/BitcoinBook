using System;
using System.IO;
using System.Net;

namespace BitcoinBook
{
    public class VersionMessage : IMessage
    {
        public const int DefaultVersion = 70015;

        static readonly Random random = new Random();

        public string Command => "version";

        public int Version { get; }
        public long Services { get; }
        public long Timestamp { get; }
        public long ReceiverServices { get; }
        public IPAddress ReceiverAddress { get; }
        public ushort ReceiverPort { get; }
        public long SenderServices { get; }
        public IPAddress SenderAddress { get; }
        public ushort SenderPort { get; }
        public ulong Nonce { get; }
        public string UserAgent { get; }
        public int Height { get; }
        public bool Flag { get; }

        public VersionMessage() : this(DefaultVersion, 0,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            0, new IPAddress(new byte[] { 0, 0, 0, 0 }), 0,
            0, new IPAddress(new byte[] { 0, 0, 0, 0 }), 0,
            NextNonce(), "/rossitertest:0.1/", 0, true)
        {
        }

        public VersionMessage(int version, long services, long timestamp, long receiverServices, IPAddress receiverAddress, ushort receiverPort, long senderServices, IPAddress senderAddress, ushort senderPort, ulong nonce, string userAgent, int height, bool flag)
        {
            Version = version;
            Services = services;
            Timestamp = timestamp;

            ReceiverServices = receiverServices;
            ReceiverAddress = receiverAddress;
            ReceiverPort = receiverPort;

            SenderServices = senderServices;
            SenderAddress = senderAddress;
            SenderPort = senderPort;

            Nonce = nonce;
            UserAgent = userAgent;
            Height = height;
            Flag = flag;
        }

        static ulong NextNonce()
        {
            var buf = new byte[8];
            random.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        public static VersionMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return new VersionMessage(
                    reader.ReadInt(4),
                    reader.ReadLong(8),
                    reader.ReadLong(8),

                    reader.ReadLong(8),
                    reader.ReadAddress(),
                    (ushort)reader.ReadInt(2),

                    reader.ReadLong(8),
                    reader.ReadAddress(),
                    (ushort)reader.ReadInt(2),

                    reader.ReadUnsignedLong(8),
                    reader.ReadString(),
                    reader.ReadInt(4),
                    reader.ReadBool());
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
            writer.Write(Version, 4);
            writer.Write(Services, 8);
            writer.Write(Timestamp, 8);

            writer.Write(ReceiverServices, 8);
            writer.Write(ReceiverAddress);
            writer.Write(ReceiverPort, 2);

            writer.Write(SenderServices, 8);
            writer.Write(SenderAddress);
            writer.Write(SenderPort, 2);

            writer.Write(Nonce, 8);
            writer.Write(UserAgent);
            writer.Write(Height, 4);
            writer.Write(Flag);
            return stream.ToArray();
        }
    }
}
