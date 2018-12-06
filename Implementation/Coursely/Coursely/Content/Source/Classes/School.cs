using System;
using System.Collections.Generic;

namespace Coursely.Content.Classes
{
    /// <summary>
    /// Represents a school at a university.
    /// </summary>
    public class School : IComparable<School>
    {
        /// <summary>
        /// The school's unique identifier.
        /// </summary>
        public int SchoolID { get; }

        /// <summary>
        /// The school's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The abbreviation of the school's name.
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// The departments the school offers.
        /// </summary>
        private HashSet<int> Departments = new HashSet<int>();

        /// <summary>
        /// Creates an instance of a school with the given parameters.
        /// </summary>
        /// <param name="schoolID">The school identifier.</param>
        /// <param name="name">The school name.</param>
        public School(int schoolID, string name)
        {
            SchoolID = schoolID;
            Name = name;
            Abbreviation = "";
        }

        /// <summary>
        /// Determines whether the school is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>True if the school identifier and the objects school identifer are the same. 
        /// False if the object is null or is not of type School.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is School))
            {
                return false;
            }
            return (obj as School).SchoolID == SchoolID;
        }

        /// <summary>
        /// Gets the unique hashcode for the school.
        /// </summary>
        /// <returns>The school identifier since it is already guaranteed to be unique.</returns>
        public override int GetHashCode()
        {
            return SchoolID;
        }

        /// <summary>
        /// Compares this school to another school.
        /// </summary>
        /// <param name="other">The other school to compare against.</param>
        /// <returns>The natural ordering of this school's name compared to the other school's name.</returns>
        public int CompareTo(School other)
        {
            return Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Adds a department to the school.
        /// </summary>
        /// <param name="departmentID">The identifier for the department to add to the school.</param>
        /// <exception cref="Exception">If the department is already housed within the school.</exception>
        public void AddDepartment(int departmentID)
        {
            if (ContainsDepartment(departmentID))
            {
                throw new Exception("Cannot add department to school! Department is already part of the school!");
            }
            Departments.Add(departmentID);
        }

        /// <summary>
        /// Determines whether a given department is housed within the school.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <returns>True if the department is housed within the school, false otherwise.</returns>
        public bool ContainsDepartment(int departmentID)
        {
            return Departments.Contains(departmentID);
        }

        /// <summary>
        /// Removes a department from the school.
        /// </summary>
        /// <param name="departmentID">The identifier of the department to remove from the school.</param>
        /// <exception cref="Exception">If the department is not housed within the school.</exception>
        public void RemoveDepartment(int departmentID)
        {
            if (!ContainsDepartment(departmentID))
            {
                throw new Exception("Cannot remove department from school! Department is not part of the school!");
            }
            Departments.Remove(departmentID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) over the departments housed in the school.
        /// </summary>
        /// <returns>An enumerator over the departments housed in the school.</returns>
        public IEnumerator<int> GetDepartments()
        {
            return Departments.GetEnumerator();
        }

        /// <summary>
        /// Modifies a school with the given parameters.
        /// </summary>
        /// <param name="abbreviation">An abbreviation of school's name.</param>
        public void Modify(string abbreviation)
        {
            Abbreviation = abbreviation;
        }
    }
}