using System;
using System.Collections.Generic;

namespace Coursely.Content.Classes
{
    /// <summary>
    /// Represents a course section.
    /// </summary>
    public class Section : IComparable<Section>
    {
        /// <summary>
        /// Constant that defines the valid days a course may be offered.
        /// </summary>
        public static readonly string[] VALID_DAYS = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        /// <summary>
        /// The unique section identifier.
        /// </summary>
        public int SectionID { get; }
        /// <summary>
        /// The identifer of the course the section belongs to.
        /// </summary>
        public int CourseID { get; }
        /// <summary>
        /// The identifier of the building the section is held in.
        /// </summary>
        public int BuildingID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// The semester the section is offered.
        /// </summary>
        public string Semester { get; }
        /// <summary>
        /// The year the section is offered.
        /// </summary>
        public int Year { get; }
        /// <summary>
        /// The room the section is held in.
        /// </summary>
        public int Room { get; set; }
        /// <summary>
        /// The first day the section begins and the beginning time of the section.
        /// </summary>
        public DateTime StartDateAndTime { get; set; }
        /// <summary>
        /// The last day of the section and the ending time of the section.
        /// </summary>
        public DateTime EndDateAndTime { get; set; }
        /// <summary>
        /// The current number of students enrolled in the section.
        /// </summary>
        public int CurrentEnrollment { get { return Students.Count; } }
        /// <summary>
        /// The maximum number of students that may enroll in the section.
        /// </summary>
        public int MaxEnrollment { get; set; }
        /// <summary>
        /// The instructors that are scheduled to teach the section.
        /// </summary>
        private HashSet<string> Instructors { get; } = new HashSet<string>();
        /// <summary>
        /// The students that are currently enrolled in the section.
        /// </summary>
        private HashSet<string> Students { get; } = new HashSet<string>();
        /// <summary>
        /// The days the section is held.
        /// </summary>
        private HashSet<string> Days { get; } = new HashSet<string>();

        /// <summary>
        /// Creates an instance of a section with the given parameters.
        /// </summary>
        /// <param name="sectionID">The section identifier.</param>
        /// <param name="courseID">The identifier for the course the section belongs to.</param>
        /// <param name="semester">The semester the section is offered.</param>
        /// <param name="year">The year the semester is offered.</param>
        public Section(int sectionID, int courseID, string semester, int year)
        {
            SectionID = sectionID;
            CourseID = courseID;
            Semester = semester;
            Year = year;
        }

        /// <summary>
        /// Determines if this section equals another object.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>True if the object is not null, is a section, and has the same section identifier as this section; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Section))
            {
                return false;
            }
            return (obj as Section).SectionID == SectionID;
        }

        /// <summary>
        /// Generates the unique hashcode for the section.
        /// </summary>
        /// <returns>The section identifier, as it is already guaranteed to be unique.</returns>
        public override int GetHashCode()
        {
            return SectionID;
        }

        /// <summary>
        /// Adds an instructor to the section.
        /// </summary>
        /// <param name="univID">The instructor's university identifier.</param>
        /// <exception cref="Exception">If the instructor is already schedule to teach the section.</exception>
        public void AddInstructor(string univID)
        {
            if (Instructors.Contains(univID))
            {
                throw new Exception("Unable to add instructor to section! The instructor is already scheduled to teach the section!");
            }
            Instructors.Add(univID);
        }

        /// <summary>
        /// Determines if an instructor is scheduled to teach a section.
        /// </summary>
        /// <param name="univID">The instructor's university identifier.</param>
        /// <returns>True if the instructor is scheduled to teach this section, false otherwise.</returns>
        public bool IsInstructorScheduled(string univID)
        {
            return Instructors.Contains(univID);
        }

