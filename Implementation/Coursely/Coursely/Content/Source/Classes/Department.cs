using System;
using System.Collections.Generic;

namespace Coursely.Content.Classes
{
    /// <summary>
    /// Represents a unique department within a school.
    /// </summary>
    public class Department : IComparable<Department>
    {
        /// <summary>
        /// The department identifier.
        /// </summary>
        public int DepartmentID { get; }

        /// <summary>
        /// The identifier of the school the department belongs to.
        /// </summary>
        public int SchoolID { get; set; }
        
        /// <summary>
        /// The name of the department.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The abbreviation of the department's name.
        /// </summary>
        public string Abbreviation { get; set; }
        
        /// <summary>
        /// The courses offered by the department.
        /// </summary>
        private HashSet<int> Courses = new HashSet<int>();
        
        /// <summary>
        /// The majors offered by the department.
        /// </summary>
        private HashSet<int> Majors = new HashSet<int>();

        /// <summary>
        /// Creates an instance of a department with the given parameters.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <param name="name">The department name.</param>
        public Department(int departmentID, string name)
        {
            DepartmentID = departmentID;
            Name = name;
        }

        /// <summary>
        /// Determines if this department is equal to another.
        /// </summary>
        /// <param name="obj">An object to compare against.</param>
        /// <returns>True if the object is not null, is a department, and has the same department identifier as this 
        /// department.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Department))
            {
                return false;
            }
            return (obj as Department).DepartmentID == DepartmentID;
        }

        /// <summary>
        /// Returns the unique hash code for the department.
        /// </summary>
        /// <returns>The department's identifier as it is already guaranteed to be unique.</returns>
        public override int GetHashCode()
        {
            return DepartmentID;
        }

        /// <summary>
        /// Compares this department to another department.
        /// </summary>
        /// <param name="other">The other department to compare against.</param>
        /// <returns>The natural ordering of comparing this department's name to the other department's name.</returns>
        public int CompareTo(Department other)
        {
            return Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Adds a course to the department.
        /// </summary>
        /// <param name="courseID">The identifier of the course to add.</param>
        /// <exception cref="Exception">If the course is already offered by the department.</exception>
        public void AddCourse(int courseID)
        {
            if (IsCourseOffered(courseID))
            {
                throw new Exception("Cannot add course to the department! Course is already offered by " +
                    "the department!");
            }
            Courses.Add(courseID);
        }

        /// <summary>
        /// Determines if a course is offered by the department.
        /// </summary>
        /// <param name="courseID">The identifier for the course to check for.</param>
        /// <returns>True if the department offers the course, false otherwise.</returns>
        public bool IsCourseOffered(int courseID)
        {
            return Courses.Contains(courseID);
        }

        /// <summary>
        /// Removes a course from the department.
        /// </summary>
        /// <param name="courseID">The identifier of the course to remove.</param>
        /// <exception cref="Exception">If the course is not offered by the department.</exception>
        public void RemoveCourse(int courseID)
        {
            if (!IsCourseOffered(courseID))
            {
                throw new Exception("Cannot remove course from the deparment! The course is not offered by " +
                    "the department!");
            }
            Courses.Remove(courseID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) over the courses offered by the department.
        /// </summary>
        /// <returns>An enumerator over the courses offered by the department.</returns>
        public IEnumerator<int> GetCourses()
        {
            return Courses.GetEnumerator();
        }

        /// <summary>
        /// Adds a major to the department.
        /// </summary>
        /// <param name="majorID">The identifier for the major.</param>
        /// <exception cref="Exception">If the department already offers the major.</exception>
        public void AddMajor(int majorID)
        {
            if (IsMajorOffered(majorID))
            {
                throw new Exception("Cannot add major to the department. Department already offers the major!");
            }
            Majors.Add(majorID);
        }

        /// <summary>
        /// Determines whether the department offers a given major.
        /// </summary>
        /// <param name="majorID">The identifier for the major.</param>
        /// <returns></returns>
        public bool IsMajorOffered(int majorID)
        {
            return Majors.Contains(majorID);
        }

        /// <summary>
        /// Removes a major from the department.
        /// </summary>
        /// <param name="majorID">The identifier for the major.</param>
        /// <exception cref="Exception">If the department does not offer the major.</exception>
        public void RemoveMajor(int majorID)
        {
            if (!IsMajorOffered(majorID))
            {
                throw new Exception("Cannot remove major from department! Department does not offer the major!");
            }
            Majors.Remove(majorID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) over the department's majors.
        /// </summary>
        /// <returns>An enumerator over the department's majors.</returns>
        public IEnumerator<int> GetMajors()
        {
            return Majors.GetEnumerator();
        }

        /// <summary>
        /// Modifes a department to the given parameters.
        /// </summary>
        /// <param name="schoolID">The school identifier.</param>
        /// <param name="abbreviation">The school's name abbreviation.</param>
        public void Modify(int schoolID, string abbreviation)
        {
            SchoolID = schoolID;
            Abbreviation = abbreviation;
        }
    }
}