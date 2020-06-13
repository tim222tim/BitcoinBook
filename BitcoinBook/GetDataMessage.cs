using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace BitcoinBook
{
    public class GetDataMessage : IMessage
    {
        readonly List<BlockDataItem> items;

        public string Command => "getdata";

        public IList<BlockDataItem> Items => items.AsReadOnly();

        public GetDataMessage(IEnumerable<BlockDataItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            this.items = items.ToList();
            if (!this.items.Any()) throw new ArgumentException("Must be at least one item", nameof(items));
        }

        public static GetDataMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                var count = reader.ReadVarInt();
                var items = new List<BlockDataItem>();
                while (count-- > 0)
                {
                    items.Add(new BlockDataItem((BlockDataType)reader.ReadInt(4), reader.ReadBytes(32)));
                }
                return new GetDataMessage(items);
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
            writer.WriteVar(items.Count);
            foreach (var item in items)
            {
                writer.Write((int)item.BlockDataType, 4);
                writer.Write(item.Hash);
            }
            return stream.ToArray();
        }
    }
}
