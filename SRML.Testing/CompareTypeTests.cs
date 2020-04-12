using System;

using NUnit.Framework;

namespace SRML.Testing
{
    [TestFixture]
    public class CompareTypeTests
    {
        [TestCase("==",CompareType.EqualTo)]
        [TestCase("<",CompareType.LessThan)]
        [TestCase(">",CompareType.GreaterThan)]
        [TestCase("<=", CompareType.LessThan | CompareType.EqualTo)]
        [TestCase(">=", CompareType.GreaterThan | CompareType.EqualTo)]
        public void CompareType_Parse_NormalInput(string input, CompareType expected)
        {
            Assert.AreEqual(CompareTypeExtensions.Parse(input), expected);
        }

        [Test]
        public void CompareType_Parse_Empty()
        {

            Assert.Throws<ArgumentException>(() => CompareTypeExtensions.Parse(""));

        }

        [Test]
        public void CompareType_Parse_Null()
        {

            Assert.Throws<ArgumentNullException>(() => CompareTypeExtensions.Parse(null));

        }

        [Test]
        public void CompareType_Parse_Big()
        {
            Assert.Throws<ArgumentException>(() => CompareTypeExtensions.Parse("aasdasdasd"));

        }

        [TestCase(CompareType.EqualTo, "==")]
        [TestCase(CompareType.GreaterThan, ">")]
        [TestCase(CompareType.LessThan, "<")]
        [TestCase(CompareType.GreaterThan | CompareType.EqualTo, ">=")]
        [TestCase(CompareType.LessThan | CompareType.EqualTo, "<=")]
        public void CompareType_GetString_Normal(CompareType type, string expected)
        {
            Assert.AreEqual((type).GetString(), expected);
            

        }

        [Test]
        public void CompareType_GetString_Invalid_None()
        {
            Assert.Throws<ArgumentOutOfRangeException>(()=>(CompareType.NONE).GetString());
           


        }

        [Test]
        public void CompareType_GetString_Invalid_BothGreater_And_LessThan()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => (CompareType.LessThan | CompareType.GreaterThan).GetString());
        }
    }
}
