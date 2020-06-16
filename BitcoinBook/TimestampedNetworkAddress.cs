using System.Net;

namespace BitcoinBook
{
    public class TimestampedNetworkAddress : NetworkAddress
    {
        public uint Timestamp { get; }

        public TimestampedNetworkAddress()
        {
        }

        public TimestampedNetworkAddress(ulong services, IPAddress address, ushort port, uint timestamp) : base(services, address, port)
        {
            Timestamp = timestamp;
        }

        public TimestampedNetworkAddress(NetworkAddress networkAddress, uint timestamp) : this(networkAddress.Services, networkAddress.IPAddress, networkAddress.Port, timestamp)
        {
        }
    }
}
