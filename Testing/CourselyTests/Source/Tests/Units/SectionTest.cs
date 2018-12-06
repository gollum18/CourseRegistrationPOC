using System;
using System.Collections.Generic;
using NUnit.Framework;
using Coursely.Content.Classes;

namespace CourselyTests.Tests.Units
{
    /// <summary>
    /// Test fixture for the Section class.
    /// </summary>
    [TestFixture(Author = "Christen Ford", TestOf = typeof(Section))]
    public class SectionTest : IDataTests
    {
        Section Section1;
        Section Section2;
        Section Section3;
        Section Section4;

        /// <summary>
        /// Sets up the test fixture.
        /// </summary>
        [OneTimeSetUp]
        protected void SetUp()
        {
            Section1 = new Section(0, 0, "Spring", 2019)
            {
                BuildingID = 0,
                StartDateAndTime = new DateTime(2019, 1, 21, 9, 0, 0),
                EndDateAndTime = new DateTime(2019, 5, 6, 9, 50, 0),
                Room = 204,
                MaxEnrollment = 32
            };
            Section2 = new Section(1, 0, "Spring", 2019)
            {
                BuildingID = 0,
                StartDateAndTime = new DateTime(2019, 1, 21, 10, 0, 0),
                EndDateAndTime = new DateTime(2019, 5, 6, 10, 50, 0),
                Room = 206,
                MaxEnrollment = 32
            };
            Section3 = new Section(1, 0, "Summer", 2019)
            {
                BuildingID = 0,
                StartDateAndTime = new DateTime(2019, 6, 3, 9, 0, 0),
                EndDateAndTime = new DateTime(2019, 8, 5, 9, 50, 0),
                Room = 108,
                MaxEnrollment = 15
            };
            Section4 = new Section(2, 0, "Summer", 2019)
            {
                BuildingID = 0,
                StartDateAndTime = new DateTime(2019, 6, 3, 9, 0, 0),
                EndDateAndTime = new DateTime(2019, 8, 5, 9, 50, 0),
                Room = 110,
                MaxEnrollment = 15
            };
        }

        /// <summary>
        /// Asserts that the start date and time for both sections are the same.
        /// </summary>
        [Test]
        public void TestCompareToEqual()
        {
            Assert.AreEqual(0, Section3.CompareTo(Section4));
        }

        /// <summary>
        /// Asserts that the start date and time of two sections follow greater than natural ordering.
        /// </summary>
        [Test]
        public void TestCompareToGreaterThan()
        {
            Assert.Greater(Section2.CompareTo(Section1), 0);
        }

        /// <summary>
        /// Assers that the start date and time of two sections follow less than natural ordering.
        /// </summary>
        [Test]
        public void TestCompareToLessThan()
        {
            Assert.Less(Section1.CompareTo(Section2), 0);
        }

        /// <summary>
        /// Asserts that two sections are equal to each other.
        /// </summary>
        [Test]
        public void TestEquals()
        {
            Assert.True(Section2.Equals(Section3));
        }

        /// <summary>
        /// Asserts that a section properly generates it's hashcode.
        /// </summary>
        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(0, Section1.GetHashCode());
        }

        /// <summary>
        /// Asserts that two sections are not equal.
        /// </summary>
        [Test]
        public void TestNotEquals()
        {
            Assert.False(Section1.Equals(Section2));
        }

        /// <summary>
        /// Asserts that a section can properly track it's enrollment.
        /// </summary>
        [Test]
        public void TestCurrentEnrollment()
        {
            Section section = GenerateTestSection();
        }

        /// <summary>
        /// Asserts that a student can be successfully added to a section.
        /// </summary>
        [Test]
        public void TestAddStudent()
        {
            Section section = GenerateTestSection();
            Assert.False(section.IsStudentEnrolled("000001"));
            section.AddStudent("000001");
            Assert.True(section.IsStudentEnrolled("000001"));
        }

        /// <summary>
        /// Asserts that a student can be successfully removed from a section.
        /// </summary>
        [Test]
        public void TestRemoveStudent()
        {
            Section section = GenerateTestSection();
            section.AddStudent("000001");
            Assert.True(section.IsStudentEnrolled("000001"));
            section.RemoveStudent("000001");
            Assert.False(section.IsStudentEnrolled("000001"));
        }

