using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SRML.Testing
{
    [TestFixture]
    public class VersionRequirementTests
    {
        [TestCase(CompareType.EqualTo, "==")]
        [TestCase(CompareType.LessThan, "<")]
        [TestCase(CompareType.GreaterThan, ">")]
        [TestCase(CompareType.LessThan | CompareType.EqualTo, "<=")]
        [TestCase(CompareType.GreaterThan | CompareType.EqualTo, ">=")]
        public void VersionRequirement_Parse_NormalInputs(CompareType type, string comparerString)
        {

            var expected = new VersionRequirement(Version.Parse("1.0"), type);
            Assert.AreEqual(expected, VersionRequirement.Parse($"{comparerString}1.0"));


        }

        [Test]
        public void VersionRequirement_Parse_NoComparer()
        {
            var expected = new VersionRequirement(Version.Parse("1.0"), CompareType.EqualTo);
            Assert.AreEqual(expected, VersionRequirement.Parse("1.0"));

        }
        [Test]
        public void VersionRequirement_Parse_Null()
        {
            Assert.Throws<ArgumentNullException>(() => VersionRequirement.Parse(null));

        }

        [Test]
        public void VersionRequirement_Parse_Empty()
        {
            Assert.Throws<ArgumentException>(() => VersionRequirement.Parse(""));
        }


        [Test]
        public void VersionRequirement_Compare_Null()
        {
            Assert.Throws<ArgumentNullException>(() => VersionRequirement.Compare(null, null, CompareType.EqualTo));
            Version valid = Version.Parse("1.0");
            Assert.Throws<ArgumentNullException>(() => VersionRequirement.Compare(valid, null, CompareType.EqualTo));
            Assert.Throws<ArgumentNullException>(() => VersionRequirement.Compare(null, valid, CompareType.EqualTo));
        }
        [TestCase(CompareType.NONE)]
        [TestCase(CompareType.LessThan | CompareType.GreaterThan)]
        public void VersionRequirement_Compare_Invalid_Compare(CompareType type)
        {
            var version = Version.Parse("1.0");
            Assert.Throws<ArgumentException>(() => VersionRequirement.Compare(version, version, type));
        }

        [TestCase("1.0", "1.0", CompareType.EqualTo, true)]
        [TestCase("1.0", "1.0", CompareType.EqualTo | CompareType.GreaterThan, true)]
        [TestCase("1.0", "1.0", CompareType.EqualTo | CompareType.LessThan, true)]
        [TestCase("1.0", "1.0", CompareType.GreaterThan, false)]
        [TestCase("1.0", "1.0", CompareType.LessThan, false)]
        [TestCase("1.0", "1.1", CompareType.EqualTo, false)]
        [TestCase("1.0", "1.1", CompareType.EqualTo | CompareType.GreaterThan, false)]
        [TestCase("1.0", "1.1", CompareType.EqualTo | CompareType.LessThan, true)]
        [TestCase("1.0", "1.1", CompareType.GreaterThan, false)]
        [TestCase("1.0", "1.1", CompareType.LessThan, true)]
        [TestCase("1.1", "1.0", CompareType.EqualTo, false)]
        [TestCase("1.1", "1.0", CompareType.EqualTo | CompareType.GreaterThan, true)]
        [TestCase("1.1", "1.0", CompareType.EqualTo | CompareType.LessThan, false)]
        [TestCase("1.1", "1.0", CompareType.GreaterThan, true)]
        [TestCase("1.1", "1.0", CompareType.LessThan, false)]
        public void VersionRequirement_Compare_Valid_Inputs(string a, string b, CompareType type,bool expected)
        {
            Assert.AreEqual(VersionRequirement.Compare(Version.Parse(a), Version.Parse(b), type), expected);
        }
        [Test]
        public void VersionRequirement_Compare_SatisfiedBy_Null_Input()
        {
            var validRequirement = new VersionRequirement(Version.Parse("1.0"), CompareType.EqualTo);
            Assert.Throws<ArgumentNullException>(() => validRequirement.SatisfiedWith(null));
        }

        // the version we're comparing to is 1.0, so write the testcases with that in mind
        [TestCase("1.0",CompareType.EqualTo,true)]
        [TestCase("1.0",CompareType.EqualTo | CompareType.LessThan,true)]
        [TestCase("1.0",CompareType.EqualTo | CompareType.GreaterThan,true)]
        [TestCase("1.0", CompareType.LessThan, false)]
        [TestCase("1.0", CompareType.GreaterThan, false)]
        [TestCase("1.1", CompareType.EqualTo, false)]
        [TestCase("1.1", CompareType.EqualTo | CompareType.LessThan, false)]
        [TestCase("1.1", CompareType.EqualTo | CompareType.GreaterThan, true)]
        [TestCase("1.1", CompareType.LessThan, false)]
        [TestCase("1.1", CompareType.GreaterThan, true)]
        [TestCase("0.1", CompareType.EqualTo, false)]
        [TestCase("0.1", CompareType.EqualTo | CompareType.LessThan, true)]
        [TestCase("0.1", CompareType.EqualTo | CompareType.GreaterThan, false)]
        [TestCase("0.1", CompareType.LessThan, true)]
        [TestCase("0.1", CompareType.GreaterThan, false)]
        public void VersionRequirement_Compare_SatisfiedBy_NormalInputs(string testedVersion, CompareType type, bool expected)
        {
            var requirement = new VersionRequirement(Version.Parse("1.0"), type);
            Assert.AreEqual(requirement.SatisfiedWith(Version.Parse(testedVersion)), expected);
        }
    }
}
