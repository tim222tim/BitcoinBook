using System.Net;

namespace BitcoinBook
{
    public class NetworkAddress
    {
        public ulong Services { get; }
        public IPAddress IPAddress { get; }
        public ushort Port { get; }
        public long TimeStamp { get; }

        public NetworkAddress() : this(0, new IPAddress(new byte[] { 0, 0, 0, 0 }), 0)
        {
        }

        public NetworkAddress(ulong services, IPAddress address, ushort port, long timeStamp = 0)
        {
            Services = services;
            IPAddress = address;
            Port = port;
            TimeStamp = timeStamp;
        }
    }
}
