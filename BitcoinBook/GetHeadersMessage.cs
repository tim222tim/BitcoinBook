using System;
using System.IO;

namespace BitcoinBook
{
    public class GetHeadersMessage : IMessage
    {
        public const int DefaultVersion = 70015;

        readonly byte[] startingBlock;
        readonly byte[] endingBlock;

        public string Command => "getheaders";

        public int Version { get; }
        public int Hashes { get; }
        public string StartingBlock => startingBlock.ToReverseHex();
        public string EndingBlock => endingBlock.ToReverseHex();

        public GetHeadersMessage(int version, int hashes, string startingBlock, string endingBlock)
        {
            Version = version;
            Hashes = hashes;
            this.startingBlock = Cipher.ToReverseBytes(startingBlock);
            this.endingBlock = Cipher.ToReverseBytes(endingBlock);
        }

        public GetHeadersMessage(string startingBlock) : this(DefaultVersion, 1, startingBlock, "0000000000000000000000000000000000000000000000000000000000000000")
        {
        }

        public static GetHeadersMessage Parse(byte[] bytes)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return new GetHeadersMessage(
                    reader.ReadInt(4),
                    reader.ReadVarInt(),
                    reader.ReadBytes(32).ToReverseHex(),
                    reader.ReadBytes(32).ToReverseHex());
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
            writer.Write(Version, 4);
            writer.WriteVar(Hashes);
            writer.Write(startingBlock);
            writer.Write(endingBlock);
            return stream.ToArray();
        }
    }
}
