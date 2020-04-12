using System;
using System.Linq;
using NUnit.Framework;
using SRML.ModLoading.API;
using SRML.ModLoading.Core;

namespace SRML.Testing.ModLoading
{
    [TestFixture]
    public class LoadOrderCalculatorTests
    {

        LoadOrderCalculator OrderCalculator;

        [SetUp]
        public void Setup()
        {
            OrderCalculator = new LoadOrderCalculator();
        }

        [Test]
        public void LoadOrderCalculator_CalculateLoadOrder_Basic_Order_Test()
        {
            var originalSequence = new Mod[]
            {
                new Mod("test",new string[]{"test2"},new string[]{}),
                new Mod("test3",new string[]{"test2"},new string[]{"test"}),
                new Mod("test2",new string[]{},new string[]{"test"})
            };
            for (int i = 0; i < 10; i++)
            {
                var random = new Random(i);
                var toSort = originalSequence.OrderBy(x => random.Next()).ToList();
                var sorted = OrderCalculator.CalculateLoadOrder(toSort).ToList();

                Assert.IsTrue(originalSequence.SequenceEqual(sorted));
            }
        }

        [Test]
        public void LoadOrderCalculator_CalculateLoadOrder_Circular_Load_Order()
        {
            var originalSequence = new Mod[]
            {
                new Mod("test",new string[]{"test2"},new string[]{}),
                new Mod("test3",new string[]{"test2"},new string[]{"test"}),
                new Mod("test2",new string[]{"test"},new string[]{})
            };

            var toSort = originalSequence;
            Assert.Throws<Exception>(() => OrderCalculator.CalculateLoadOrder(toSort));
            
        }
    }

    public class Mod : IMod, IModInfo, IModLoadOrder
    {
        public string[] LoadsBefore { get; set; } = new string[0];

        public string[] LoadsAfter { get; set; } = new string[0];

        public string ID {get;set;}

        public Version Version => throw new NotImplementedException();

        public Version APIVersion => throw new NotImplementedException();

        public IModMetaData MetaData => throw new NotImplementedException();

        public IModDependency[] Dependencies => throw new NotImplementedException();

    public IModLoadOrder LoadOrder => this;

        public IModInfo Info => this;

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void PreLoad()
        {
            throw new NotImplementedException();
        }

        public Mod(string iD,string[] loadsBefore, string[] loadsAfter) : this(iD)
        {
            LoadsBefore = loadsBefore;
            LoadsAfter = loadsAfter;
        }

        public Mod(string iD)
        {
            ID = iD;
        }
    }
}
