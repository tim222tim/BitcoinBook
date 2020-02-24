namespace BitcoinBook
{
    public interface IHashSigner
    {
        Signature Sign(byte[] hash);
    }
}