using System;
using System.Collections.Generic;
using NUnit.Framework;
using SRML.ModLoading.API;
using SRML.ModLoading.Core;
using SRML.ModLoading.Core.Integrity;

namespace SRML.Testing.ModLoading.Integrity
{
    [TestFixture]
    public class DependencyIntegrityTests
    {
        DependencyIntegrityChecker checker;

        [SetUp]
        public void Setup()
        {
            checker = new DependencyIntegrityChecker();
        }

        [Test]
        public void DependencyIntegrityChecker_CheckValidity_No_Dependencies()
        {
            var mods = new List<IModInfo>()
            {
                new DepModInfo("test","1.1"),
                new DepModInfo("test2","1.3")
            };
            Assert.DoesNotThrow(() => checker.CheckForValidity(mods));
        }

        [Test]
        public void DependencyIntegrityChecker_CheckValidity_One_Missing_Dependencies()
        {
            var mods = new List<IModInfo>()
            {
                new DepModInfo("test","1.1",new Dependency("test3",">=1.0")),
                new DepModInfo("test2","1.3")
            };
            Assert.Throws<Exception>(() => checker.CheckForValidity(mods));
        }

        [Test]
        public void DependencyIntegrityChecker_CheckValidity_Wrong_Version_Dependency()
        {
            var mods = new List<IModInfo>()
            {
                new DepModInfo("test","1.1",new Dependency("test2","<1.0")),
                new DepModInfo("test2","1.3")
            };
            Assert.Throws<Exception>(() => checker.CheckForValidity(mods));
        }

        [Test]
        public void DependencyIntegrityChecker_CheckValidity_Correct_Dependency()
        {
            var mods = new List<IModInfo>()
            {
                new DepModInfo("test","1.1",new Dependency("test2",">=1.0")),
                new DepModInfo("test2","1.3")
            };
            Assert.DoesNotThrow(() => checker.CheckForValidity(mods));
        }
        [Test]
        public void DependencyIntegrityChecker_CheckValidity_Correct_Dependency_And_Incorrect()
        {
            var mods = new List<IModInfo>()
            {
                new DepModInfo("test","1.1",new Dependency("test2",">=1.0"),new Dependency("test5","<=1.0")),
                new DepModInfo("test2","1.3")
            };
            Assert.Throws<Exception>(() => checker.CheckForValidity(mods));
        }

        [Test]
        public void DependencyIntegrityChecker_CheckValidity_Duplicate_Dependency()
        {
            var mods = new List<IModInfo>()
            {
                new DepModInfo("test","1.1",new Dependency("test2",">=1.0"),new Dependency("test2",">=1.1")),
                new DepModInfo("test2","1.3")
            };
            Assert.Throws<Exception>(() => checker.CheckForValidity(mods));
        }

    }

    public class DepModInfo : IModInfo
    {
        public string ID { get; set; }

        public Version Version { get; set; }

        public Version APIVersion => throw new NotImplementedException();

        public IModMetaData MetaData => throw new NotImplementedException();

        public IModDependency[] Dependencies { get; set; } = new IModDependency[0];

        public IModLoadOrder LoadOrder => throw new NotImplementedException();

        public DepModInfo(string iD, string version, params IModDependency[] dependencies) : this(iD, version)
        {
            Dependencies = dependencies;
        }

        public DepModInfo(string iD, string version)
        {
            ID = iD;
            Version = Version.Parse(version);
        }
    }

    public class Dependency : IModDependency
    {
        public string ID { get; set; }

        public VersionRequirement VersionRequirement { get; set; }

        public Dependency(string iD, string version)
        {
            ID = iD;
            VersionRequirement = VersionRequirement.Parse(version);
        }
    }
}
