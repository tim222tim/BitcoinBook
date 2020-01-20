using System.Numerics;

namespace BitcoinBook.Test
{
    public class Field
    {
        public BigInteger Prime { get; }

        public Field(BigInteger prime)
        {
            Prime = prime;
        }

        public FieldElement Element(BigInteger number)
        {
            return new FieldElement(number, Prime);
        }
    }
}