        /// <summary>
        /// Removes an instructor from the section.
        /// </summary>
        /// <param name="univID">The instructor's university identifier.</param>
        /// <returns>True if the instructor was successfully removed, false if the instructor does not exist or was not successfully removed.</returns>
        public bool RemoveInstructor(string univID)
        {
            return Instructors.Remove(univID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) of all the instructors in the section.
        /// </summary>
        /// <returns>An enumerator over all the instructors in the section.</returns>
        public IEnumerator<string> GetInstructors()
        {
            return Instructors.GetEnumerator();
        }

        /// <summary>
        /// Adds a student to a section.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <exception cref="Exception">If the section is already at max capacity.</exception>
        /// <exception cref="Exception">If the student is already enrolled in the section.</exception>
        public void AddStudent(string univID)
        {
            if (Students.Count == MaxEnrollment)
            {
                throw new Exception("Unable to add student! The section is already at max enrollment!");
            }
            if (Students.Contains(univID))
            {
                throw new Exception("Unable to add student! The student is already enrolled in the section!");
            }
            Students.Add(univID);
        }

        /// <summary>
        /// Determines if a student is already enrolled in the section.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <returns>True if the student is already enrolled in the section, false otherwise.</returns>
        public bool IsStudentEnrolled(string univID)
        {
            return Students.Contains(univID);
        }

        /// <summary>
        /// Removes a student from a section.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <returns>True if the student was successfully removed, false if the student does not exist or was not successfully removed.</returns>
        public bool RemoveStudent(string univID)
        {
            return Students.Remove(univID);
        }

        /// <summary>
        /// Gets an enumerator (iterator) of all the students in the section.
        /// </summary>
        /// <returns>An enumerator over all the students in the section.</returns>
        public IEnumerator<string> GetStudents()
        {
            return Students.GetEnumerator();
        }

        /// <summary>
        /// Adds a day to the section.
        /// </summary>
        /// <param name="day">A day.</param>
        /// <exception cref="Exception">If the day is not valid.</exception>
        /// <exception cref="Exception">If the section is already held on the given day.</exception>
        public void AddDay(string day)
        {
            if (!IsDayValid(day))
            {
                throw new Exception($"Unable to schedule section for {day}! The day: {day}, is not valid.");
            }
            if (IsSectionHeldOnDay(day))
            {
                throw new Exception($"Unable to schedule section for {day}! The section is already scheduled for {day}!");
            }
            Days.Add(day);
        }

        /// <summary>
        /// Determines if this section is held on the given day.
        /// </summary>
        /// <param name="day">A day.</param>
        /// <returns>True if the section is held on the given day, false otherwise.</returns>
        public bool IsSectionHeldOnDay(String day)
        {
            return Days.Contains(day);
        }

        /// <summary>
        /// Removes a day from the section.
        /// </summary>
        /// <param name="day">A day.</param>
        /// <exception cref="Exception">If the day is not valid.</exception>
        /// <exception cref="Exception">If the section is not held on the given day.</exception>
        public void RemoveDay(string day)
        {
            if (!IsDayValid(day))
            {
                throw new Exception($"Unable to unschedule section for {day}! The day: {day}, is not valid.");
            }
            if (!IsSectionHeldOnDay(day))
            {
                throw new Exception($"Unable to unschedule section for {day}! The section is not scheduled for {day}!");
            }
            Days.Remove(day);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetDays()
        {
            return Days.GetEnumerator();
        }

        /// <summary>
        /// Determines if a given day is a valid day a section may be offered.
        /// </summary>
        /// <param name="day">The day to check.</param>
        /// <returns>True if the day is valid, false otherwise.</returns>
        public static bool IsDayValid(string day)
        {
            foreach (string item in VALID_DAYS)
            {
                if (day.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Compares this section to another.
        /// </summary>
        /// <param name="other">The other section to compare to.</param>
        /// <returns>The natural ordering of the start date and time.</returns>
        public int CompareTo(Section other)
        {
            return StartDateAndTime.CompareTo(other.StartDateAndTime);
        }

        /// <summary>
        /// Modifes a section with the given parameters.
        /// </summary>
        /// <param name="buildingID">A building identifier.</param>
        /// <param name="room">A room number.</param>
        /// <param name="startDateAndTime">A start time and date.</param>
        /// <param name="endDateAndTime">An end time and date.</param>
        /// <param name="maxEnrollment">A max enrollment value.</param>
        public void Modify(int buildingID, int room, DateTime startDateAndTime, DateTime endDateAndTime, 
            int maxEnrollment, List<string> instructors, List<string> days)
        {
            BuildingID = buildingID;
            Room = room;
            StartDateAndTime = startDateAndTime;
            EndDateAndTime = endDateAndTime;
            MaxEnrollment = maxEnrollment;
            // TODO: Handle updates to the instructors and days
            Instructors.Clear();
            foreach (var instructor in instructors)
            {
                instructors.Add(instructor);
            }
            Days.Clear();
            foreach (var day in days)
            {
                Days.Add(day);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"SO-{GetSectionNumberAsString()}";
        }

        /// <summary>
        /// Gets a string version of the section number.
        /// </summary>
        /// <returns>A string version of the section number.</returns>
        private string GetSectionNumberAsString()
        {
            string number = string.Copy(Number);
            while (number.Length < 3)
            {
                number = number.Insert(0, "0");
            }
            return number;
        }

        /// <summary>
        /// Determines if a section is at maximum enrollment.
        /// </summary>
        /// <returns>True if the section is at max enrollment, false otherwise.</returns>
        public bool IsSectionFull() => CurrentEnrollment == MaxEnrollment;

        /// <summary>
        /// Clears the days a course is offered.
        /// </summary>
        public void ClearDays()
        {
            Days.Clear();
        }

        /// <summary>
        /// Clears the instructors signed up to teach a section.
        /// </summary>
        public void ClearInstructors()
        {
            Instructors.Clear();
        }

        /// <summary>
        /// Clears the students enrolled in a section.
        /// </summary>
        public void ClearStudents()
        {
            Students.Clear();
        }
    }
}