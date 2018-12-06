using Coursely.Content.Cache;
using NUnit.Framework;

namespace CourselyTests.Tests.Components
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(CacheItem<int, string>))]
    class CacheItemTest
    {
        private readonly int TestKey = 0;
        private readonly int TestKey2 = 1;
        private readonly string TestValue = "Claptrap";
        private readonly string TestValue2 = "Jack";
        private readonly bool TestOccupied = true;
        
        private CacheItem<int, string> Item;

        [OneTimeSetUp]
        public void SetUp()
        {
            Item = new CacheItem<int, string>(TestKey, TestValue)
            {
                Occupied = TestOccupied
            };
        }

        [TearDown]
        public void TearDown()
        {
            Item.Key = TestKey;
            Item.Value = TestValue;
            Item.Occupied = TestOccupied;
        }

        [Test]
        public void TestGetKey()
        {
            Assert.AreEqual(TestKey, Item.Key);
        }

        [Test]
        public void TestSetKey()
        {
            Item.Key = TestKey2;
            Assert.AreEqual(TestKey2, Item.Key);
        }

        [Test]
        public void TestGetValue()
        {
            Assert.AreEqual(TestValue, Item.Value);
        }

        [Test]
        public void TestSetValue()
        {
            Item.Value = TestValue2;
            Assert.AreEqual(TestValue2, Item.Value);
        }

        [Test]
        public void TestGetOccupied()
        {
            Assert.True(Item.Occupied);
        }

        [Test]
        public void TestSetOccupied()
        {
            Item.Occupied = false;
            Assert.False(Item.Occupied);
        }

        [Test]
        public void TestGetAge()
        {
            Assert.AreEqual(0, Item.Age);
        }

        [Test]
        public void TestSetAge()
        {
            Item.Age++;
            Assert.AreEqual(1, Item.Age);
        }
    }
}
