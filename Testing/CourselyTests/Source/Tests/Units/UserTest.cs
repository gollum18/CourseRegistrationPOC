using System;
using System.Collections.Generic;
using NUnit.Framework;
using Coursely.Content.Classes;

namespace CourselyTests.Tests.Units
{
    [TestFixture(Author = "Christen Ford", TestOf = typeof(User))]
    public class UserTest : IDataTests
    {
        private User Claptrap;
        private User Handsome;
        private User Ugly;
        private User Zero;
        private User[] Users = new User[4];

        /// <summary>
        /// Setups up the testing bed for the user class.
        /// </summary>
        [OneTimeSetUp]
        protected void SetUp()
        {
            Claptrap = new User("000001", "Claptrap", "Stewardbot", "claptrap@hyperion.com", "Instructor");
            Handsome = new User("000001", "Handsome", "Jack", "hjack@hyperion.com", "Administrator");
            Ugly = new User("000002", "Ugly", "Jack", "ujack@hyperion.com", "Administrator");
            Zero = new User("000003", "Zero", "[Unknown]", "zero@unknown.com", "Student");
            Users[0] = Claptrap;
            Users[1] = Handsome;
            Users[2] = Ugly;
            Users[3] = Zero;
        }

        /// <summary>
        /// Asserts that a user is equal to another.
        /// </summary>
        [Test]
        public void TestCompareToEqual()
        {
            Assert.AreEqual(Handsome.CompareTo(Ugly), 0);
        }

        /// <summary>
        /// Asserts that a user's last name is alphabetically greater than another user's.
        /// </summary>
        [Test]
        public void TestCompareToGreaterThan()
        {
            Assert.Greater(Claptrap.CompareTo(Handsome), 0);
        }

        /// <summary>
        /// Asserts that a user's last name is alphabetically less than another user's.
        /// </summary>
        [Test]
        public void TestCompareToLessThan()
        {
            Assert.Less(Handsome.CompareTo(Claptrap), 0);
        }

        /// <summary>
        /// Asserts that one user is equal to another.
        /// </summary>
        [Test]
        public void TestEquals()
        {
            Assert.True(Claptrap.Equals(Handsome));
        }

        /// <summary>
        /// Asserts that a user can properly generate a hashcode.
        /// </summary>
        [Test]
        public void TestGetHashCode()
        {
            User Claptrap2 = new User("000001", "Claptrap", "Stewardbot", "cl4ptbot@hyperion.com", "Instructor");
            Assert.AreEqual(Claptrap.GetHashCode() ,Claptrap2.GetHashCode());
        }

        /// <summary>
        /// Asserts that a user is not equal to another.
        /// </summary>
        [Test]
        public void TestNotEquals()
        {
            Assert.False(Claptrap.Equals(Ugly));
        }

        [Test]
        public void TestAddMajor()
        {
            Assert.False(Zero.HasMajor(0));
            Zero.AddMajor(0);
            Assert.True(Zero.HasMajor(0));
            Zero.RemoveMajor(0);
        }

        [Test]
        public void TestRemoveMajor()
        {
            Zero.AddMajor(0);
            Assert.True(Zero.HasMajor(0));
            Zero.RemoveMajor(0);
            Assert.False(Zero.HasMajor(0));
        }

        [Test]
        public void TestHasMajor()
        {
            Assert.False(Zero.HasMajor(0));
            Zero.AddMajor(0);
            Assert.True(Zero.HasMajor(0));
            Zero.RemoveMajor(0);
        }

        [Test]
        public void TestGetMajors()
        {
            bool contains = true;
            Zero.AddMajor(0);
            IEnumerator<int> majors = Zero.GetMajors();
            while (majors.MoveNext())
            {
                if (!Zero.HasMajor(majors.Current))
                {
                    contains = false;
                    break;
                }
            }
            Assert.True(contains);
            Zero.RemoveMajor(0);
        }

        [Test]
        public void TestAddRelationship()
        {
            Assert.False(Zero.HasRelationship("000001"));
            Zero.AddRelationship("000001");
            Assert.True(Zero.HasRelationship("000001"));
            Zero.RemoveRelationship("000001");
        }

        [Test]
        public void TestRemoveRelationship()
        {
            Zero.AddRelationship("000001");
            Assert.True(Zero.HasRelationship("000001"));
            Zero.RemoveRelationship("000001");
            Assert.False(Zero.HasRelationship("000001"));
        }

        [Test]
        public void TestHasRelationship()
        {
            Assert.False(Claptrap.HasRelationship("000001"));
            Zero.AddRelationship("000001");
            Assert.True(Zero.HasRelationship("000001"));
            Zero.RemoveRelationship("000001");
        }

        [Test]
        public void TestGetRelationships()
        {
            bool contains = true;
            Zero.AddRelationship("000001");
            IEnumerator<string> relationships = Zero.GetRelationships();
            while (relationships.MoveNext())
            {
                if (!Zero.HasRelationship(relationships.Current))
                {
                    contains = false;
                    break;
                }
            }
            Assert.True(contains);
            Zero.RemoveRelationship("000001");
        }

        [Test]
        public void TestAddDepartment()
        {
            Assert.False(Claptrap.HasDepartment(0));
            Claptrap.AddDepartment(0);
            Assert.True(Claptrap.HasDepartment(0));
            Claptrap.RemoveDepartment(0);
        }

        [Test]
        public void TestRemoveDepartment()
        {
            Claptrap.AddDepartment(0);
            Assert.True(Claptrap.HasDepartment(0));
            Claptrap.RemoveDepartment(0);
            Assert.False(Claptrap.HasDepartment(0));
        }

        [Test]
        public void TestHasDepartment()
        {
            Assert.False(Claptrap.HasDepartment(0));
            Claptrap.AddDepartment(0);
            Assert.True(Claptrap.HasDepartment(0));
            Claptrap.RemoveDepartment(0);
        }
        
        [Test]
        public void TestGetDepartments()
        {
            bool contains = true;
            Claptrap.AddDepartment(0);
            IEnumerator<int> departments = Claptrap.GetDepartments();
            while (departments.MoveNext())
            {
                if (!Claptrap.HasDepartment(departments.Current))
                {
                    contains = false;
                    break;
                }
            }
            Assert.True(contains);
            Claptrap.RemoveDepartment(0);
        }
    }
}