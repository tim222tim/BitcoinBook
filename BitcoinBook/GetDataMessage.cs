using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace BitcoinBook
{
    public class GetDataMessage : IMessage
    {
        readonly List<byte[]> items;

        public string Command => "getdata";

        public BlockDataType BlockDataType { get; }

        public IList<byte[]> Items => items.AsReadOnly();

        public GetDataMessage(BlockDataType blockDataType, IEnumerable<byte[]> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            BlockDataType = blockDataType;
            this.items = items.ToList();

            CheckHashes(this.items, false);
        }

        public static GetDataMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                var count = reader.ReadVarInt();
                var dataType = (BlockDataType)reader.ReadInt(4);
                var items = new List<byte[]>();
                while (count-- > 0)
                {
                    items.Add(reader.ReadBytes(32));
                }
                return new GetDataMessage(dataType, items);
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
            writer.Write((int)BlockDataType, 4);
            foreach (var item in items)
            {
                writer.Write(item);
            }
            return stream.ToArray();
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        void CheckHashes(List<byte[]> hashes, bool isEmptyAllowed)
        {
            if (!(isEmptyAllowed || hashes.Any())) throw new ArgumentException("Must contain at least one hash", nameof(hashes));
            if (hashes.Any(h => h == null || h.Length != 32)) throw new ArgumentException("All hashes must be 32 bytes", nameof(hashes));
        }
    }
}
