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
    }
}
