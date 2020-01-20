using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class DoublePointTests
    {
        readonly DoubleCurve c = new DoubleCurve(5, 7);

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
            Assert.Equal(p1, c.Point(-1, -1));
            Assert.NotEqual(p1, p2);
            Assert.Equal(c.Infinity, c.Infinity);
            Assert.NotEqual(c.Infinity, p1);
        }

        [Fact]
        public void AddIdentityTest()
        {
            var p1 = c.Point(-1, -1);
            Assert.Equal(p1, p1 + c.Infinity);
            Assert.Equal(p1, c.Infinity + p1);
        }

        [Fact]
        public void AddInverseTest()
        {
            var p1 = c.Point(-1, -1);
            var p2 = c.Point(-1, 1);
            Assert.Equal(c.Infinity, p1 + p2);
        }

        [Fact]
        public void AddTest()
        {
            var p1 = c.Point(-1, -1);
            var p2 = c.Point(2, 5);
            var result = c.Point(3, -7);
            Assert.Equal(result, p1 + p2);
            Assert.Equal(result, p2 + p1);
        }

        [Fact]
        public void AddSameTest()
        {
            var p1 = c.Point(-1, 1);
            var result = c.Point(18, -77);
            Assert.Equal(result, p1 + p1);
        }
    }
}
