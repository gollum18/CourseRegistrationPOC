using System.Collections.Generic;
using NUnit.Framework;
using Coursely.Content.Cache;

namespace CourselyTests.Tests.Components
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(MemoryCache<int, string>))]
    class CacheTest
    {
        private readonly int[] TestKeys = new int[]
        {
            0, 1, 2, 3,
            4, 5, 6, 7,
            8, 9, 10, 11,
            12, 13, 14, 15
        };

        private readonly string[] TestValues = new string[]
        {
            "Claptrap", "Zero", "Jack", "Brick",
            "Roland", "Lilith", "Marcus", "Maya",
            "Salvadore", "Morokei", "Moxy", "Gaige",
            "Wilhelm", "Athena", "Nisha", "Sir Hammerlock"
        };

        private MemoryCache<int, string> Cache;
        private delegate void TestPageDelegate(string value);
        private string Paged = "";

        private void TestPageHandler(string value)
        {
            Paged = value;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            Cache = new MemoryCache<int, string>(16);
        }

        [TearDown]
        public void TearDown()
        {
            Cache.Reset();
            Paged = "";
        }

        [Test]
        public void TestAddValue()
        {
            Cache.Add(TestKeys[0], TestValues[0]);
            Assert.True(Cache.Contains(TestKeys[0]));
        }

        [Test]
        public void TestContainsValue()
        {
            Cache.Add(TestKeys[0], TestValues[0]);
            Assert.True(Cache.Contains(TestKeys[0]));
        }

        [Test]
        public void TestRemoveValue()
        {
            Cache.Add(TestKeys[0], TestValues[0]);
            Assert.True(Cache.Contains(TestKeys[0]));
            Cache.Remove(TestKeys[0]);
            Assert.False(Cache.Contains(TestKeys[0]));
        }

        [Test]
        public void TestReadValue()
        {
            Cache.Add(TestKeys[0], TestValues[0]);
            Assert.AreEqual(TestValues[0], Cache.Get(TestKeys[0]));
        }

        [Test]
        public void TestGetValues()
        {
            for (int i = 0; i < TestKeys.Length; i++)
            {
                Cache.Add(TestKeys[i], TestValues[i]);
            }
            List<string> values = Cache.GetValues();
            for (int i = 0; i < TestValues.Length; i++)
            {
                Assert.Contains(TestValues[i], values);
            }
        }

        [Test]
        public void TestSetValue()
        {
            Cache.Add(TestKeys[0], TestValues[0]);
            Assert.AreEqual(TestValues[0], Cache.Get(TestKeys[0]));
            Cache.Set(TestKeys[0], TestValues[1]);
            Assert.AreEqual(TestValues[1], Cache.Get(TestKeys[0]));
        }
    }
}
