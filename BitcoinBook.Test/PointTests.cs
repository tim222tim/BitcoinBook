using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class PointTests
    {
        [Theory]
        [InlineData(192,105)]
        [InlineData(17, 56)]
        [InlineData(1, 193)]
        public void ValidPointsTest(int x, int y)
        {
            var f = new Field(223);
            var c = new Curve(f.Element(0), f.Element(7));
            c.Point(f.Element(x), f.Element(y));
        }

        [Theory]
        [InlineData(200, 119)]
        [InlineData(42, 99)]
        public void InvalidPointsTest(int x, int y)
        {
            var f = new Field(223);
            var c = new Curve(f.Element(0), f.Element(7));
            Assert.Throws<ArithmeticException>(() => c.Point(f.Element(x), f.Element(y)));
        }

        [Theory]
        [InlineData(170, 142, 192, 105, 17, 56)]
        [InlineData(60, 139, 47, 71, 117, 141)]
        [InlineData(47, 71, 143, 98, 76, 66)]
        public void AddTest(int x3, int y3, int x1, int y1, int x2, int y2)
        {
            var f = new Field(223);
            var c = new Curve(f.Element(0), f.Element(7));
            var p1 = c.Point(f.Element(x1), f.Element(y1));
            var p2 = c.Point(f.Element(x2), f.Element(y2));
            Assert.Equal(c.Point(f.Element(x3),f.Element(y3)), p1 + p2);
        }
    }
}
