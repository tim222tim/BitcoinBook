using System.ComponentModel.DataAnnotations;

namespace BitcoinBook
{
    public class HeaderChecker
    {
        BlockHeader previous;

        public void Check(BlockHeader header)
        {
            if (previous != null)
            {
                if (!header.IsValidProofOfWork())
                {
                    throw new ValidationException($"Invalid proof of work at {header.Id}");
                }

                if (header.PreviousBlock != previous.Id)
                {
                    throw new ValidationException($"Discontinuous block at {header.Id}");
                }
            }

            previous = header;
        }
    }
}
