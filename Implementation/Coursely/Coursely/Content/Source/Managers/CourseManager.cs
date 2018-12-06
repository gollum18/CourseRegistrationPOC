using System;
using System.Linq;
using System.Collections.Generic;

using Coursely.Content.Cache;
using Coursely.Content.Classes;

namespace Coursely.Content.Managers
{
    /// <summary>
    /// Manages instance of the Course and Section classes. Allows for creating, retrieving, deleting, and updating 
    /// instances of said classes.
    /// </summary>
    public class CourseManager
    {
        //
        // ATTRIBUTES
        //
        private MemoryCache<int, Course> CourseCache;
        private MemoryCache<int, Section> SectionCache;

        /// <summary>
        /// Private static instance only accessible through InstanceOf. Enforces Singleton design pattern.
        /// </summary>
        private static CourseManager Instance { get; set; }

        //
        // CONSTRUCTORS
        //

        /// <summary>
        /// Private constructor to enforce the Singleton design pattern.
        /// </summary>
        private CourseManager()
        {
            CourseCache = new MemoryCache<int, Course>();
            SectionCache = new MemoryCache<int, Section>();
        }

        /// <summary>
        /// Returns an instance of a CourseManager.
        /// </summary>
        /// <returns>CourseManager</returns>
        public static CourseManager InstanceOf()
        {
            if (Instance == null)
            {
                Instance = new CourseManager();
            }
            return Instance;
        }

        /// <summary>
        /// Logical layer method for adding a course.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <param name="name">The course name.</param>
        /// <param name="number">The course number.</param>
        /// <param name="credits">The credit hours for the course.</param>
        /// <param name="archived">Whether the course is archived.</param>
        /// <param name="description">The description for the course.</param>
        /// <param name="prerequisites">THe courses prerequisites, a list of (courseID, groupID pairs).</param>
        /// <returns>True if the course was succsfully added, false otherwise.</returns>
        public bool AddCourse(int departmentID, string name, string number, int credits,
            bool archived, string description, List<Tuple<int, int>> prerequisites)
        {
            if (!DatabaseManager.InstanceOf().DoesDepartmentExist(departmentID))
            {
                return false;
            }
            if (DatabaseManager.InstanceOf().DoesCourseExist(departmentID, number))
            {
                return false;
            }
            return DatabaseManager.InstanceOf().AddCourse(departmentID, name, number, credits, archived, description, prerequisites);
        }

        /// <summary>
        /// Logical layer method for adding a section.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <param name="buildingID">The building identifier.</param>
        /// <param name="number">The section number.</param>
        /// <param name="semester">The semester the section is offered.</param>
        /// <param name="year">The year the section is offered.</param>
        /// <param name="room">The room number the section is housed in.</param>
        /// <param name="startDateAndTime">The start date and time of the section.</param>
        /// <param name="endDateAndTime">The end date and time of the section.</param>
        /// <param name="maxEnrollment">THe max enrollment for a section.</param>
        /// <returns>True if the section was added successfully, false otherwise.</returns>
        public bool AddSection(int courseID, int buildingID, string number, string semester, int year, int room,
            DateTime startDateAndTime, DateTime endDateAndTime, int maxEnrollment, List<string> instructors, 
            List<string> days)
        {
            if (!CourseCache.Contains(courseID) && 
                !DatabaseManager.InstanceOf().DoesCourseExist(courseID))
            {
                return false;
            }
            if (!UniversityManager.InstanceOf().HasBuildingCached(buildingID) && 
                !DatabaseManager.InstanceOf().DoesBuildingExist(buildingID))
            {
                return false;
            }
            if (DatabaseManager.InstanceOf().DoesSectionExist(new Tuple<string, int>(semester, year), courseID, 
                number))
            {
                return false;
            }
            return DatabaseManager.InstanceOf().AddSection(courseID, buildingID, number, semester, year, room,
                startDateAndTime, endDateAndTime, maxEnrollment, instructors, days);
        }

