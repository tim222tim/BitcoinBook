namespace BitcoinBook
{
    public class Merkle
    {
        public static byte[] ComputeParent(byte[] hash1, byte[] hash2)
        {
            return Cipher.Hash256(hash1.Concat(hash2));
        }
    }
}
