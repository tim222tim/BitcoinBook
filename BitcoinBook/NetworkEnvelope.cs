using System;
using System.IO;
using System.Linq;

namespace BitcoinBook
{
    public class NetworkEnvelope
    {
        public string Command { get; }
        public byte[] Payload { get; }
        public bool Testnet { get; }

        public NetworkEnvelope(string command, byte[] payload, bool testnet)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            Testnet = testnet;
        }

        public static NetworkEnvelope Parse(byte[] bytes, bool testnet)
        {
            var reader = new ByteReader(bytes);
            try
            {
                var magic = reader.ReadUnsignedInt(4);
                if (magic != (testnet ? 0x0709110b: 0xd9b4bef9))
                {
                    throw new FormatException("Invalid magic");
                }

                var command = reader.ReadString(12);
                var payloadLength = reader.ReadInt(4);
                var payload = payloadLength > 0 ? reader.ReadBytes(payloadLength) : new byte[0];
                var checksum = reader.ReadBytes(4);
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

        public override string ToString()
        {
            return string.Format($"{Command}: {Payload.ToHex()}");
        }
    }
}
