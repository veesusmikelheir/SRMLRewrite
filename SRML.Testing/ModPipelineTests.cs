using System;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace SRML.Testing
{
    [TestFixtureSource("Cases")]
    public class ModPipelineTests
    {
        static object[] Cases =
        {
            new object[]{new string[][]{new string[] { "test" }, new string[] { "test2" } } }
        };

        string[][] filesToTest;
        Mock<IModLocator> locator;
        Mock<IModParser> parser;
        Mock<IModLoader> loader;
        Mock<IModIntegrityChecker> integrity;
        Mock<IAssemblyResolver> resolver;
        Mock<ILoadOrderCalculator> loadorder;
        
        Mock<IModLoadingDomain> fakeDomain;

        ModPipeline pipeline;

        delegate bool ParseDel(IModFileSystem files, out IModInfo info);
        delegate bool LoadDel(IModInfo info, IModFileSystem system, IModLoadingDomain domain, out IMod mod);
        public ModPipelineTests(string[][] files)
        {
            filesToTest = files;
            // return fake file systems based on the input "files"
            fakeDomain = new Mock<IModLoadingDomain>();
            var fileSystems = files.Select(x => new FakeFileSystem(x));
            fakeDomain.Setup(x => x.ModFiles).Returns(fileSystems);
            
            
            locator = new Mock<IModLocator>();
            locator.Setup(x => x.LocateMods(It.IsAny<String>())).Returns(fakeDomain.Object);

            parser = new Mock<IModParser>();
            parser.Setup(x => x.TryParse(It.IsAny<FakeFileSystem>(), out It.Ref<IModInfo>.IsAny)).Returns(
                new ParseDel((IModFileSystem system, out IModInfo info) =>
            {
                info = null;
                if (system.ModFiles.Count() == 0) return false;
                // create a modinfo that keeps track of the file system it belongs too for later verification
                info = new FakeModInfo(system);
                return true;
            }));
           

            integrity = new Mock<IModIntegrityChecker>();


            resolver = new Mock<IAssemblyResolver>();

            loader = new Mock<IModLoader>();
            loader.Setup(x => x.TryLoad(It.IsAny<FakeModInfo>(), It.IsAny<FakeFileSystem>(), fakeDomain.Object, out It.Ref<IMod>.IsAny)).Returns(new LoadDel(
                (IModInfo info, IModFileSystem fileSystem, IModLoadingDomain domain, out IMod mod) =>
                {
                    mod = new Mock<IMod>().Object; // it really does not matter what this returns
                    Assert.AreEqual((info as FakeModInfo).system, fileSystem); // make sure the systems match
                    return true;
                }
                ));
            loadorder = new Mock<ILoadOrderCalculator>();
            loadorder.Setup(x => x.CalculateLoadOrder(It.IsAny<IEnumerable<IMod>>())).Returns(new Func<IEnumerable<IMod>, IEnumerable<IMod>>(x => x));
            pipeline = new ModPipeline(locator.Object, parser.Object, loader.Object, integrity.Object, resolver.Object, loadorder.Object);
        }

        [Test]
        public void Test()
        {
            pipeline.InitializeMods("none");
            integrity.Verify(x => x.CheckForValidity(It.IsAny<IEnumerable<IModInfo>>()), Times.AtLeastOnce());
            resolver.Verify(x => x.Initialize(fakeDomain.Object), Times.AtLeastOnce());
            loadorder.Verify(x => x.CalculateLoadOrder(It.IsAny<IEnumerable<IMod>>()), Times.AtLeastOnce());
            Assert.AreEqual(filesToTest.Length, pipeline.Mods.Count());

        }
        
        class FakeFileSystem : IModFileSystem
        { 
            string[] files;
            public FakeFileSystem(string[] files)
            {
                this.files = files;
            }

            public IEnumerable<string> ModFiles => files;
        }

        class FakeModInfo : IModInfo
        {
            public IModFileSystem system;
            public FakeModInfo(IModFileSystem associatedSystem)
            {
                system = associatedSystem;
            }
            public string ID => throw new NotImplementedException();

            public Version Version => throw new NotImplementedException();

            public Version APIVersion => throw new NotImplementedException();

            public IModMetaData MetaData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public IModDependency[] Dependencies => throw new NotImplementedException();

            public IModLoadOrder LoadOrder => throw new NotImplementedException();
        }
    }
}
