using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class AddressMessage : MessageBase
    {
        readonly List<TimestampedNetworkAddress> addresses;

        public override string Command => "addr";

        public IList<TimestampedNetworkAddress> Addresses => addresses.AsReadOnly();

        public AddressMessage(IEnumerable<TimestampedNetworkAddress> addresses)
        {
            this.addresses = (addresses ?? throw new ArgumentNullException(nameof(addresses))).ToList();
            if (!addresses.Any()) throw new ArgumentException("Must be at least one address", nameof(addresses));
        }

        public static AddressMessage Parse(byte[] bytes)
        {
            return Parse(bytes, reader =>
            {
                var count = reader.ReadVarInt();
                var addresses = new List<TimestampedNetworkAddress>();
                while (count-- > 0)
                {
                    addresses.Add(reader.ReadTimestampedNetworkAddress());
                }

                return new AddressMessage(addresses);
            });
        }

        public override void Write(ByteWriter writer)
        {
            writer.WriteVar(addresses.Count);
            foreach (var address in addresses)
            {
                writer.Write(address);
            }
        }
    }
}
