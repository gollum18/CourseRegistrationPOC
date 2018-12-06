using System;
using NUnit.Framework;
using Coursely.Content.Classes;

namespace CourselyTests.Tests.Units
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(School))]
    public class SchoolTest : IDataTests
    {
        School ArtsAndHumanities;
        School Law;
        School Medicine;
        School NaturalSciences;
        School PhysicalScience;

        [OneTimeSetUp]
        protected void SetUp()
        {
            ArtsAndHumanities = new School(0, "Felyn College of Arts and Humanities")
            {
                Abbreviation = "FARH"
            };
            Law = new School(1, "Moorehead School of Law")
            {
                Abbreviation = "MSCL"
            };
            Medicine = new School(2, "Kelso School of Medicine")
            {
                Abbreviation = "KMED"
            };
            NaturalSciences = new School(3, "Cresling School of Natural Sciences")
            {
                Abbreviation = "CSNS"
            };
            PhysicalScience = new School(3, "Cresling School of Natural Sciences")
            {
                Abbreviation = "CSNS"
            };
        }

        [Test]
        public void TestCompareToEqual()
        {
            Assert.AreEqual(0, NaturalSciences.CompareTo(PhysicalScience));
        }

        [Test]
        public void TestCompareToGreaterThan()
        {
            Assert.Greater(ArtsAndHumanities.CompareTo(PhysicalScience), 0);
        }

        [Test]
        public void TestCompareToLessThan()
        {
            Assert.Less(PhysicalScience.CompareTo(ArtsAndHumanities), 0);
        }

        [Test]
        public void TestEquals()
        {
            Assert.True(NaturalSciences.Equals(PhysicalScience));
        }

        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(1, Law.GetHashCode());
        }

        [Test]
        public void TestNotEquals()
        {
            Assert.False(Law.Equals(Medicine));
        }
    }
}