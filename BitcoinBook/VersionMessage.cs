﻿using System;
using System.IO;

namespace BitcoinBook
{
    public class VersionMessage : MessageBase
    {
        public const int DefaultVersion = 70015;

        static readonly Random random = new Random();

        public override string Command => "version";

        public int Version { get; }
        public ulong Services { get; }
        public long Timestamp { get; }
        public NetworkAddress ReceiverAddress { get; }
        public NetworkAddress SenderAddress { get; }
        public ulong Nonce { get; }
        public string UserAgent { get; }
        public int Height { get; }
        public bool RelayFlag { get; }

        public VersionMessage() : this(DefaultVersion, 0,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            new NetworkAddress(), 
            new NetworkAddress(), 
            NextNonce(), "/rossitertest:0.1/", 0, false)
        {
        }

        public VersionMessage(int version, ulong services, long timestamp, NetworkAddress receiverAddress, NetworkAddress senderAddress, ulong nonce, string userAgent, int height, bool relayFlag)
        {
            Version = version;
            Services = services;
            Timestamp = timestamp;
            ReceiverAddress = receiverAddress;
            SenderAddress = senderAddress;
            Nonce = nonce;
            UserAgent = userAgent;
            Height = height;
            RelayFlag = relayFlag;
        }

        static ulong NextNonce()
        {
            var buf = new byte[8];
            random.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        public static VersionMessage Parse(byte[] bytes)
        {
            return Parse(bytes, reader =>
                new VersionMessage(
                    reader.ReadInt(4),
                    reader.ReadUnsignedLong(8),
                    reader.ReadLong(8),

                    reader.ReadNetworkAddress(),
                    reader.ReadNetworkAddress(),

                    reader.ReadUnsignedLong(8),
                    reader.ReadString(),
                    reader.ReadInt(4),
                    reader.ReadBool()));
        }

        public override byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new ByteWriter(stream);
            writer.Write(Version, 4);
            writer.Write(Services, 8);
            writer.Write(Timestamp, 8);

            writer.Write(ReceiverAddress);
            writer.Write(SenderAddress);

            writer.Write(Nonce, 8);
            writer.Write(UserAgent);
            writer.Write(Height, 4);
            writer.Write(RelayFlag);
            return stream.ToArray();
        }
    }
}
