using System;

namespace BitcoinBook;

public class OutputPoint
{
    public byte[] TransactionId { get; }
    public int Index { get; }

    public OutputPoint(byte[] transactionId, int index)
    {
        TransactionId = transactionId;
        Index = index;
    }

    public OutputPoint(string transactionId, int index) : this(Cipher.ToBytes(transactionId), index)
    { 
    }

    public OutputPoint(string outputPoint)
    {
        var split = outputPoint.Split(':');
        if (split.Length != 2)
        {
            throw new FormatException("Invalid output point format");
        }

        TransactionId = Cipher.ToBytes(split[0]);
        Index = int.Parse(split[1]);
    }
}