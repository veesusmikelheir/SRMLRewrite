using NUnit.Framework;
using SRML.ModLoading;
using SRML.ModLoading.Core;
using SRML.ModLoading.Core.Integrity;
using SRML.ModLoading.Core.Parsers;
using System;

namespace SRML.Testing.ModLoading
{
    [TestFixture]
    public class ModLoadingIntegrationTest
    {
        ModPipeline pipeline;

        [SetUp]
        public void Setup()
        {
            pipeline = new ModPipeline(new FileSystemModLocator(), new AggregateModParser(new ModInfoJsonParser(),new DLLManifestJsonParser()), new EntryPointModLoader(), new AggregateIntegrityChecker(new DuplicateIntegrityChecker(), new DependencyIntegrityChecker()), new BasicAssemblyResolver(), new LoadOrderCalculator());
        }

        [Test]
        public void LoadMods()
        {
            pipeline.InitializeMods(TestContext.CurrentContext.TestDirectory+"/TestModRoot/");
            foreach(var mod in pipeline.Mods)
            {
                Console.WriteLine(mod.Info.ID);
            }
        }
    }
}
