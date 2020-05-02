using System;
using System.IO;
using System.Linq;

namespace BitcoinBook
{
    public class NetworkEnvelope
    {
        public string Command { get; }
        public byte[] Payload { get; }
        public IMessage Message { get; }
        public bool Testnet { get; }

        public NetworkEnvelope(string command, byte[] payload, bool testnet = false)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            Testnet = testnet;
            Message = ParseMessage(Command, Payload);
        }

        public NetworkEnvelope(IMessage message, bool testnet) : this(message?.Command, message?.ToBytes(), testnet)
        {
        }

        public static NetworkEnvelope Parse(byte[] bytes, bool testnet = false)
        {
            return Parse(new MemoryStream(bytes));
        }

        public static NetworkEnvelope Parse(Stream stream, bool testnet = false)
        {
            var reader = new ByteReader(stream);
            try
            {
                var magic = reader.ReadUnsignedInt(4);
                if (magic != Magic(testnet))
                {
                    throw new FormatException("Invalid magic");
                }

                var command = reader.ReadString(12);
                var payloadLength = reader.ReadInt(4);
                var checksum = reader.ReadBytes(4);
                var payload = payloadLength > 0 ? reader.ReadBytes(payloadLength) : new byte[0];
                if (!checksum.SequenceEqual(Cipher.Hash256Prefix(payload)))
                {
                    throw new FormatException("Bad checksum");
                }
                return new NetworkEnvelope(command, payload, testnet);
            }
            catch (EndOfStreamException ex)
            {
                throw new FormatException("Read past end of data", ex);
            }
        }

        public byte[] ToBytes()
        {
            var stream = new MemoryStream();
            WriteTo(stream);
            return stream.ToArray();
        }

        public void WriteTo(Stream stream)
        {
            var writer = new ByteWriter(stream);
            writer.Write(Magic(Testnet), 4);
            writer.Write(Command, 12);
            writer.Write(Payload.Length, 4);
            writer.Write(Cipher.Hash256Prefix(Payload));
            writer.Write(Payload);
        }

        public override string ToString()
        {
            return string.Format($"{Command}: {Payload.ToHex()}");
        }

        IMessage ParseMessage(string commandName, byte[] payload)
        {
            switch (commandName)
            {
                case "version":
                    return VersionMessage.Parse(payload);
                case "verack":
                    return VerAckMessage.Parse(payload);
                default:
                    return null;
            }
        }

        static uint Magic(bool testnet)
        {
            return testnet ? 0x0709110b : 0xd9b4bef9;
        }
    }
}
