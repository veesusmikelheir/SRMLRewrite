using NUnit.Framework;
using SRML.Config.Reflection;
using System;

namespace SRML.Testing
{
    [TestFixture]
    public class ReflectionStringParserTests
    {
        [TestCase(typeof(Int32))]
        [TestCase(typeof(bool))]
        [TestCase(typeof(long))]

        public void ReflectionStringParser_GetParseMethod_NormalInput(Type type)
        {
            Assert.AreEqual(type.GetMethod("Parse", new Type[] { typeof(string) }), ReflectionStringParser.GetParseMethod(type));
        }
        
    }
}
