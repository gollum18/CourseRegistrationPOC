using System;
using NUnit.Framework;
using Coursely.Content.Classes;

namespace CourselyTests.Tests.Units
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(Department))]
    public class DepartmentTest : IDataTests
    {
        private Department Chemistry;
        private Department ComputerScience;
        private Department InformationSystems;
        private Department Mathematics;
        private Department Physics;

        /// <summary>
        /// Sets up testing.
        /// </summary>
        [OneTimeSetUp]
        protected void SetUp()
        {
            Chemistry = new Department(0, "Chemistry")
            {
                Abbreviation = "CHY",
                SchoolID = 0
            };
            ComputerScience = new Department(1, "Computer Science")
            {
                Abbreviation = "CSC",
                SchoolID = 0
            };
            InformationSystems = new Department(1, "Computer Science")
            {
                Abbreviation = "CIS",
                SchoolID = 0
            };
            Mathematics = new Department(2, "Mathematics")
            {
                Abbreviation = "MTH",
                SchoolID = 0
            };
            Physics = new Department(3, "Physics")
            {
                Abbreviation = "PSC",
                SchoolID = 0
            };
        }

        /// <summary>
        /// Asserts that two departments share the same name.
        /// </summary>
        [Test]
        public void TestCompareToEqual()
        {
            Assert.AreEqual(0, ComputerScience.CompareTo(InformationSystems));
        }

        /// <summary>
        /// Asserts that one department's name is alphabetically greater than the other department's name.
        /// </summary>
        [Test]
        public void TestCompareToGreaterThan()
        {
            Assert.Greater(Physics.CompareTo(Chemistry), 0);
        }

        /// <summary>
        /// Asserts that one department's name is alphabetically less than the other department's name.
        /// </summary>
        [Test]
        public void TestCompareToLessThan()
        {
            Assert.Less(Chemistry.CompareTo(Physics), 0);
        }

        /// <summary>
        /// Asserts that two departments are equal.
        /// </summary>
        [Test]
        public void TestEquals()
        {
            Assert.True(ComputerScience.Equals(InformationSystems));
        }

        /// <summary>
        /// Asserts that a department properly generates it's hashcode.
        /// </summary>
        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(Mathematics.GetHashCode(), 2);
        }

        /// <summary>
        /// Asserts that two departments are not equal.
        /// </summary>
        [Test]
        public void TestNotEquals()
        {
            Assert.False(Physics.Equals(Mathematics));
        }
    }
}