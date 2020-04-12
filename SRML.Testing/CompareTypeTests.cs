using System;

using NUnit.Framework;

namespace SRML.Testing
{
    [TestFixture]
    public class CompareTypeTests
    {
        [Test]
        public void CompareType_Parse_NormalInput()
        {
            var type = CompareTypeExtensions.Parse("==");
            Assert.AreEqual(CompareType.EqualTo, type);

            type = CompareTypeExtensions.Parse(">");
            Assert.AreEqual(CompareType.GreaterThan, type);

            type = CompareTypeExtensions.Parse("<");
            Assert.AreEqual(CompareType.LessThan, type);

            type = CompareTypeExtensions.Parse("<=");
            Assert.AreEqual(CompareType.LessThan | CompareType.EqualTo, type);

            type = CompareTypeExtensions.Parse(">=");
            Assert.AreEqual(CompareType.GreaterThan | CompareType.EqualTo, type);
        }

        [Test]
        public void CompareType_Parse_Null_And_Empty()
        {
            
            Assert.Throws<ArgumentException>(() => CompareTypeExtensions.Parse(""));
            Assert.Throws<ArgumentNullException>(() => CompareTypeExtensions.Parse(null));
        }
        [Test]
        public void CompareType_Parse_Big()
        {
            Assert.Throws<ArgumentException>(() => CompareTypeExtensions.Parse("aasdasdasd"));

        }

        [Test]
        public void CompareType_GetString_Normal()
        {
            Assert.AreEqual((CompareType.EqualTo).GetString(), "==");
            Assert.AreEqual((CompareType.LessThan).GetString(), "<");
            Assert.AreEqual((CompareType.GreaterThan).GetString(), ">");
            Assert.AreEqual((CompareType.GreaterThan | CompareType.EqualTo).GetString(), ">=");
            Assert.AreEqual((CompareType.LessThan | CompareType.EqualTo).GetString(), "<=");

        }

        [Test]
        public void CompareType_GetString_Invalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(()=>(CompareType.NONE).GetString());
            Assert.Throws<ArgumentOutOfRangeException>(() => (CompareType.LessThan | CompareType.GreaterThan).GetString());


        }
    }
}
