using System;

namespace Coursely.Content.Classes
{
    public class Major : IComparable<Major>
    {
        //
        // FIELDS
        //

        /// <summary>
        /// The major identifier.
        /// </summary>
        public int MajorID { get; }
        /// <summary>
        /// The department identifier.
        /// </summary>
        public int DepartmentID { get; set; }
        /// <summary>
        /// The majors name.
        /// </summary>
        public string Name { get; }

        //
        // CONSTRUCTORS
        //

        /// <summary>
        /// Returns an instance of a major with the given parameters.
        /// </summary>
        /// <param name="majorID">A major identifier.</param>
        /// <param name="name">A major name.</param>
        public Major(int majorID, string name)
        {
            MajorID = majorID;
            DepartmentID = default(int);
            Name = name;
        }
        
        //
        // METHODS
        //

        /// <summary>
        /// Determines if this major is equal to another.
        /// </summary>
        /// <param name="obj">The major to compare against.</param>
        /// <returns>True if two majors are equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Major))
            {
                return false;
            }
            return (obj as Major).MajorID == MajorID;
        }

        /// <summary>
        /// Generates a hashcode for the major.
        /// </summary>
        /// <returns>The majo identifier, it is already guaranteed to be unique.</returns>
        public override int GetHashCode()
        {
            return MajorID;
        }

        /// <summary>
        /// Compares this major's name to another majors name.
        /// </summary>
        /// <param name="other">The major to compare against.</param>
        /// <returns>The natural ordering of this majors name to another majors name.</returns>
        public int CompareTo(Major other)
        {
            return Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Modifies a major to the given parameters.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        public void Modify(int departmentID)
        {
            DepartmentID = departmentID;
        }
    }
}