using NUnit;
using NUnit.Framework;
using SRML.Config;
using System;

namespace SRML.Testing
{
    [TestFixture]
    public class RangeTests
    {
        [Test]
        public void Range_Max_Test()
        {
            var range = new Range<int>(0, 100);
            range.Value = 200;
            Assert.AreEqual(range.Value, 100);
        }

        [Test]
        public void Range_Min_Test()
        {
            var range = new Range<int>(0, 100);
            range.Value = -100;
            Assert.AreEqual(range.Value, 0);
        }

        [Test]
        public void Range_NoBinaryOperator_Test()
        {
            Assert.Throws<InvalidOperationException>(()=>new Range<object>(new object(), new object()));
        }
    }
}
