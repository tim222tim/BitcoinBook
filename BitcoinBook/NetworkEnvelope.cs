using System;

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

        public override string ToString()
        {
            return string.Format($"{Command}: {Payload.ToHex()}");
        }
    }
}
