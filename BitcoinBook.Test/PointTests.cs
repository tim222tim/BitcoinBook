using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class PointTests
    {
        readonly Field field = new Field(223);
        readonly Curve curve;

        public PointTests()
        {
            curve = new Curve(field, 0, 7);
        }

        [Theory]
        [InlineData(192,105)]
        [InlineData(17, 56)]
        [InlineData(1, 193)]
        public void ValidPointsTest(int x, int y)
        {
            curve.Point(x, y);
        }

        [Theory]
        [InlineData(200, 119)]
        [InlineData(42, 99)]
        public void InvalidPointsTest(int x, int y)
        {
            Assert.Throws<ArithmeticException>(() => curve.Point(x, y));
        }

        [Theory]
        [InlineData(170, 142, 192, 105, 17, 56)]
        [InlineData(60, 139, 47, 71, 117, 141)]
        [InlineData(47, 71, 143, 98, 76, 66)]
        public void AddTest(int x3, int y3, int x1, int y1, int x2, int y2)
        {
            var p1 = curve.Point(x1, y1);
            var p2 = curve.Point(x2, y2);
            Assert.Equal(curve.Point(x3, y3), p1 + p2);
        }
    }
}
