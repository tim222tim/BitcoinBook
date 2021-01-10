using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BitcoinBook
{
    public class HeadersMessage : MessageBase
    {
        readonly List<BlockHeader> blockHeaders;

        public ReadOnlyCollection<BlockHeader> BlockHeaders => blockHeaders.AsReadOnly();

        public override string Command => "headers";

        public HeadersMessage(IEnumerable<BlockHeader> blockHeaders)
        {
            this.blockHeaders = new List<BlockHeader>(blockHeaders ?? throw new ArgumentNullException(nameof(blockHeaders)));
        }

        public static HeadersMessage Parse(byte[] bytes)
        {
            return Parse(bytes, reader =>
            {
                var headers = new List<BlockHeader>();
                var count = reader.ReadVarInt();
                for (var i = 0; i < count; i++)
                {
                    headers.Add(BlockHeader.Parse(reader.ReadBytes(80)));
                    if (reader.ReadVarInt() != 0)
                    {
                        throw new FormatException("Expected no transactions");
                    }
                }

                return new HeadersMessage(headers);
            });
        }

        public override void Write(ByteWriter writer)
        {
            writer.WriteVar(blockHeaders.Count);
            foreach (var header in blockHeaders)
            {
                writer.Write(header.ToBytes());
                writer.WriteVar(0);
            }
        }
    }
}
