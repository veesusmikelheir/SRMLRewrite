using IniParser.Model;
using IniParser.Parser;
using NUnit.Framework;
using SRML.Config;
using SRML.Config.Reflection;
using SRML.Config.Reflection.Attributes;
using System;
using System.IO;

namespace SRML.Testing
{
    [TestFixture]
    public class ReflectionConfigGeneratorTests
    {
        ReflectionConfigGenerator generator;
        [SetUp]
        public void Setup()
        {
            generator = new ReflectionConfigGenerator();
        }

        [TearDown]
        public void Teardown()
        {
            ConfigTest.Test = "yes";
            ConfigTest.Test2 = 1;
            ConfigTest.Testing = new long[]{ 1, 23, 4 };
            ConfigTest.Internal.Test3 = "yes2";
            ConfigTest.Internal.StringArray = new string[] { "test", "tost" };
        }

        [Test]
        public void ReflectionConfigGenerator_GenerateConfigFile_GenerateTest()
        {
            var file = generator.GenerateConfigFile(new ReflectionConfigGenerator.GeneratorConfig() { Type = typeof(ConfigTest) });
            var data = new IniData();
            file.PushTo(data);
            Console.WriteLine(data.ToString());
            Assert.AreEqual(1,file["GLOBAL"]["Test2"].Value);
            Assert.AreEqual("yes2", file["Internal"]["Test3"].Value);
        }

        [Test]
        public void ReflectionConfigGenerator_GenerateConfigFile_SetValues()
        {
            var file = generator.GenerateConfigFile(new ReflectionConfigGenerator.GeneratorConfig() { Type = typeof(ConfigTest) });

            file["GLOBAL"]["Test"].Value = "no";
            Assert.AreEqual("no", ConfigTest.Test);
        }

        [Test]
        public void ReflectionConfigGenerator_GenerateConfigFile_Invalid_Cast()
        {
            var file = generator.GenerateConfigFile(new ReflectionConfigGenerator.GeneratorConfig() { Type = typeof(ConfigTest) });

            Assert.Throws<InvalidCastException>(() => file["GLOBAL"]["Test2"].Value = null);

        }

        [Test]
        public void ReflectionConfigGenerator_GenerateConfigFile_LoadFromFile()
        {
            var file = generator.GenerateConfigFile(new ReflectionConfigGenerator.GeneratorConfig() { Type = typeof(ConfigTest) });
            var text = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "/config.ini");
            var parser = new IniParser.Parser.IniDataParser();
            var data = parser.Parse(text);
            file.PullFrom(data);
            Assert.AreEqual(" ",ConfigTest.Test);
            Assert.AreEqual(0, ConfigTest.Testing.Length);
            Assert.AreEqual(112, ConfigTest.Test2);
        }

        [Test]
        public void ReflectionConfigGenerator_GenerateConfigFile_CustomNameAttribute()
        {
            var file = generator.GenerateConfigFile(new ReflectionConfigGenerator.GeneratorConfig() { Type = typeof(DecoratedConfigTest) });

            Assert.NotNull(file["GLOBAL"]["FriendlyName"]);
            Assert.AreEqual(file["GLOBAL"]["FriendlyName"].Value, "yea");
        }

        [Test]
        public void FieldBackedConfigValue_StandIn_Test()
        {
            var field = typeof(TestStandIn).GetField("Test");
            var element = new FieldBackedConfigValue(field, null);
            element.Value = "test";
            Assert.AreEqual("test", TestStandIn.Test.Value);
            Assert.AreEqual(typeof(string), element.ValueType);
            Assert.AreEqual("test", element.Value);
        }
    }

    public class TestStandIn : IValueStandIn
    {
        public object Value { get; set; }

        public Type ValueType => typeof(string);

        public static TestStandIn Test = new TestStandIn();
    }

    public static class ConfigTest
    {
        public static string Test = "yes";
        public static int Test2 = 1;
        public static long[] Testing = { 1, 23, 4 };
        public static class Internal
        {
            public static string Test3 = "yes2";
            public static string[] StringArray = { "test", "tost" };
        }
    }


    public static class DecoratedConfigTest
    {
        [ConfigName("FriendlyName")]
        public static string I_DONT_WANT_THIS_WRITTEN = "yea";
    }
}
