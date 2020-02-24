namespace BitcoinBook
{
    public static class StandardScripts
    {
        public static Script PayToPublicKeyHash(byte[] hash160)
        {
            return new Script(
                OpCode.OP_DUP, 
                OpCode.OP_HASH160, 
                hash160, 
                OpCode.OP_EQUALVERIFY, 
                OpCode.OP_CHECKSIG);
        }

        public static Script PayToPublicKeyHash(PublicKey publicKey)
        {
            return PayToPublicKeyHash(publicKey.ToHash160());
        }
    }
}
