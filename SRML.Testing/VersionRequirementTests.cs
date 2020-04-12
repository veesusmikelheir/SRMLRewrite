using System;
using NUnit.Framework;

namespace SRML.Testing
{
    [TestFixture]
    public class VersionRequirementTests
    {
        [Test]
        public void VersionRequirement_Parse_Normal()
        {
            var expected = new VersionRequirement(Version.Parse("1.0"), CompareType.EqualTo);
            Assert.AreEqual(expected, VersionRequirement.Parse("==1.0"));
            Assert.AreEqual(expected, VersionRequirement.Parse("1.0"));

            expected = new VersionRequirement(Version.Parse("1.0"), CompareType.GreaterThan);
            Assert.AreEqual(expected, VersionRequirement.Parse(">1.0"));

            expected = new VersionRequirement(Version.Parse("1.0"), CompareType.LessThan);
            Assert.AreEqual(expected, VersionRequirement.Parse("<1.0"));

            expected = new VersionRequirement(Version.Parse("1.0"), CompareType.LessThan | CompareType.EqualTo);
            Assert.AreEqual(expected, VersionRequirement.Parse("<=1.0"));


            expected = new VersionRequirement(Version.Parse("1.0"), CompareType.GreaterThan | CompareType.EqualTo);
            Assert.AreEqual(expected, VersionRequirement.Parse(">=1.0"));
        }

        [Test]
        public void VersionRequirement_Parse_Null_And_Empty()
        {
            Assert.Throws<ArgumentNullException>(() => VersionRequirement.Parse(null));
            Assert.Throws<ArgumentException>(() => VersionRequirement.Parse(""));
        }
    }
}