        /// <summary>
        /// Determines whether a course exists with the given course identifier.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <returns>True if a course exists, false otherwise.</returns>
        public bool DoesCourseExist(int courseID)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesCourseExist(courseID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if a course exists in the given department with the given course number.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <param name="number">A course number.</param>
        /// <returns>True if a course exists, false otherwise.</returns>
        public bool DoesCourseExist(int departmentID, string number)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesCourseExist(departmentID, number);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if a course exists with the given name.
        /// </summary>
        /// <param name="name">A course name.</param>
        /// <returns>True if a course exists, false otherwise.</returns>
        public bool DoesCourseExist(string name)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesCourseExist(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if a section exists with the given section identifier.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>True if a section exists, false otherwise.</returns>
        public bool DoesSectionExist(int sectionID)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesSectionExist(sectionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if a section exists for the given course with the given course number in the given term.
        /// </summary>
        /// <param name="term">A term consisting of a semester and year.</param>
        /// <param name="courseID">A course identifier.</param>
        /// <param name="number">A section number.</param>
        /// <returns>True if a section exists, false otherwise.</returns>
        public bool DoesSectionExist(Tuple<string, int> term, int courseID, string number)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesSectionExist(term, courseID, number);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a course from the course manager.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns>A course from the course manager if it exists, otherwise one from the database.</returns>
        /// <exception cref="Exception">If a course was not found with the given identifier in cache, 
        /// or in the database.</exception>
        public Course GetCourse(int courseID)
        {
            if (CourseCache.Contains(courseID))
            {
                return CourseCache.Get(courseID);
            }
            try
            {
                Course course = DatabaseManager.InstanceOf().GetCourse(courseID);
                CourseCache.Add(course.CourseID, course);
                return course;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a course identifier for a course using the department identifier and course number.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <param name="number">A course number.</param>
        /// <returns>A department identifier if it exists.</returns>
        public int GetCourseID(int departmentID, string number)
        {
            try
            {
                return DatabaseManager.InstanceOf().GetCourseID(departmentID, number);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of all courses from the cache and database.
        /// </summary>
        /// <returns>A list of courses.</returns>
        public List<Course> GetCourses()
        {
            try
            {
                List<Course> courses = DatabaseManager.InstanceOf().GetCourses();
                foreach (var course in CourseCache.GetValues())
                {
                    courses.Add(course);
                }
                foreach (var course in courses)
                {
                    if (!CourseCache.Contains(course.CourseID))
                    {
                        CourseCache.Add(course.CourseID, course);
                    }
                }
                courses.Sort();
                return courses;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all of the courses that belong to the specified department.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <returns>A list of courses that belong to the specified department.</returns>
        public List<Course> GetCourses(int departmentID)
        {
            try
            {
                List<Course> courses = DatabaseManager.InstanceOf().GetCourses(departmentID);
                courses.AddRange(CourseCache.GetValues().Where(course => course.DepartmentID == departmentID));
                foreach (Course course in courses)
                {
                    if (!CourseCache.Contains(course.CourseID))
                    {
                        CourseCache.Add(course.CourseID, course);
                    }
                }
                courses.Sort();
                return courses;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a section with the corresponding section identifier.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>A section from the database if it exists.</returns>
        public Section GetSection(int sectionID)
        {
            if (SectionCache.Contains(sectionID))
            {
                return SectionCache.Get(sectionID);
            }
            try
            {
                Section section = DatabaseManager.InstanceOf().GetSection(sectionID);
                SectionCache.Add(sectionID, section);
                return section;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the sections for a course with filtered by the specified term.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <param name="term">A term; consists of a semester and year.</param>
        /// <returns>A section if it exists.</returns>
        /// <exception cref="Exception">If the specified course does not exist.</exception>
        public List<Section> GetSections(int courseID, Tuple<string, int> term)
        {
            if (!DatabaseManager.InstanceOf().DoesCourseExist(courseID))
            {
                throw new Exception("The specified course does not exist!");
            }
            try
            {
                List<Section> sections = DatabaseManager.InstanceOf().GetSections(courseID, term);
                sections.AddRange(SectionCache.GetValues().Where(section => section.CourseID == courseID && 
                    section.Semester == term.Item1 && section.Year == term.Item2));
                foreach (Section section in sections)
                {
                    if (!SectionCache.Contains(section.SectionID))
                    {
                        SectionCache.Add(section.SectionID, section);
                    }
                }
                sections.Sort();
                return sections;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if the course manager has a course cached using the specified course identifier.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <returns>True if the course manager has the course cached, false otherwise.</returns>
        public bool HasCourseCached(int courseID)
        {
            return CourseCache.Contains(courseID);
        }

        /// <summary>
        /// Determines if the course manager has the specified section cached based on the 
        /// section identifier.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>True if the course manager has the section cached, false otherwise.</returns>
        public bool HasSectionCached(int sectionID)
        {
            return SectionCache.Contains(sectionID);
        }

        /// <summary>
        /// Modifies a course with the specified parameters.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <param name="number">A course number.</param>
        /// <param name="credits">The amount of credits the course is worth.</param>
        /// <param name="archived">Whether the course is archived or not.</param>
        /// <param name="description">The course description.</param>
        /// <returns>True if the cousre was successfully modified, false otherwise.</returns>
        public bool ModifyCourse(int courseID, string number, int credits, bool archived, string description)
        {
            try
            {
                if (!DatabaseManager.InstanceOf().DoesCourseExist(courseID))
                {
                    throw new Exception($"No course with the given identifier {courseID} exists!");
                }
                if (DatabaseManager.InstanceOf().ModifyCourse(courseID, number, credits, archived, description) &&
                        HasCourseCached(courseID))
                {
                    Course course = CourseCache.Get(courseID);
                    course.Number = number;
                    course.Credits = credits;
                    course.Archived = archived;
                    course.Description = description;
                    course = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Modifies the prerequisites of a course to those given.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <param name="prerequisites">A list of (courseID, group) pairs.</param>
        /// <returns>True if the prerequisites were modified successfully, false otherwise.</returns>
        public bool ModifyPrerequisites(int courseID, List<Tuple<int, int>> prerequisites)
        {
            if (!DoesCourseExist(courseID))
            {
                throw new Exception("Error: Cannot modify prerequisites, course does not exist!");
            }
            if (prerequisites == null)
            {
                throw new Exception("Error: Cannot modify prerequisites, null value passed!");
            }
            try
            {
                if (DatabaseManager.InstanceOf().ModifyPrerequisites(courseID, prerequisites) && 
                        HasCourseCached(courseID))
                {
                    Course course = CourseCache.Get(courseID);
                    course.ClearPrerequisites();
                    foreach (var prereq in prerequisites)
                    {
                        course.AddPrerequisite(prereq);
                    }
                    course = null;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Modifies a section with the indicated parameters.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <param name="buildingID">A building identifier.</param>
        /// <param name="room">A room number.</param>
        /// <param name="startDateAndTime">A start date and time.</param>
        /// <param name="endDateAndTime">An end date and time.</param>
        /// <param name="maxEnrollment">A new max enrollment value.</param>
        /// <param name="days">The days the section is offered.</param>
        /// <param name="instructors">The instructors who teach the section.</param>
        /// <param name="students">The students enrolled the section.</param>
        /// <returns>True if the section was successfully modified, false otherwise.</returns>
        public bool ModifySection(int sectionID, int buildingID, int room,
            DateTime startDateAndTime, DateTime endDateAndTime, int maxEnrollment, 
            List<string> days, List<string> instructors)
        {
            try
            {
                if (!DatabaseManager.InstanceOf().DoesSectionExist(sectionID))
                {
                    throw new Exception($"Error: No section exists with the section identifier: {sectionID}!");
                }
                if (!UniversityManager.InstanceOf().DoesBuildingExist(buildingID))
                {
                    throw new Exception($"Error: No building exists with the building identifier: {buildingID}!");
                }
                if (DatabaseManager.InstanceOf().ModifySection(sectionID, buildingID, room, startDateAndTime,
                    endDateAndTime, maxEnrollment) && HasSectionCached(sectionID))
                {
                    Section section = SectionCache.Get(sectionID);
                    section.BuildingID = buildingID;
                    section.Room = room;
                    section.StartDateAndTime = startDateAndTime;
                    section.EndDateAndTime = endDateAndTime;
                    section.MaxEnrollment = maxEnrollment;
                    ModifySectionDays(sectionID, days);
                    ModifySectionInstructors(sectionID, instructors);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Modifies the days in a section.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <param name="days">A lsit of dasy.</param>
        private bool ModifySectionDays(int sectionID, List<string> days)
        {
            try
            {
                if (DatabaseManager.InstanceOf().ModifySectionDays(sectionID, days) && 
                        HasSectionCached(sectionID))
                {
                    Section section = SectionCache.Get(sectionID);
                    section.ClearDays();
                    foreach (var day in days)
                    {
                        section.AddDay(day);
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Modifies the instructors in a section.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <param name="instructors">A list of instructor university idenfiers.</param>
        private bool ModifySectionInstructors(int sectionID, List<string> instructors)
        {
            try
            {
                if (DatabaseManager.InstanceOf().ModifySectionInstructors(sectionID, instructors) &&
                        HasSectionCached(sectionID))
                {
                    Section section = SectionCache.Get(sectionID);
                    section.ClearInstructors();
                    foreach (var instructor in instructors)
                    {
                        section.AddInstructor(instructor);
                    }
                    section = null;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}