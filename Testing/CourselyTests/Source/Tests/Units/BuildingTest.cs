using System;
using NUnit.Framework;
using Coursely.Content.Classes;

namespace CourselyTests.Tests.Units
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(Building))]
    public class BuildingTest : IDataTests
    {
        private Building BuildingCH;
        private Building BuildingFH;
        private Building BuildingSH;
        private Building BuildingSH2;
        private Building BuildingWH;

        [OneTimeSetUp]
        protected void SetUp()
        {
            BuildingCH = new Building(0, "Chadwick Hall")
            {
                Abbreviation = "CH"
            };
            BuildingFH = new Building(1, "Fenn Hall")
            {
                Abbreviation = "FH"
            };
            BuildingSH = new Building(2, "Stillwell Hall")
            {
                Abbreviation = "SH"
            };
            BuildingSH2 = new Building(2, "Stillwell Hall")
            {
                Abbreviation = "SH"
            };
            BuildingWH = new Building(3, "Washkewicz Hall")
            {
                Abbreviation = "WH"
            };
        }

        /// <summary>
        /// Asserts that two buildings are equal.
        /// </summary>
        [Test]
        public void TestEquals()
        {
            Assert.True(BuildingSH.Equals(BuildingSH2));
        }

        /// <summary>
        /// Asserts that two buildings are not equal.
        /// </summary>
        [Test]
        public void TestNotEquals()
        {
            Assert.False(BuildingCH.Equals(BuildingFH));
        }

        /// <summary>
        /// Asserts that a building generates the correct hashcode.
        /// </summary>
        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(3, BuildingWH.GetHashCode());
        }

        /// <summary>
        /// Asserts that one buildings name alphabetically comes before the others.
        /// </summary>
        [Test]
        public void TestCompareToLessThan()
        {
            Assert.Less(BuildingCH.CompareTo(BuildingFH), 0);
        }

        /// <summary>
        /// Asserts that one buildings name is the same as another buildings name.
        /// </summary>
        [Test]
        public void TestCompareToEqual()
        {
            Assert.AreEqual(0, BuildingSH.CompareTo(BuildingSH2));
        }

        /// <summary>
        /// Asserts that one buildings 
        /// </summary>
        [Test]
        public void TestCompareToGreaterThan()
        {
            Assert.Greater(BuildingFH.CompareTo(BuildingCH), 0);
        }
    }
}