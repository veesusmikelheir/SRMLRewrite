using NUnit.Framework;
using SRML.Utils;
using System;

namespace SRML.Testing
{
    [TestFixture]
    public class ReflectionUtilTests
    {
        [TestCase(1,typeof(int),true)]
        [TestCase(1,typeof(string),false)]
        [TestCase(null, typeof(int), false)]
        [TestCase(" ", typeof(int), false)]
        [TestCase(null, typeof(Test),false)]

        public void ReflectionUtil_IsCompatible_Test(object toTest, Type type, bool expected)
        {
            Assert.AreEqual(expected, ReflectionUtil.IsCompatible(toTest, type));
        }

        [Test]
        public void ReflectionUtil_IsCompatible_Boxing_Test()
        {
            ToBox box = new Test();
            Assert.IsTrue(ReflectionUtil.IsCompatible(box, typeof(Test)));
        }
        [TestCase(1,1,false)]
        [TestCase(2,1,true)]
        public void ReflectionUtil_GetGreaterThan_Normal_Test(int param1, int param2, bool expected)
        {
            var del = ReflectionUtil.GetGreaterThan<int>();
            Assert.NotNull(del);
            Assert.AreEqual(expected, del(param1, param2));
        }

        [Test]
        public void ReflectionUtil_GetGreaterThan_Fail_Test()
        {
            Assert.Throws<InvalidOperationException>(()=>ReflectionUtil.GetGreaterThan<object>());
            
        }
    }

    public interface ToBox { }
    public struct Test : ToBox
    {

    }
}
