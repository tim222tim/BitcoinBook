using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BitcoinBook
{
    public static class S256Curve
    {
        public static Field Field { get; } = new Field(BigInteger.Pow(2, 256) - BigInteger.Pow(2, 32) - 977);
        public static Curve Curve { get; } = new Curve(Field, 0, 7);
        public static Point Generator = Curve.Point(
            "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", 
            "483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8");
        public static BigInteger Order { get; } = 
            BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", NumberStyles.HexNumber);

        public static Point Point(string x, string y, NumberStyles numberStyles = NumberStyles.HexNumber)
        {
            return Curve.Point(x, y, numberStyles);
        }

        public static BigInteger ComputeHash(byte[] data)
        {
            using var sha256Hash = SHA256.Create();
            return new BigInteger(AddZeros(sha256Hash.ComputeHash(data), 4));
        }

        public static BigInteger ComputeHash(string data)
        {
            return ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        static byte[] AddZeros(byte[] bytes, int count)
        {
            var newBytes = new byte[bytes.Length + count];
            for (int i = 0; i < count; i++)
            {
                newBytes[i] = 0;
            }
            bytes.CopyTo(newBytes, 2);
            return newBytes;
        }
    }
}
