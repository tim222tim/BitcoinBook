using System.Collections;

namespace BitcoinBook
{
    public static class BitArrayExtensions
    {
        public static byte[] ToBytes(this BitArray bits)
        {
            var bytes = new byte[(bits.Length + 7) / 8];
            bits.CopyTo(bytes, 0);
            return bytes;
        }
    }
}
