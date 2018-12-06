using System;
using NUnit.Framework;

namespace CourselyTests.Tests.Units
{
    interface IDataTests
    {
        /// <summary>
        /// Asserts that two buildings are equal.
        /// </summary>
        [Test]
        void TestEquals();

        /// <summary>
        /// Asserts that two buildings are not equal.
        [Test]
        void TestNotEquals();

        /// <summary>
        /// Asserts that a building generates the correct hashcode.
        /// </summary>
        [Test]
        void TestGetHashCode();

        /// <summary>
        /// Asserts that one buildings name alphabetically comes before the others.
        /// </summary>
        [Test]
        void TestCompareToLessThan();

        /// <summary>
        /// Asserts that one buildings name is the same as another buildings name.
        /// </summary>
        [Test]
        void TestCompareToEqual();

        /// <summary>
        /// Asserts that one buildings 
        /// </summary>
        [Test]
        void TestCompareToGreaterThan();
    }
}
