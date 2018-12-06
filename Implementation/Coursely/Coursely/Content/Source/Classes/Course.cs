using System;
using System.Collections.Generic;

namespace Coursely.Content.Classes
{
    /// <summary>
    /// Represents a course in a university.
    /// </summary>
    public class Course : IComparable<Course>
    {
        /// <summary>
        /// The unique course identifier.
        /// </summary>
        public int CourseID { get; }
        /// <summary>
        /// The identifier for the department the course belongs to.
        /// </summary>
        public int DepartmentID { get; set; }
        /// <summary>
        /// The course name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The course number.
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// The credit hours the course is worth.
        /// </summary>
        public int Credits { get; set; }
        /// <summary>
        /// Whether the course is archived or not.
        /// </summary>
        public bool Archived { get; set; }
        /// <summary>
        /// THe course description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The prerequisites for the course.
        /// </summary>
        private HashSet<Tuple<int, int>> Prerequisites { get; } = new HashSet<Tuple<int, int>>();

        /// <summary>
        /// Creates a course with the given parameters.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <param name="name">The course name.</param>
        public Course(int courseID, string name)
        {
            CourseID = courseID;
            Name = name;
        }

        /// <summary>
        /// Determines if this course is equal to another.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>False if the obj is null, is not a course, or does not have the same course identifier as this 
        /// course, true otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Course))
            {
                return false;
            }
            return (obj as Course).CourseID == CourseID;
        }

        /// <summary>
        /// Gets the unique hash for this course.
        /// </summary>
        /// <returns>The course identifier. Good enough in this instance as course identifiers are already guaranteed 
        /// to be unique.</returns>
        public override int GetHashCode()
        {
            return CourseID;
        }

        /// <summary>
        /// Compares this course to another.
        /// </summary>
        /// <param name="other">The other course to compare to.</param>
        /// <returns>The natural ordering of this course's name to the other course's name.</returns>
        public int CompareTo(Course other)
        {
            return Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Adds a prerequisite to the course.
        /// </summary>
        /// <param name="prerequisite"></param>
        public void AddPrerequisite(Tuple<int, int> prerequisite)
        {
            if (HasPrerequisite(prerequisite.Item1))
            {
                throw new Exception("");
            }
            Prerequisites.Add(prerequisite);
        }

        /// <summary>
        /// Determines whether a prerequisite is required by the course.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <returns>True if a prerequisite with the given course identifier is listed by the course, false 
        /// otherwise.</returns>
        public bool HasPrerequisite(int courseID)
        {
            if (Prerequisites.Count == 0)
            {
                return false;
            }
            bool hasPrereq = false;
            using (var enumerator = Prerequisites.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Item1.Equals(courseID))
                    {
                        hasPrereq = true;
                    }
                }
            }
            return hasPrereq;
        }

        /// <summary>
        /// Removes a prerequisite from the course.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        public void RemovePrerequisite(int courseID)
        {
            if (!HasPrerequisite(courseID))
            {
                throw new Exception();
            }
            using (var enumerator = Prerequisites.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Item1.Equals(courseID))
                    {
                        Prerequisites.Remove(enumerator.Current);
                        break;
                    }
                }
            }
        }

        public IEnumerator<Tuple<int, int>> GetPrerequisites()
        {
            return Prerequisites.GetEnumerator();
        }

        /// <summary>
        /// Modifies a course to the given parameters.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="credits"></param>
        /// <param name="archived"></param>
        /// <param name="description"></param>
        public void Modify(string number, int credits, bool archived, string description, 
            List<Tuple<int, int>> prerequisites)
        {
            Number = number;
            Credits = credits;
            Archived = archived;
            Description = description;
            Prerequisites.Clear();
            foreach (var prereq in prerequisites)
            {
                Prerequisites.Add(prereq);
            }
        }

        /// <summary>
        /// Generates a string representation of this course object.
        /// </summary>
        /// <returns>The course number.</returns>
        public override string ToString()
        {
            string str = Number.ToString();
            while (str.Length < 4)
            {
                str.Insert(0, "0");
            }
            return str;
        }

        /// <summary>
        /// Clears the prerequisites for a course.
        /// </summary>
        public void ClearPrerequisites()
        {
            Prerequisites.Clear();
        }
    }
}