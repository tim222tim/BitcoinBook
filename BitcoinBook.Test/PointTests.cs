using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class PointTests
    {
        readonly Field field = new(223);
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

        [Fact]
        public void EqualsTest()
        {
            var p1 = curve.Point(192, 105);
            var p1B = curve.Point(192, 105);
            var p2 = curve.Point(17, 56);
            Assert.Equal(p1, p1B);
            Assert.Equal(p2, p2);
            Assert.True(p1 == p1B);
            Assert.NotEqual(p1, p2);
            Assert.True(p1 != p2);
        }

        [Theory]
        [InlineData(170, 142, 192, 105, 17, 56)]
        [InlineData(60, 139, 47, 71, 117, 141)]
        [InlineData(47, 71, 143, 98, 76, 66)]
        public void AddTest(int rx, int ry, int x1, int y1, int x2, int y2)
        {
            var p1 = curve.Point(x1, y1);
            var p2 = curve.Point(x2, y2);
            Assert.Equal(curve.Point(rx, ry), p1 + p2);
        }

        [Theory]
        [InlineData(192, 105, 192, 105, 1)]
        [InlineData(49, 71, 192, 105, 2)]
        [InlineData(64, 168, 143, 98, 2)]
        [InlineData(36, 111, 47, 71, 2)]
        [InlineData(194, 51, 47, 71, 4)]
        [InlineData(116, 55, 47, 71, 8)]
        public void MultiplyByTest(int rx, int ry, int x, int y, int m)
        {
            Assert.Equal(curve.Point(rx, ry), curve.Point(x, y).MultiplyBy(m));
        }

        [Fact]
        public void MultiplyByZeroTest()
        {
            var p = curve.Point(47, 71);
            Assert.Equal(curve.GetInfinity(), p * 0);
        }

        [Fact]
        public void MultiplyByOneTest()
        {
            var p = curve.Point(47, 71);
            Assert.Equal(p, p * 1);
        }

        [Fact]
        public void MultiplyToInfinityTest()
        {
            var p = curve.Point(47, 71);
            Assert.Equal(curve.GetInfinity(), p * 21);
        }

        [Fact]
        public void MultiplyByLotsTest()
        {
            var p = curve.Point(47, 71);
            Assert.Equal(curve.Point(36, 112), p * 1234567);
        }

        [Fact]
        public void OrderTest()
        {
            Assert.Equal(7, curve.Point(15, 86).GetOrder());
        }
    }
}
