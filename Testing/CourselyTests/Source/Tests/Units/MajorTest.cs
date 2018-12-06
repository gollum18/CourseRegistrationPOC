using System;
using NUnit.Framework;
using Coursely.Content.Classes;

namespace CourselyTests.Tests.Units
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(Major))]
    public class MajorTest : IDataTests
    {
        private Major BusinessAdmin;
        private Major ComputerScience;
        private Major InformationSystems;
        private Major Mathematics;

        [OneTimeSetUp]
        protected void SetUp()
        {
            BusinessAdmin = new Major(0, "Business Administration")
            {
                DepartmentID = 0
            };
            ComputerScience = new Major(1, "Computer Science")
            {
                DepartmentID = 1
            };
            InformationSystems = new Major(1, "Computer Science")
            {
                DepartmentID = 1
            };
            Mathematics = new Major(2, "Mathematics")
            {
                DepartmentID = 2
            };
        }

        [Test]
        public void TestCompareToEqual()
        {
            Assert.AreEqual(0, ComputerScience.CompareTo(InformationSystems));
        }

        [Test]
        public void TestCompareToGreaterThan()
        {
            Assert.Greater(Mathematics.CompareTo(BusinessAdmin), 0);
        }

        [Test]
        public void TestCompareToLessThan()
        {
            Assert.Less(BusinessAdmin.CompareTo(Mathematics), 0);
        }

        [Test]
        public void TestEquals()
        {
            Assert.True(ComputerScience.Equals(InformationSystems));
        }

        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(0, BusinessAdmin.GetHashCode());
        }

        [Test]
        public void TestNotEquals()
        {
            Assert.False(BusinessAdmin.Equals(Mathematics));
        }
    }
}