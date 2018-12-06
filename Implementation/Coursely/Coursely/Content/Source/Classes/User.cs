using System;
using System.Collections.Generic;

namespace Coursely.Content.Classes
{
    /// <summary>
    /// Represents a user of the system.
    /// </summary>
    public class User : IComparable<User>
    {
        //
        // Constants
        //

        /// <summary>
        /// Represents the administrator role.
        /// </summary>
        public static readonly string ADMINISTRATOR = "Administrator";

        /// <summary>
        /// Represents the instructor role.
        /// </summary>
        public static readonly string INSTRUCTOR = "Instructor";

        /// <summary>
        /// Represents the student role.
        /// </summary>
        public static readonly string STUDENT = "Student";

        //
        // Fields
        //

        /// <summary>
        /// The user's unique university identifier.
        /// </summary>
        public string UnivID { get; }

        /// <summary>
        /// The user's first name.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// The user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's role.
        /// </summary>
        public string Role { get; }

        /// <summary>
        /// Holds the student's majors.
        /// </summary>
        private HashSet<int> Majors = null;

        /// <summary>
        /// Holds a students/instructors relationships with each other.
        /// </summary>
        private HashSet<string> Relationships = null;

        /// <summary>
        /// Holds an instructor's departments.
        /// </summary>
        private HashSet<int> Departments = null;

        //
        // CONSTRUCTORS
        //

        /// <summary>
        /// Creates an instance of a user with the given parameters.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="role">The user's role.</param>
        public User(string univID, string firstName, string lastName, string email, string role)
        {
            UnivID = univID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            if (!role.Equals(User.ADMINISTRATOR)) {
                Relationships = new HashSet<string>();
                if (role.Equals(User.STUDENT))
                {
                    Majors = new HashSet<int>();
                }
                if (role.Equals(User.INSTRUCTOR))
                {
                    Departments = new HashSet<int>();
                }
            }
        }

        //
        // METHODS
        //

        /// <summary>
        /// Determines if an object is equal to the user.
        /// </summary>
        /// <param name="obj">An object to compare against.</param>
        /// <returns>True if the obj is both a User and the user's university ID is equal to it, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is User))
            {
                return false;
            }
            return (obj as User).UnivID.Equals(UnivID);
        }

        /// <summary>
        /// Determines the unique hash code of the user.
        /// </summary>
        /// <returns>Essentially the hash of the user's university ID.</returns>
        public override int GetHashCode()
        {
            return UnivID.GetHashCode();
        }

        /// <summary>
        /// Compares this user to another.
        /// </summary>
        /// <param name="other">The other user to compare against.</param>
        /// <returns>The natural ordering of the user's last names.</returns>
        public int CompareTo(User other)
        {
            return LastName.CompareTo(other.LastName);
        }

        /// <summary>
        /// Determines whether a student has a specific major.
        /// </summary>
        /// <param name="majorID">The major identifier.</param>
        /// <returns>True if the student has a major, false otherwise.</returns>
        /// <exception cref="Exception">If the user is not a student.</exception>
        public bool HasMajor(int majorID)
        {
            if (!Role.Equals(User.STUDENT))
            {
                throw new Exception("Cannot determine if user has the given major! User is not a student!");
            }
            return Majors.Contains(majorID);
        }

        /// <summary>
        /// Enrolls the student in the given major.
        /// </summary>
        /// <param name="majorID">The major identifier.</param>
        /// <exception cref="Exception">If the user is not a student.</exception>
        /// <exception cref="Exception">If the student is already enrolled in the major.</exception>
        public void AddMajor(int majorID)
        {
            if (!Role.Equals(User.STUDENT))
            {
                throw new Exception("Cannot enroll user in major! User is not a student!");
            }
            if (HasMajor(majorID))
            {
                throw new Exception("Cannot enroll the student in the specified major! The student already has the major!");
            }
            Majors.Add(majorID);
        }