        [Test]
        public void TestHasStudent()
        {
            Section section = GenerateTestSection();
            section.AddStudent("000001");
            Assert.True(section.IsStudentEnrolled("000001"));
        }

        [Test]
        public void TestGetStudents()
        {
            Section section = GenerateTestSection();
            section.AddStudent("000001");
            section.AddStudent("000002");
            IEnumerator<string> enumerator = section.GetStudents();
            List<string> students = new List<string>();
            while (enumerator.MoveNext())
            {
                students.Add(enumerator.Current);
            }
            Assert.True(students.Contains("000001"));
            Assert.True(students.Contains("000002"));
        }

        /// <summary>
        /// Asserts that an instructor can be added to a section.
        /// </summary>
        [Test]
        public void TestAddInstructor()
        {
            Section section = GenerateTestSection();
            Assert.False(section.IsInstructorScheduled("000001"));
            section.AddInstructor("000001");
            Assert.True(section.IsInstructorScheduled("000001"));
        }

        /// <summary>
        /// Asserts that an instructor can be removed from a section.
        /// </summary>
        [Test]
        public void TestRemoveInstructor()
        {
            Section section = GenerateTestSection();
            section.AddInstructor("000001");
            Assert.True(section.IsInstructorScheduled("000001"));
            section.RemoveInstructor("000001");
            Assert.False(section.IsInstructorScheduled("000001"));
        }

        [Test]
        public void TestHasInstructor()
        {
            Section section = GenerateTestSection();
            section.AddInstructor("000010");
            Assert.True(section.IsInstructorScheduled("000010"));
        }

        [Test]
        public void TestGetInstructors()
        {
            Section section = GenerateTestSection();
            section.AddInstructor("000000");
            section.AddInstructor("000001");
            section.AddInstructor("000002");
            IEnumerator<string> enumerator = section.GetInstructors();
            List<string> instructors = new List<string>();
            while (enumerator.MoveNext())
            {
                instructors.Add(enumerator.Current);
            }
            Assert.True(instructors.Contains("000000"));
            Assert.True(instructors.Contains("000001"));
            Assert.True(instructors.Contains("000001"));
        }

        /// <summary>
        /// Asserts that a section can be scheduled for a day.
        /// </summary>
        [Test]
        public void TestAddDay()
        {
            Section section = GenerateTestSection();
            Assert.False(section.IsSectionHeldOnDay("Monday"));
            section.AddDay("Monday");
            Assert.True(section.IsSectionHeldOnDay("Monday"));
        }

        /// <summary>
        /// Asserts that a section can be unscheduled for a day.
        /// </summary>
        [Test]
        public void TestRemoveDay()
        {
            Section section = GenerateTestSection();
            section.AddDay("Monday");
            Assert.True(section.IsSectionHeldOnDay("Monday"));
            section.RemoveDay("Monday");
            Assert.False(section.IsSectionHeldOnDay("Monday"));
        }

        [Test]
        public void TestHasDay()
        {
            Section section = GenerateTestSection();
            section.AddDay("Monday");
            Assert.True(section.IsSectionHeldOnDay("Monday"));
        }

        [Test]
        public void TestGetDays()
        {
            Section section = GenerateTestSection();
            section.AddDay("Monday");
            section.AddDay("Wednesday");
            section.AddDay("Friday");
            IEnumerator<string> enumerator = section.GetDays();
            List<string> days = new List<string>();
            while (enumerator.MoveNext())
            {
                days.Add(enumerator.Current);
            }
            Assert.True(days.Contains("Monday"));
            Assert.True(days.Contains("Wednesday"));
            Assert.True(days.Contains("Friday"));
        }

        private Section GenerateTestSection()
        {
            return new Section(0, 0, "Spring", 2019)
            {
                BuildingID = 0,
                StartDateAndTime = new DateTime(2019, 1, 21, 9, 0, 0),
                EndDateAndTime = new DateTime(2019, 5, 6, 9, 50, 0),
                Room = 204,
                MaxEnrollment = 32
            };
        }
    }
}