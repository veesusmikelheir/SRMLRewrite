using System;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace SRML.Testing
{
    [TestFixture]
    public class ModPipelineTests
    {
        


        IModLocator locator;
        IModParser parser;
        IModLoader loader;
        Mock<IModIntegrityChecker> integrity;
        Mock<IAssemblyResolver> resolver;
        Mock<IDisposable> resolverDisposeMock;
        Mock<ILoadOrderCalculator> loadorder;

        int resolveCount = 0;
        ModPipeline pipeline;

        delegate bool ParseDel(IModFileSystem files, out IModInfo info);
        delegate bool LoadDel(IModInfo info, IModFileSystem system, IModLoadingDomain domain, out IMod mod);
        [SetUp]
        public void Setup()
        {

            // return fake file systems based on the input "files"


            
            locator = new FakeModLoactor();
            resolveCount = 0;
            parser = new FakeModParser();
            loader = new FakeModLoader();

            integrity = new Mock<IModIntegrityChecker>();

            resolverDisposeMock = new Mock<IDisposable>();
            
            resolver = new Mock<IAssemblyResolver>();

            resolver.Setup(x => x.Initialize(It.IsAny<IModLoadingDomain>())).Returns(resolverDisposeMock.Object).Callback(()=>resolveCount++);

            loadorder = new Mock<ILoadOrderCalculator>();
            loadorder.Setup(x => x.CalculateLoadOrder(It.IsAny<IEnumerable<IMod>>())).Returns(new Func<IEnumerable<IMod>, IEnumerable<IMod>>(x => x));
            pipeline = new ModPipeline(locator, parser, loader, integrity.Object, resolver.Object, loadorder.Object);
        }

        void CommonAssertions()
        {
            integrity.Verify(x => x.CheckForValidity(It.IsAny<IEnumerable<IModInfo>>()), Times.AtLeastOnce());
            resolver.Verify(x => x.Initialize(It.IsAny<IModLoadingDomain>()), Times.AtLeastOnce());
            resolverDisposeMock.Verify(x => x.Dispose(), Times.Exactly(resolveCount));
            loadorder.Verify(x => x.CalculateLoadOrder(It.IsAny<IEnumerable<IMod>>()), Times.AtLeastOnce());
        }

        [Test]
        public void Pipeline_InitializeMods_GoodInput()
        {
            pipeline.InitializeMods("normal;normal2");
            CommonAssertions();
            Assert.AreEqual(2, pipeline.Mods.Count());

        }

        [Test]
        public void Pipeline_InitializeMods_ParseFailure()
        {
            Assert.Throws<Exception>(()=>pipeline.InitializeMods(""));
            
            

        }

        [Test]
        public void Pipeline_InitializeMods_LoadFailure()
        {
            Assert.Throws<Exception>(() => pipeline.InitializeMods("fail"));



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
            public string ID => "testMod";

            public Version Version => throw new NotImplementedException();

            public Version APIVersion => throw new NotImplementedException();

            public IModMetaData MetaData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public IModDependency[] Dependencies => throw new NotImplementedException();

            public IModLoadOrder LoadOrder => throw new NotImplementedException();
        }

        class FakeModParser : IModParser
        {
            public bool TryParse(IModFileSystem loadInfo, out IModInfo modInfo)
            {
                modInfo = new FakeModInfo(loadInfo);
                
                return !loadInfo.ModFiles.Any(x=>x==""); // return false if one of the "files" is empty, in order to simulate a parsing failure (it being empty has no significance outside of signaling to the test to throw a false)
            }
        }

        class FakeModLoader : IModLoader
        {
            public bool TryLoad(IModInfo info, IModFileSystem modfiles, IModLoadingDomain loadingdomain, out IMod mod)
            {
                mod = new Mock<IMod>().Object; // really doesn't matter what we return
                Assert.AreEqual((info as FakeModInfo).system, modfiles); // make sure the proper system is being passed along with each mod
                Assert.NotNull(loadingdomain); // make sure the domain is a thing
                return !modfiles.ModFiles.Any(x => x == "fail"); // if a "file" is named fail, thats the test signaling to us that we want to simulate a failed mod load
            }
        }

        class FakeModLoactor : IModLocator
        {
            public IModLoadingDomain LocateMods(string coredirectory)
            {
                var fakes = new List<IModFileSystem>();
                foreach(var list in coredirectory.Split(';')) // we split the file systems we want to put in using semicolons
                {
                    fakes.Add(new FakeFileSystem(list.Split(','))); // split the system based on commas
                }
                return new FakeModLoadingDomain(fakes);
            }
            public class FakeModLoadingDomain : IModLoadingDomain
            {
                public List<IModFileSystem> files;

                public FakeModLoadingDomain(List<IModFileSystem> files)
                {
                    this.files = files;
                }

                public IEnumerable<IModFileSystem> ModFiles => files;

                public IEnumerable<string> AllFiles => throw new NotImplementedException();
            }
        }
    }
}
