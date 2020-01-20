using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class PointTests
    {
        readonly Curve c = new Curve(5, 7);

        [Fact]
        public void InvalidPointTest()
        {
            Assert.Throws<ArithmeticException>(() => c.Point(-1, -2));
        }

        [Fact]
        public void ValidPointTest()
        {
            c.Point(-1, -1);
        }

        [Fact]
        public void EqualsTest()
        {
            var p1 = c.Point(-1, -1);
            var p2 = c.Point(18, 77);
            Assert.False(p1.Equals(p2));
            Assert.True(p1.Equals(c.Point(-1, -1)));
        }
    }
}
