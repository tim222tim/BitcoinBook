namespace BitcoinBook;

public class PongMessage : PingPongMessageBase
{
    public override string Command => "pong";

    public PongMessage(ulong nonce) : base(nonce)
    {
    }

    public static PongMessage Parse(byte[] bytes)
    {
        return Parse(bytes, r => new PongMessage(r.ReadUnsignedLong(8)));
    }
}