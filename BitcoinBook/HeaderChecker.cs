using System;
using System.ComponentModel.DataAnnotations;

namespace BitcoinBook;

public class HeaderChecker
{
    int count;
    uint firstTimestamp;
    uint currentBits;

    public BlockHeader PreviousHeader { get; private set; }

    public HeaderChecker(BlockHeader previous)
    {
        PreviousHeader = previous ?? throw new ArgumentNullException(nameof(previous));
        firstTimestamp = PreviousHeader.Timestamp;
        currentBits = PreviousHeader.Bits;
    }

    public void Check(BlockHeader header)
    {
        ++count;
        if (!header.IsValidProofOfWork())
        {
            throw CreateException(header, "Invalid proof of work");
        }

        if (header.PreviousBlock != PreviousHeader.Id)
        {
            throw CreateException(header, "Discontinuous block");
        }

        if (count % 2016 == 0)
        {
            currentBits = BlockMath.ComputeNewBits(firstTimestamp, PreviousHeader.Timestamp, currentBits);
            firstTimestamp = header.Timestamp;
        }

        if (header.Bits != currentBits)
        {
            throw CreateException(header, "Invalid bits");
        }

        PreviousHeader = header;
    }

    ValidationException CreateException(BlockHeader header, string message)
    {
        return new($"{message} at #{count} {header.Id}");
    }
}