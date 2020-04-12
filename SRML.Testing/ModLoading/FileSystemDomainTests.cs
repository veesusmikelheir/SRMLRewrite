using NUnit.Framework;
using SRML.ModLoading.Core;
using System;
using System.IO;
using System.Linq;

namespace SRML.Testing.ModLoading
{
    [TestFixture]
    public class FileSystemDomainTests
    {
        private const string moddirectory = "/ModLocatingDirectory/";
        FileSystemDomain Domain;
        [SetUp]
        public void Setup()
        {
            Domain = new FileSystemDomain(TestContext.CurrentContext.TestDirectory + moddirectory);
        }
        [Test]
        public void FileSystemDomain_Initialize_Valid_Count()
        {

            var modList = Domain.ModFiles.ToList();

            // found all the mods
            Assert.AreEqual(modList.Count, 4);


        }

        [Test]
        public void FileSystemDomain_Initialize_CorrectFiles()
        {
            string[][] expected =
            {
                new string[]{@"Mod\Mod.dll",@"Mod\modinfo.json"}.OrderBy(x=>x).ToArray(),
                new string[]{@"OtherMod\OtherMod.dll",@"OtherMod\Subfolder\Asset.json"}.OrderBy(x=>x).ToArray(),
                new string[]{@"AlsoFake.dll"},
                new string[]{@"Fake.dll"}
            };

            var files = Domain.ModFiles.Select(x => x.ModFiles.Select(z => z.Remove(0, Domain.CoreDirectory.Length)).OrderBy(y => y).ToArray()).ToArray();

            Assert.IsTrue(expected.All(x => files.Any(y => y.SequenceEqual(x))));
        }

        [Test]
        public void FileSystemDomain_Initialize_No_Incorrect_Files()
        {
            Assert.IsFalse(Domain.ModFiles.SelectMany(x => x.ModFiles).Any(x => x.EndsWith("DontScanThis.json")));
        }
    }
}
