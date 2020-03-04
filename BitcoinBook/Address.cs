namespace BitcoinBook
{
    public static class Address
    {
        public static string ToAddress(byte prefix, byte[] hash)
        {
            return Cipher.ToBase58Check(new[] { prefix }.Concat(hash));
        }

        public static string ToPayToPublicKeyHashAddress(byte[] hash, bool testnet = false)
        {
            return ToAddress(testnet ? (byte)'\x6f' : (byte)0, hash);
        }

        public static byte[] HashFromAddress(string address)
        {
            return Cipher.FromBase58Check(address).Copy(1);
        }
    }
}
