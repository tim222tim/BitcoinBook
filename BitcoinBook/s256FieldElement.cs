using System.Numerics;

namespace BitcoinBook
{
    public class S256FieldElement : FieldElement
    {
        public static Field S256Field { get; } = new Field(BigInteger.Pow(2, 256) - BigInteger.Pow(2, 32) - 977);

        public S256FieldElement(BigInteger number) : base(number, S256Field)
        {
        }
    }
}
