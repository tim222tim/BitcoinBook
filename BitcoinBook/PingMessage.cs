namespace BitcoinBook;

public class PingMessage : PingPongMessageBase
{
    public override string Command => "ping";

    public PingMessage(ulong nonce) : base(nonce)
    {
    }

    public static PingMessage Parse(byte[] bytes)
    {
        return Parse(bytes, r => new PingMessage(r.ReadUnsignedLong(8)));
    }
}