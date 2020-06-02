using System;
using System.ComponentModel.DataAnnotations;

namespace BitcoinBook
{
    public class HeaderChecker
    {
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
            if (!header.IsValidProofOfWork())
            {
                throw new ValidationException($"Invalid proof of work at {header.Id}");
            }

            if (header.PreviousBlock != PreviousHeader.Id)
            {
                throw new ValidationException($"Discontinuous block at {header.Id}");
            }

            if (header.Bits!= currentBits)
            {
                throw new ValidationException($"Invalid bits at {header.Id}");
            }

            PreviousHeader = header;
        }
    }
}
