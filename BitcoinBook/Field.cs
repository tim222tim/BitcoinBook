using System.Numerics;

namespace BitcoinBook
{
    public record Field(BigInteger Prime)
    {
        public FieldElement Element(BigInteger number)
        {
            return new(number, this);
        }
    }
}