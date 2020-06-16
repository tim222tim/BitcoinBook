using System;
using System.Collections.Generic;
using System.IO;
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
            var reader = new ByteReader(bytes);
            try
            {
                var count = reader.ReadVarInt();
                var addresses = new List<TimestampedNetworkAddress>();
                while (count-- > 0)
                {
                    addresses.Add(reader.ReadTimestampedNetworkAddress());
                }
                return new AddressMessage(addresses);
            }
            catch (EndOfStreamException ex)
            {
                throw new FormatException("Read past end of data", ex);
            }
        }

        public override byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new ByteWriter(stream);
            writer.WriteVar(addresses.Count);
            foreach (var address in addresses)
            {
                writer.Write(address);
            }
            return stream.ToArray();
        }
    }
}
