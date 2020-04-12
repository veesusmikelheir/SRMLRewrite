using System;
using System.IO;
using NUnit.Framework;
using SRML.ModLoading.API;
using SRML.ModLoading.Core;

namespace SRML.Testing
{
    [TestFixture]
    public class JsonParserTests
    {

        IModInfo LoadJSONFile(string filename)
        {

            string json = File.ReadAllText(TestContext.CurrentContext.TestDirectory + $"/JSONSamples/{filename}.json");
            var info = JsonModParser.ParseJSON(json);
            return info;
        }

        void NotNullAsserts(IModInfo info)
        {
            Assert.NotNull(info);
            Assert.NotNull(info.MetaData);
            Assert.NotNull(info.MetaData.Name);
            Assert.NotNull(info.MetaData.Author);
            Assert.NotNull(info.MetaData.Description);
            Assert.NotNull(info.Dependencies);
            Assert.NotNull(info.LoadOrder);
            Assert.NotNull(info.LoadOrder.LoadAfter);
            Assert.NotNull(info.LoadOrder.LoadBefore);
            Assert.NotNull(info.APIVersion);
            Assert.NotNull(info.Version);
        }

        [Test]
        public void JsonParser_ParseJSON_MinimumInput_NoNulls()
        {
            var info = LoadJSONFile("minimum");


            NotNullAsserts(info);
            
        }


        [Test]
        public void JsonParser_ParseJSON_MinimumInput_Correct_Mandatory_Fields()
        {
            var info = LoadJSONFile("minimum");


            Assert.AreEqual(info.ID, "test");
            Assert.AreEqual(info.MetaData.Name, "name");
            Assert.AreEqual(info.Version, Version.Parse("1.0"));

        }


       
            

        [Test]
        public void JsonParser_ParseJSON_Default_API_Version()
        {
            var info = LoadJSONFile("minimum");

            Assert.AreEqual(info.APIVersion, typeof(JsonModParser).Assembly.GetName().Version);
            
        }

        [Test]
        public void JsonParser_ParseJSON_Missing_ID()
        {
            Assert.Throws<ArgumentNullException>(()=>LoadJSONFile("missingid"),"id");



        }
        [Test]
        public void JsonParser_ParseJSON_Missing_Version()
        {
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("missingversion"), "version");



        }

        [Test]
        public void JsonParser_ParseJSON_Missing_Name()
        {
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("missingname"), "name");



        }

        public void JsonParser_ParseJSON_Null_ID()
        {
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("nullid"), "id");



        }
        [Test]
        public void JsonParser_ParseJSON_Null_Version()
        {
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("nullversion"), "version");



        }

        [Test]
        public void JsonParser_ParseJSON_Null_Name()
        {
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("nullname"), "name");



        }



        [Test]
        public void JsonParser_ParseJSON_Dependency_Normal ()
        {
            var info = LoadJSONFile("dependencynormal");

            Assert.AreEqual(info.Dependencies.Length, 2);

            Assert.AreEqual(info.Dependencies[0].ID, "test2");
            var expected = new VersionRequirement(Version.Parse("1.0"), CompareType.GreaterThan);
            Assert.AreEqual(info.Dependencies[0].VersionRequirement, expected);

            Assert.AreEqual(info.Dependencies[1].ID, "test4");
            expected = new VersionRequirement(Version.Parse("1.3.2"), CompareType.GreaterThan | CompareType.EqualTo);
            Assert.AreEqual(info.Dependencies[1].VersionRequirement, expected);
        }

        [Test]
        public void JsonParser_ParseJSON_Dependency_Duplicate()
        {
            Assert.Throws<ArgumentException>(()=>LoadJSONFile("dependencyduplicate"));

        }

        [Test]
        public void JsonParser_ParseJSON_Dependency_Null_ID()
        {
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("dependencynullid"));

        }

        [Test]
        public void JsonParser_ParseJSON_Dependency_Missing_ID()
        {
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("dependencymissingid"));

        }

        [Test]
        public void JsonParser_ParseJSON_Dependency_Missing_Version()
        {
            
            Assert.Throws<ArgumentNullException>(() => LoadJSONFile("dependencymissingversion"));

        }

        [Test]
        public void JsonParser_ParseJSON_Null_Json()
        {

            Assert.Throws<ArgumentNullException>(() => JsonModParser.ParseJSON(null));

        }

        [Test]
        public void JsonParser_ParseJSON_Empty_Json()
        {

            Assert.Throws<ArgumentException>(() => JsonModParser.ParseJSON(""));

        }
    }

}
