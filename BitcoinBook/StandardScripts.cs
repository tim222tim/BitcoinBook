namespace BitcoinBook
{
    public static class StandardScripts
    {
        public static Script PayToPubKey(byte[] sec)
        {
            return new(
                sec,
                OpCode.OP_CHECKSIG);
        }

        public static Script PayToPubKey(PublicKey publicKey)
        {
            return PayToPubKey(publicKey.ToSec());
        }

        public static Script PayToPubKeyHash(byte[] hash160)
        {
            return new(
                OpCode.OP_DUP, 
                OpCode.OP_HASH160, 
                hash160, 
                OpCode.OP_EQUALVERIFY, 
                OpCode.OP_CHECKSIG);
        }

        public static Script PayToPubKeyHash(PublicKey publicKey)
        {
            return PayToPubKeyHash(publicKey.ToHash160());
        }
    }
}
