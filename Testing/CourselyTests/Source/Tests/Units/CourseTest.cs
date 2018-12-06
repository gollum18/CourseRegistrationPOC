using System;
using NUnit.Framework;
using Coursely.Content.Classes;

namespace CourselyTests.Tests.Units
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(Course))]
    public class CourseTest : IDataTests
    {
        private Course ArtificialIntelligence;
        private Course ComputerNetworks;
        private Course DatabaseSystems;
        private Course DataStructures;
        private Course IntroComputing;
        private Course LocalAreaNetworks;

        [OneTimeSetUp]
        protected void SetUp()
        {
            ArtificialIntelligence = new Course(0, "Artificial Intelligence")
            {
                Archived = false,
                Credits = 4,
                DepartmentID = 0,
                Description = "Students will develop an understanding of modern artificial intellgence systems. Major topics include machine learning, " +
                    "neural networks, markov decision processes, and decision tree learning.",
                Number = "410"
            };
            ComputerNetworks = new Course(1, "Computer Networks")
            {
                Archived = false,
                Credits = 4,
                DepartmentID = 0,
                Description = "Students will explore the history of computer networks as well as modern computer networks. Hands on lab experience designing and " +
                    "building simulated computer networks is explored.",
                Number = "225"
            };
            DatabaseSystems = new Course(2, "Database Systems")
            {
                Archived = false,
                Credits = 3,
                DepartmentID = 0,
                Description = "Students are introduced to modern database systems such as FireBase, CouchDB, MariaDB, and SQL Server.",
                Number = "375"
            };
            DataStructures = new Course(3, "Data Structures and Algorithms")
            {
                Archived = false,
                Credits = 4,
                DepartmentID = 0,
                Description = "Students explore various linear and non-linear data structures. Appropriate algorithms are explored from brute-force to dynamic " +
                "programming within the context of each data structure.",
                Number = "250"
            };
            IntroComputing = new Course(4, "Introduction to Computing")
            {
                Archived = false,
                Credits = 3,
                DepartmentID = 0,
                Description = "Students are introduced to modern topics in computing. Topics include artificial intelligence, computer networks, databases, ethics, " +
                    "research, and software engineering. Students will complete a research-based term paper on a topic of their choice and present it at the end " +
                    "of the term.",
                Number = "150"
            };
            LocalAreaNetworks = new Course(1, "Computer Networks")
            {
                Archived = false,
                Credits = 4,
                DepartmentID = 0,
                Description = "Students will explore the history of computer networks as well as modern computer networks. Hands on lab experience designing and " +
                    "building simulated computer networks is explored.",
                Number = "225"
            };
        }

        /// <summary>
        /// Asserts that one 
        /// </summary>
        [Test]
        public void TestCompareToEqual()
        {
            Assert.AreEqual(0, ComputerNetworks.CompareTo(LocalAreaNetworks));
        }

        [Test]
        public void TestCompareToGreaterThan()
        {
            Assert.Greater(IntroComputing.CompareTo(ArtificialIntelligence), 0);
        }

        /// <summary>
        /// Asserts that one class comes alphabetically before the other.
        /// </summary>
        [Test]
        public void TestCompareToLessThan()
        {
            Assert.Less(ArtificialIntelligence.CompareTo(IntroComputing), 0);
        }

        /// <summary>
        /// Asserts that two courses are equal.
        /// </summary>
        [Test]
        public void TestEquals()
        {
            Assert.True(ComputerNetworks.Equals(LocalAreaNetworks));
        }

        /// <summary>
        /// Asserts that a course generates the correct hashcode.
        /// </summary>
        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(3, DataStructures.GetHashCode());
        }

        /// <summary>
        /// Asserts that one course is not equal to another.
        /// </summary>
        [Test]
        public void TestNotEquals()
        {
            Assert.False(DatabaseSystems.Equals(DataStructures));
        }
    }
}