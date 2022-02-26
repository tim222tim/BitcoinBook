using System.Collections.Generic;
using System.Numerics;

namespace BitcoinBook;

public class ScriptStack
{
    readonly Stack<byte[]> stack = new();

    public int Count => stack.Count;

    public bool Push(byte[] bytes)
    {
        stack.Push(bytes);
        return true;
    }

    public bool Push(BigInteger i)
    {
        return Push(i == 0 ? new byte[0] : i.ToLittleBytes());
    }

    public bool Push(bool b)
    {
        return Push(b ? 1 : 0);
    }

    public byte[] Pop()
    {
        return stack.Pop();
    }

    public BigInteger PopInt()
    {
        return GetBigInteger(Pop());
    }

    public PublicKey PopPublicKey()
    {
        var publicKey = PublicKey.FromSec(Pop());
        return publicKey;
    }

    public Signature PopSignature()
    {
        // last byte is hash type! -- should this be previously removed?
        var signature = Signature.FromDer(Pop().Copy(0, -1));
        return signature;
    }

    public byte[] Peek()
    {
        return stack.Peek();
    }

    public BigInteger PeekInt()
    {
        return GetBigInteger(Peek());
    }

    BigInteger GetBigInteger(byte[] bytes)
    {
        return bytes.Length == 0 ? 0 : new BigInteger(bytes);
    }
}