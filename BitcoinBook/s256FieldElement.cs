using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class S256FieldElement : FieldElement
    {
        public S256FieldElement(BigInteger number) : base(number, S256Curve.Field)
        {
        }

        public S256FieldElement(string number, NumberStyles numberStyles = NumberStyles.HexNumber) : 
            this(BigInteger.Parse(number, numberStyles))
        {
        }
    }
}