        /// <summary>
        /// Unenrolls the student from the given major.
        /// </summary>
        /// <param name="majorID">The major identifier.</param>
        /// <exception cref="Exception">If the user is not a student.</exception>
        /// <exception cref="Exception">If the student is not enrolled in the major.</exception>
        public void RemoveMajor(int majorID)
        {
            if (!Role.Equals(User.STUDENT))
            {
                throw new Exception("Cannot unenroll user in major! User is not a student!");
            }
            if (!HasMajor(majorID))
            {
                throw new Exception("Cannot unenroll the student in the specified major! The student is not enrolled in the major!");
            }
            Majors.Remove(majorID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) over the 
        /// </summary>
        /// <returns>An enumerator over the student's majors.</returns>
        /// <exception cref="Exception">If the user is not a student.</exception>
        public IEnumerator<int> GetMajors()
        {
            if (!Role.Equals(User.STUDENT))
            {
                throw new Exception("Unable to get the user's majors! User is not a student!");
            }
            return Majors.GetEnumerator();
        }

        /// <summary>
        /// Adds an advisor/advisee relationship to the user. User must be either an instructor or student.
        /// </summary>
        /// <param name="univID">A user's university identifier.</param>
        /// <exception cref="Exception">If the user is an administrator.</exception>
        /// <exception cref="Exception">If the user already has a relationship with the given user.</exception>
        public void AddRelationship(string univID)
        {
            if (Role.Equals(User.ADMINISTRATOR))
            {
                throw new Exception("User does not have advisor/advisee relationships! User is not a student or " +
                    "instructor!");
            }
            if (HasRelationship(univID))
            {
                throw new Exception($"User {UnivID} already has a relationship with user {univID}!");
            }
            Relationships.Add(univID);
        }

        /// <summary>
        /// Determines if a user has a relationship with the given user.
        /// </summary>
        /// <param name="univID">A user's university identifier.</param>
        /// <returns>True if the user has a relationship with the given user.</returns>
        /// <exception cref="Exception">If the user is an administrator.</exception>
        public bool HasRelationship(string univID)
        {
            if (Role.Equals(User.ADMINISTRATOR))
            {
                throw new Exception("User does not have advisor/advisee relationships! User is not a student or " +
                    "instructor!");
            }
            return Relationships.Contains(univID);
        }

        /// <summary>
        /// Removes an advisee/advisor relationship from the user.
        /// </summary>
        /// <param name="univID">A user's university identifier.</param>
        /// <exception cref="Exception">If the user is an administrator.</exception>
        /// <exception cref="Exception">If the user does not have a relationship with the given user.</exception>
        public void RemoveRelationship(string univID)
        {
            if (Role.Equals(User.ADMINISTRATOR))
            {
                throw new Exception("User does not have advisor/advisee relationships! User is not a student or " +
                    "instructor!");
            }
            if (!HasRelationship(univID))
            {
                throw new Exception($"User {UnivID} does not have a relationship with user {univID}!");
            }
            Relationships.Remove(univID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) over the user's advisee/advisor relationships.
        /// </summary>
        /// <returns>An enumeator over the user's relationships.</returns>
        /// <exception cref="Exception">If the user is an administrator.</exception>
        public IEnumerator<string> GetRelationships()
        {
            if (Role.Equals(User.ADMINISTRATOR))
            {
                throw new Exception("User does not have advisor/advisee relationships! User is not a student or " +
                    "instructor!");
            }
            return Relationships.GetEnumerator();
        }

        /// <summary>
        /// Determines whether a instructor has the given department or not.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <returns>True if the instructor has the indicated department, false otherwise.</returns>
        public bool HasDepartment(int departmentID)
        {
            if (!Role.Equals(User.INSTRUCTOR))
            {
                throw new Exception("Cannot determine if the user has the indicated department. User is not a " +
                    "instructor!");
            }
            return Departments.Contains(departmentID);
        }

        /// <summary>
        /// Adds a department to the instructor.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        public void AddDepartment(int departmentID)
        {
            if (!Role.Equals(User.INSTRUCTOR))
            {
                throw new Exception("Cannot add department to user. User is not a instructor!");
            }
            if (HasDepartment(departmentID))
            {
                throw new Exception("Cannot add department to user. User already has department!");
            }
            Departments.Add(departmentID);
        }

        /// <summary>
        /// Removes a department from the instructor.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        public void RemoveDepartment(int departmentID)
        {
            if (!Role.Equals(User.INSTRUCTOR))
            {
                throw new Exception("Cannot remove department from user. User is not a instructor!");
            }
            if (!HasDepartment(departmentID))
            {
                throw new Exception("Cannot remove department from user. User does not have department!");
            }
            Departments.Remove(departmentID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) over the instructor's departments.
        /// </summary>
        /// <returns>An enumerator of the instructor's departments.</returns>
        public IEnumerator<int> GetDepartments()
        {
            if (!Role.Equals(User.INSTRUCTOR))
            {
                throw new Exception("Cannot get departments for user. User is not a instructor!");
            }
            return Departments.GetEnumerator();
        }

        /// <summary>
        /// Clears the departments an instructor belongs to.
        /// </summary>
        /// <exception cref="Exception">If the user is not an instructor.</exception>
        public void ClearDepartments()
        {
            if (!Role.Equals(INSTRUCTOR))
            {
                throw new Exception("Error: Unable to clear departments for the user, user is not an instructor!");
            }
            Departments.Clear();
        }

        /// <summary>
        /// Clears the majors a student has.
        /// </summary>
        /// <exception cref="Exception">If the user is not a student.</exception>
        public void ClearMajors()
        {
            if (!Role.Equals(STUDENT))
            {
                throw new Exception("Unable to clear majors for the user, user is not a student!");
            }
            Majors.Clear();
        }

        /// <summary>
        /// Clears the relationships an instructor or student has.
        /// </summary>
        /// <exception cref="Exception">If the user is an administrator.</exception>
        public void ClearRelationships()
        {
            if (Role.Equals(ADMINISTRATOR))
            {
                throw new Exception("Cannot clear the relationships for the user, user is not an instructor or student!");
            }
            Relationships.Clear();
        }

        public override string ToString()
        {
            return $"{UnivID} : {LastName}, {FirstName}";
        }
    }
}