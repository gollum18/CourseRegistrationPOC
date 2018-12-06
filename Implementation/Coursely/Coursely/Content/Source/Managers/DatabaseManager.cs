using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

using Coursely.Content.Classes;

// This is starting to get rather monolithic.

namespace Coursely.Content.Managers
{
    /// <summary>
    /// Exists within the data layer. Interacts with the Coursely database and serves the Managers in the 
    /// logical layer.
    /// </summary>
    public class DatabaseManager
    {
        //
        // CONSTANTS
        //

        public static readonly int NOT_FOUND = -1;

        //
        // ATTRIBUTES
        //

        /// <summary>
        /// Private static instance only accessible through InstanceOf. Enforces Singleton design pattern.
        /// </summary>
        private static DatabaseManager Instance { get; set; }

        /// <summary>
        /// Used to connect to the SQL Express database.
        /// </summary>
        private string ConnectionString { get; } = System.Configuration.
            ConfigurationManager.ConnectionStrings["CourselyDBContext"].ConnectionString;

        /// <summary>
        /// Used to control concurrent read-write access to the database.
        /// </summary>
        private ReaderWriterLockSlim ReadWriteLock = new ReaderWriterLockSlim();

        //
        // CONSTRUCTORS
        //

        /// <summary>
        /// Private constructor to enforce the Singleton design pattern.
        /// </summary>
        private DatabaseManager() { }

        //
        // METHODS
        //

        /// <summary>
        /// Returns an instance of a UserManager.
        /// </summary>
        /// <returns>UserManager</returns>
        public static DatabaseManager InstanceOf()
        {
            if (Instance == null)
            {
                Instance = new DatabaseManager();
            }
            return Instance;
        }

        /// <summary>
        /// Adds a building to the database.
        /// </summary>
        /// <param name="name">The name of the building.</param>
        /// <param name="abbreviation">The abbreviation of the building.</param>
        /// <returns>True if the building was successfully added, false otherwise.</returns>
        public bool AddBuilding(string name, string abbreviation)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Buildings (Name, Abbreviation) " +
                                "VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            comm.Parameters.AddWithValue("@1", abbreviation);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Adds a course to the database.
        /// </summary>
        /// <param name="departmentID">The identifier for the department the course belongs to.</param>
        /// <param name="name">The name of the course.</param>
        /// <param name="number">The number of the course.</param>
        /// <param name="credits">The credit hours for the course.</param>
        /// <param name="archived">Whether the course is archived.</param>
        /// <param name="description">The description of the course.</param>
        /// <returns>True if the course was successfully added, false otherwise.</returns>
        public bool AddCourse(int departmentID, string name, string number, int credits, 
            bool archived, string description, List<Tuple<int, int>> prerequisites)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld) {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Courses (DepartmentID, Name, Number, Credits, Archived, Description) " +
                                "VALUES (@0, @1, @2, @3, @4, @5)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            comm.Parameters.AddWithValue("@1", name);
                            comm.Parameters.AddWithValue("@2", number);
                            comm.Parameters.AddWithValue("@3", credits);
                            comm.Parameters.AddWithValue("@4", archived);
                            comm.Parameters.AddWithValue("@5", description);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                        // Get the course id
                        int courseID = -1;
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT CourseID FROM Courses WHERE DepartmentID = @0 AND UPPER(Number) = UPPER(@1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            comm.Parameters.AddWithValue("@1", number);
                            object obj = comm.ExecuteScalar();
                            if (obj != null) {
                                courseID = Convert.ToInt32(obj);
                            }
                        }
                        // Adds the prerequisites into the database
                        if (result && prerequisites != null)
                        {
                            foreach (var prereq in prerequisites)
                            {
                                using (SqlCommand comm = new SqlCommand(
                                    "INSERT INTO Prerequisites VALUES (@0, @1, @2)", conn))
                                {
                                    comm.Parameters.AddWithValue("@0", courseID);
                                    comm.Parameters.AddWithValue("@1", prereq.Item1);
                                    comm.Parameters.AddWithValue("@2", prereq.Item2);
                                    comm.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Adds a department to the database.
        /// </summary>
        /// <param name="schoolID">The identifier for the school the department belongs to.</param>
        /// <param name="name">The name of the department.</param>
        /// <param name="abbreviation">The abbreviation of the departments name.</param>
        /// <returns></returns>
        public bool AddDepartment(int schoolID, string name, string abbreviation)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld) {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Departments (SchoolID, Name, Abbreviation) " +
                                "VALUES (@0, @1, @2)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", schoolID);
                            comm.Parameters.AddWithValue("@1", name);
                            comm.Parameters.AddWithValue("@2", abbreviation);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Adds a major to the database.
        /// </summary>
        /// <param name="departmentID">The identifier of the department the major belongs to.</param>
        /// <param name="name">The name of the major.</param>
        /// <returns></returns>
        public bool AddMajor(int departmentID, string name)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Majors (DepartmentID, Name) " +
                                "VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            comm.Parameters.AddWithValue("@1", name);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Adds a school to the database.
        /// </summary>
        /// <param name="name">The name of the department.</param>
        /// <param name="abbreviation">The abbreviation of the department's name.</param>
        /// <returns></returns>
        public bool AddSchool(string name, string abbreviation)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Schools (Name, Abbreviation) " +
                                "VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            comm.Parameters.AddWithValue("@1", abbreviation);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Adds a section to the database.
        /// </summary>
        /// <returns>Whether the section was added successfully or not.</returns>
        public bool AddSection(int courseID, int buildingID, string number, string semester, int year, int room, 
            DateTime startDateAndTime, DateTime endDateAndTime, int maxEnrollment, List<string> instructors, 
            List<string> days)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Sections (CourseID, BuildingID, Number, Semester, Year, Room, StartDateAndTime, " +
                                "EndDateAndTime, MaxEnrollment) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            comm.Parameters.AddWithValue("@1", buildingID);
                            comm.Parameters.AddWithValue("@2", number);
                            comm.Parameters.AddWithValue("@3", semester);
                            comm.Parameters.AddWithValue("@4", year);
                            comm.Parameters.AddWithValue("@5", room);
                            comm.Parameters.AddWithValue("@6", startDateAndTime);
                            comm.Parameters.AddWithValue("@7", endDateAndTime);
                            comm.Parameters.AddWithValue("@8", maxEnrollment);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                        int sectionID = -1;
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT SectioNID FROM Sections WHERE CourseID = @0 AND Number = @1 AND " +
                                "Semester = @2 AND Year = @3", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            comm.Parameters.AddWithValue("@1", number);
                            comm.Parameters.AddWithValue("@2", semester);
                            comm.Parameters.AddWithValue("@3", year);
                            sectionID = Convert.ToInt32(comm.ExecuteScalar());
                        }
                        if (result && sectionID != -1) {
                            // Adds the instructors for the section into the database
                            foreach (var instructor in instructors)
                            {
                                using (SqlCommand comm = new SqlCommand(
                                    "INSERT INTO SectionInstructors VALUES (@0, @1)", conn))
                                {
                                    comm.Parameters.AddWithValue("@0", sectionID);
                                    comm.Parameters.AddWithValue("@1", instructor);
                                    comm.ExecuteNonQuery();
                                }
                            }
                            // Adds the days for the section into the database
                            foreach (var day in days)
                            {
                                using (SqlCommand comm = new SqlCommand(
                                    "INSERT INTO SectionDays VALUES (@0, @1)", conn))
                                {
                                    comm.Parameters.AddWithValue("@0", sectionID);
                                    comm.Parameters.AddWithValue("@1", day);
                                    comm.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Assigns a student to an advisor.
        /// </summary>
        /// <param name="instructorID">The instructor's university ID.</param>
        /// <param name="studentID">The student's university ID.</param>
        /// <returns>True if the advisor was successfully assigned, false otherwise.</returns>
        public bool AssignAdvisorToStudent(string instructorID, string studentID)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Advisees VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", instructorID);
                            comm.Parameters.AddWithValue("@1", studentID);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Assigns a student a final grade in a course.
        /// </summary>
        /// <param name="studentID">The student's university ID.</param>
        /// <param name="courseID">The identifier for the course.</param>
        /// <param name="grade">The grade received in the course.</param>
        /// <returns>True if the grade was successfully assigned, false otherwise.</returns>
        public bool AssignGradeToStudent(string studentID, int courseID, string grade)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Grades (StudentID, CourseID, Grade) VALUES (@0, @1, @2)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", studentID);
                            comm.Parameters.AddWithValue("@1", courseID);
                            comm.Parameters.AddWithValue("@2", grade);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Assigns a student to a major.
        /// </summary>
        /// <param name="studentID">The student's university ID.</param>
        /// <param name="majorID">The identifier for the major.</param>
        /// <returns>True if the major was successfully assigned, false otherwise.</returns>
        public bool AssignMajorToStudent(string studentID, int majorID)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO StudentMajors VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", studentID);
                            comm.Parameters.AddWithValue("@1", majorID);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Assigns a day to a section.
        /// </summary>
        /// <param name="sectionID">The identifier of the section.</param>
        /// <param name="day">The day to assign.</param>
        /// <returns>True if the day was successfully assigned to the indicated section, false otherwise.</returns>
        public bool AssignDayToSection(int sectionID, string day)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO SectionDays VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.Parameters.AddWithValue("@1", day);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Assings majors to a student.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <param name="majors">The majors to assign to the student.</param>
        /// <returns>True if the majors were successfully assigned, false otherwise.</returns>
        public bool AssignMajorsToStudent(string univID, List<int> majors)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        foreach (var major in majors) {
                            using (SqlCommand comm = new SqlCommand(
                                "INSERT INTO StudentMajors VALUES (@0, @1)", conn))
                            {
                                comm.Parameters.AddWithValue("@0", univID);
                                comm.Parameters.AddWithValue("@1", major);
                                if (comm.ExecuteNonQuery() == 0)
                                {
                                    throw new Exception("Error: An issue occured assigning the majors to the student!");
                                }
                            }
                        }
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Assigns departments to an instructor.
        /// </summary>
        /// <param name="univID">An instructor's university identifier.</param>
        /// <param name="departments">The departments to assign.</param>
        /// <returns>True if the departments are assigned correctly, false otherwise.</returns>
        public bool AssignDepartmentsToInstructor(string univID, List<int> departments)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                        conn.Open();
                        foreach (var department in departments)
                        {
                            using (SqlCommand comm = new SqlCommand(
                                "INSERT INTO DepartmentInstructors (DepartmentID, InstructorID) VALUES (@1, @0)", conn))
                            {
                                comm.Parameters.AddWithValue("@0", univID);
                                comm.Parameters.AddWithValue("@1", department);
                                if (comm.ExecuteNonQuery() == 0)
                                {
                                    throw new Exception("Error: An issue occured assigning the majors to the student!");
                                }
                            }
                        }
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Assigns advisors to a student.
        /// </summary>
        /// <param name="univID">The student's university identifiers.</param>
        /// <param name="advisors">The advisors to assign to the student.</param>
        /// <returns>True if the advisors are assigned correctly, false otherwise.</returns>
        public bool AssignAdvisorsToStudent(string univID, List<string> advisors)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                        conn.Open();
                        foreach (var advisor in advisors)
                        {
                            using (SqlCommand comm = new SqlCommand(
                                "INSERT INTO Advisees (AdvisorID, StudentID) VALUES (@1, @0)", conn))
                            {
                                comm.Parameters.AddWithValue("@0", univID);
                                comm.Parameters.AddWithValue("@1", advisor);
                                if (comm.ExecuteNonQuery() == 0)
                                {
                                    throw new Exception("Error: An issue occured assigning the majors to the student!");
                                }
                            }
                        }
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Assign an instructor to a section.
        /// </summary>
        /// <param name="sectionID">The identifier of a section.</param>
        /// <param name="instructorID">The university ID of an instructor.</param>
        /// <returns>True if the instructor was successfully assigned to the indicated section, false otherwise.</returns>
        public bool AssignInstructorToSection(int sectionID, string instructorID)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO SectionInstructors (SectionID, InstructorID) VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.Parameters.AddWithValue("@1", instructorID);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Assigns a semester to a course.
        /// </summary>
        /// <param name="courseID">The identifier of the course to assign the semester to.</param>
        /// <param name="semester">The semester to be assigned.</param>
        /// <returns>True if the semester was successfully assigned to the cousre, false otherwise.</returns>
        public bool AssignSemesterToCourse(int courseID, string semester)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO CourseSemesters (CourseID, Semester) VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            comm.Parameters.AddWithValue("@1", semester);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Assigs a student to a section.
        /// </summary>
        /// <param name="sectionID">The identifier of a section.</param>
        /// <param name="studentID">The univeristy ID of a student.</param>
        /// <returns>True if the student was successfull assigned to the indicated section, false otherwise.</returns>
        public bool AssignStudentToSection(int sectionID, string studentID)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO SectionStudents (SectionID, StudentID) VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.Parameters.AddWithValue("@1", studentID);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Changes a user's email to that specified.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="email">The user's new email.</param>
        /// <returns>True if the email was updated successfully, false otherwise.</returns>
        public bool ChangeEmail(string univID, string email)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Users SET Email = @1 WHERE UnivID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            comm.Parameters.AddWithValue("@1", email);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Changes a users password and salt to those indicated.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="password">The user's new password.</param>
        /// <param name="salt">The user's new salt.</param>
        /// <returns>True if the password was successfully updated, false otherwise.</returns>
        public bool ChangePassword(string univID, string password, string salt)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Users SET Password = @1, Salt = @2", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            comm.Parameters.AddWithValue("@1", password);
                            comm.Parameters.AddWithValue("@2", salt);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Attempts to create a user with the specified parameters.
        /// </summary>
        /// <param name="univID">The unviersity ID of the new user.</param>
        /// <param name="firstName">The first name of the new user.</param>
        /// <param name="lastName">The last name of the new user.</param>
        /// <param name="email">The email of the new user.</param>
        /// <param name="role">The role of the new user.</param>
        /// <param name="password">The hashed and salted password of the new user.</param>
        /// <param name="salt">The salt of the new password.</param>
        /// <returns>True if the user was successfully created, false otherwise.</returns>
        public bool CreateUser(string univID, string firstName, string lastName, string email, 
            string role, string password, string salt)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        // Create the user
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Users VALUES (@0, @1, @2, @3, @4, @5, @6)", conn))
                        {
                            // Lots of parameters here.
                            comm.Parameters.AddWithValue("@0", univID);
                            comm.Parameters.AddWithValue("@1", firstName);
                            comm.Parameters.AddWithValue("@2", lastName);
                            comm.Parameters.AddWithValue("@3", email);
                            comm.Parameters.AddWithValue("@4", role);
                            comm.Parameters.AddWithValue("@5", password);
                            comm.Parameters.AddWithValue("@6", salt);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines if the specified building exists.
        /// </summary>
        /// <param name="name">The bildings name.</param>
        /// <returns>True if the building exists, false otherwise.</returns>
        public bool DoesBuildingExist(string name)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT BuildingID FROM Buildings WHERE Name = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database.");
            }
            return result;
        }

        /// <summary>
        /// Determines if a building exists by using the building identifer.
        /// </summary>
        /// <param name="buildingID">The building identifier.</param>
        /// <returns>True if the building exists, false otherwise.</returns>
        public bool DoesBuildingExist(int buildingID)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT BuildingID FROM Buildings WHERE BuildingID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", buildingID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database.");
            }
            return result;
        }

        /// <summary>
        /// Determines if a course exists with the given course identifier.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <returns>True if a course exists, false otherwise.</returns>
        public bool DoesCourseExist(int courseID)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT CourseID FROM Courses WHERE CourseID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception();
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines if a course exists in the given department with the given course number.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <param name="number">A course number.</param>
        /// <returns>True if a course exists, false otherwise.</returns>
        public bool DoesCourseExist(int departmentID, string number)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT CourseID FROM Courses WHERE DepartmentID = @0 AND Number = @1", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            comm.Parameters.AddWithValue("@1", number);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines if a course exists with the given name.
        /// </summary>
        /// <param name="name">A course name.</param>
        /// <returns>True if a course exists, false otherwise.</returns>
        public bool DoesCourseExist(string name)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT CourseID FROM Courses WHERE UPPER(Name) = UPPER(@0)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines whether a department exists within the given school with the given name.
        /// </summary>
        /// <param name="schoolID">A school identifier.</param>
        /// <param name="name">A department name.</param>
        /// <returns>True if a department exists, false otherwise.</returns>
        public bool DoesDepartmentExist(int schoolID, string name)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT DepartmentID FROM Departments WHERE SchoolID = @0 AND " +
                                "UPPER(Name) = Upper(@1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", schoolID);
                            comm.Parameters.AddWithValue("@1", name);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Determines if a department exists by using the department identifier.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <returns>True if the department exists, false otherwise.</returns>
        public bool DoesDepartmentExist(int departmentID)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT DepartmentID FROM Departments WHERE DepartmentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database.");
            }
            return result;
        }

        /// <summary>
        /// Determines whether a department exists with the given abbreviation.
        /// </summary>
        /// <param name="abbreviation">An abbreviation of a department name.</param>
        /// <returns>True if a department exists, false otherwise.</returns>
        public bool DoesDepartmentExist(string abbreviation)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT DepartmentID FROM Departments WHERE UPPER(Abbreviation) = UPPER(@0)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", abbreviation);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines whether a given major exists or not based on its major identifier.
        /// </summary>
        /// <param name="majorID">A major identifier.</param>
        /// <returns>True if a major exists with the given major identifier, false otherwise.</returns>
        public bool DoesMajorExist(int majorID)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT MajorID FROM Majors WHERE MajorID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", majorID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines if the specified major exists.
        /// </summary>
        /// <param name="departmentID">The identifier for the department the major belongs to.</param>
        /// <param name="name">The name of the major.</param>
        /// <returns>True if the major exists, false otherwise.</returns>
        public bool DoesMajorExist(int departmentID, string name)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand("SELECT MajorID FROM Majors WHERE " +
                            "DepartmentID = @0 AND Name = @1", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            comm.Parameters.AddWithValue("@1", name);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database.");
            }
            return result;
        }

        /// <summary>
        /// Determines if the school exists.
        /// </summary>
        /// <param name="name">The name of the school.</param>
        /// <returns>True if the school exists, false otherwise.</returns>
        public bool DoesSchoolExist(string name)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT SchoolID FROM Schools WHERE Name = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database.");
            }
            return result;
        }

        /// <summary>
        /// Determines whether a school exists with the given school identifier.
        /// </summary>
        /// <param name="schoolID">The school identifier.</param>
        /// <returns>True if the school exists, false otherwise.</returns>
        public bool DoesSchoolExist(int schoolID)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT SchoolID FROM Schools WHERE SchoolID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", schoolID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else {
                throw new Exception("Unable to connect to the database.");
            }
            return result;
        }

        /// <summary>
        /// Determines if a section exists with the given section identifier.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>True if a section exists, false otherwise.</returns>
        public bool DoesSectionExist(int sectionID)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT SectionID FROM Sections WHERE SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
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
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT SectionID FROM Sections WHERE Semester = @0 AND Year = @1 AND " +
                                "CourseID = @2 AND Number = @3", conn))
                        {
                            comm.Parameters.AddWithValue("@0", term.Item1);
                            comm.Parameters.AddWithValue("@1", term.Item2);
                            comm.Parameters.AddWithValue("@2", courseID);
                            comm.Parameters.AddWithValue("@3", number);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines if a user exists.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>True if a user with the university identifier exists, false otherwise.</returns>
        public bool DoesUserExist(string univID)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT UnivID FROM Users WHERE UnivID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Determines if a user exists.
        /// </summary>
        /// <param name="univID">The university identifier of the user.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <returns>True if the user exists, false otherwise.</returns>
        public bool DoesUserExist(string univID, string firstName, string lastName, string email)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                // Check the university identifier.
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT UnivID FROM Users WHERE UnivID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            if (comm.ExecuteScalar() != null)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                // Check the name and email.
                if (!result)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(ConnectionString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(
                                "SELECT UnivID FROM Users Where UPPER(FName) = UPPER(@0) and UPPER(LName) = UPPER(@1)", conn))
                            {
                                comm.Parameters.AddWithValue("@0", firstName);
                                comm.Parameters.AddWithValue("@1", lastName);
                                if (comm.ExecuteScalar() != null)
                                {
                                    result = true;
                                }
                            }
                            if (!result) {
                                using (SqlCommand comm = new SqlCommand(
                                    "SELECT UnivID FROM Users Where UPPER(Email) = UPPER(@0)", conn))
                                {
                                    comm.Parameters.AddWithValue("@0", email);
                                    if (comm.ExecuteScalar() != null)
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ReadWriteLock.ExitReadLock();
                    }
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database.");
            }
            return result;
        }

        /// <summary>
        /// Attempts to enroll a student in a section.
        /// </summary>
        /// <param name="univID">A student's university identifier.</param>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>True if the student was successfully enrolled in the section, false otherwise.</returns>
        public bool EnrollInSection(string univID, int sectionID)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO SectionStudents (SectionID, StudentID) VALUES (@0, @1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.Parameters.AddWithValue("@1", univID);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }
        
        /// <summary>
        /// Gets a student's academic record.
        /// </summary>
        /// <param name="univID">The student's unviersity identifier.</param>
        /// <returns>A list of tuples representing the student's record.</returns>
        public List<Tuple<int, string>> GetAcademicRecord(string univID)
        {
            List<Tuple<int, string>> record = new List<Tuple<int, string>>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT CourseID, Grade FROM Grades WHERE StudentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    record.Add(new Tuple<int, string>(reader.GetInt32(0), reader.GetString(1)));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return record;
        }
        
        /// <summary>
        /// Gets a building from the database using the specified building identifier.
        /// </summary>
        /// <param name="buildingID">The identifier belonging to the building to retrieve.</param>
        /// <returns>A building.</returns>
        /// <exception cref="Exception">If there is an issue retrieving the building.</exception>
        /// <exception cref="Exception">If there is an issue connecting to the database.</exception>
        /// <exception cref="Exception">If there was no building found with the given identifier.</exception>
        public Building GetBuilding(int buildingID)
        {
            Building building = null;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Buildings WHERE BuildingID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", buildingID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    building = new Building(buildingID, reader.GetString(1))
                                    {
                                        Abbreviation = reader.GetString(2)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ReadWriteLock.ExitReadLock();
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Cannot connect to the database!!");
            }
            if (building == null)
            {
                throw new Exception($"No building found with the specfied building identifier: {buildingID}!");
            }
            return building;
        }

        /// <summary>
        /// Gets a building identifier based on the buildings name.
        /// </summary>
        /// <param name="name">The name of the building.</param>
        /// <returns>A building identifier if the building exists, NOT_FOUND otherwise.</returns>
        public int GetBuildingID(string name)
        {
            int result = NOT_FOUND;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT BuildingID FROM Buildings WHERE UPPER(Name) = UPPER(@0)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            object obj = comm.ExecuteScalar();
                            if (obj != null)
                            {
                                result = Convert.ToInt32(obj);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a list of all buildings in the database.
        /// </summary>
        /// <returns></returns>
        public List<Building> GetBuildings()
        {
            List<Building> buildings = new List<Building>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld) {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Buildings", conn))
                        {
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!UniversityManager.InstanceOf().HasBuildingCached(reader.GetInt32(0))) {
                                        buildings.Add(new Building(reader.GetInt32(0), reader.GetString(1))
                                        {
                                            Abbreviation = reader.GetString(2)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to acquire connection to database!");
            }
            return buildings;
        }

        /// <summary>
        /// Gets a single course from the database using the CourseID of the course.
        /// </summary>
        /// <param name="courseID">The CourseID of the course.</param>
        /// <returns>The course if it is exists, null otherwise.</returns>
        public Course GetCourse(int courseID)
        {
            Course course = null;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Courses WHERE CourseID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    course = new Course(reader.GetInt32(0), reader.GetString(2))
                                    {
                                        DepartmentID = reader.GetInt32(1),
                                        Number = reader.GetString(3),
                                        Credits = reader.GetInt32(4),
                                        Archived = reader.GetBoolean(5),
                                        Description = reader.GetString(6)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            if (course == null)
            {
                throw new Exception($"No course exists with the given course identifier {courseID}!");
            }
            // Get the courses prerequisites
            foreach (var prereq in GetPrerequisites(courseID))
            {
                course.AddPrerequisite(prereq);
            }
            return course;
        }

        /// <summary>
        /// Gets a course identifier for a course using the department identifier and course number.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <param name="number">A course number.</param>
        /// <returns>A department identifier if it exists.</returns>
        public int GetCourseID(int departmentID, string number)
        {
            int courseID = -1;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT CourseID FROM Courses WHERE DepartmentID = @0 AND " +
                                "UPPER(Number) = Upper(@1)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            comm.Parameters.AddWithValue("@1", number);
                            object obj = comm.ExecuteScalar();
                            if (obj != null) {
                                courseID = Convert.ToInt32(obj);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            if (courseID == -1)
            {
                throw new Exception($"Error: No course found that belongs to department {departmentID} with number {number}!");
            }
            return courseID;
        }

        /// <summary>
        /// Gets a list of all non-cached courses from the database.
        /// </summary>
        /// <returns>A list of courses.</returns>
        public List<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand("SELECT * FROM Courses", conn))
                        {
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!CourseManager.InstanceOf().HasCourseCached(reader.GetInt32(0)))
                                    {
                                        courses.Add(new Course(reader.GetInt32(0), reader.GetString(2))
                                        {
                                            DepartmentID = reader.GetInt32(1),
                                            Number = reader.GetString(3),
                                            Credits = reader.GetInt32(4),
                                            Archived = reader.GetBoolean(5),
                                            Description = reader.GetString(6)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            // Add in the courses prerequisites and sections
            foreach (var course in courses)
            {
                // Add in the courses prerequisites
                foreach (var prereq in GetPrerequisites(course.CourseID))
                {
                    course.AddPrerequisite(prereq);
                }
            }
            return courses;
        }

        /// <summary>
        /// Gets the courses from the database that align with the given department ID.
        /// </summary>
        /// <param name="departmentID">The ID of the department the courses belong to.</param>
        /// <returns>A list containing courses that exist within the given department.</returns>
        public List<Course> GetCourses(int departmentID)
        {
            List<Course> courses = new List<Course>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                // Read in all of the courses
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Courses WHERE DepartmentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!CourseManager.InstanceOf().HasCourseCached(reader.GetInt32(0)))
                                    {
                                        courses.Add(new Course(reader.GetInt32(0), reader.GetString(2))
                                        {
                                            DepartmentID = reader.GetInt32(1),
                                            Number = reader.GetString(3),
                                            Credits = reader.GetInt32(4),
                                            Archived = reader.GetBoolean(5),
                                            Description = reader.GetString(6)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            // Attempt to read the prerequisites
            foreach (Course course in courses)
            {
                foreach (var prerequisite in GetPrerequisites(course.CourseID))
                {
                    course.AddPrerequisite(prerequisite);
                }
            }
            return courses;
        }

        /// <summary>
        /// Reads the prerequisites for a course.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <returns>A list of prerequisites. Will be empty if the course does not have any prerequisites.</returns>
        public List<Tuple<int, int>> GetPrerequisites(int courseID)
        {
            List<Tuple<int, int>> prerequisites = new List<Tuple<int, int>>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld) {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Prerequisites WHERE CourseID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            int prereq = 0, group = 0;
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    prereq = reader.GetInt32(1);
                                    group = reader.GetInt32(2);
                                    prerequisites.Add(new Tuple<int, int>(prereq, group));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return prerequisites;
        }

        /// <summary>
        /// Gets a department from the database.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <returns>The department if it exists.</returns>
        /// <exception cref="Exception">If the department does not exist or we were unable to connect to the database.</exception>
        public Department GetDepartment(int departmentID)
        {
            Department department = null;
            ReadWriteLock.EnterReadLock();
            // Attempt to get the specified department
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Departments WHERE DepartmentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    department = new Department(reader.GetInt32(0), reader.GetString(2))
                                    {
                                        SchoolID = reader.GetInt32(1),
                                        Abbreviation = reader.GetString(3)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            // If the department is null, then the department does not exist
            if (department == null) {
                throw new Exception($"No department found with the given department identifier: {departmentID}!");
            }
            // Get the departments courses
            foreach (int courseID in GetIntegerKeys(
                "SELECT CourseID FROM Courses WHERE DepartmentID = @0", departmentID))
            {
                department.AddCourse(courseID);
            }
            // Get the departments majors
            foreach (int majorID in GetIntegerKeys(
                "SELECT MajorID FROM Majors WHERE DepartmentID = @0", departmentID))
            {
                department.AddMajor(majorID);
            }
            return department;
        }

        /// <summary>
        /// Gets the advisees for an instructor.
        /// </summary>
        /// <param name="univID">A user's university identifier.</param>
        /// <returns>A list of strings representing the instrucor's advisees.</returns>
        public List<string> GetInstructorAdvisees(string univID)
        {
            List<string> advisees = new List<string>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT StudentID FROM Advisees WHERE AdvisorID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    advisees.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            return advisees;
        }

        /// <summary>
        /// Gets the departments for an instructor.
        /// </summary>
        /// <param name="univID">A university identifier.</param>
        /// <returns>A list of integers representing the instructors departments.</returns>
        public List<int> GetInstructorDepartments(string univID)
        {
            List<int> departments = new List<int>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld) {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT DepartmentID FROM DepartmentInstructors WHERE InstructorID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader()) {
                                while (reader.Read())
                                {
                                    departments.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            return departments;
        }

        /// <summary>
        /// Retrieves a list containing integer primary keys from a field in a table specified by the 
        /// select statement. Keys are filtered using the given foreign key.
        /// </summary>
        /// <param name="select">The select statement to use to retrieve the keys.</param>
        /// <param name="key">The foreign key for the table.</param>
        /// <returns>A list of integer primary surrogate keys from a table.</returns>
        /// <exception cref="Exception">If we are unable to acquire a connection to the database.</exception>
        private List<int> GetIntegerKeys(string select, object key)
        {
            List<int> keys = new List<int>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(select, conn))
                        {
                            comm.Parameters.AddWithValue("@0", key);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    keys.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to acquire connection to database!");
            }
            return keys;
        }

        /// <summary>
        /// Retrieves a list containing string primary keys from a field in a table specified by the 
        /// select statement. Keys are filtered using the given foreign key.
        /// </summary>
        /// <param name="select">The select statement to use to retrieve the keys.</param>
        /// <param name="key">The foreign key for the table.</param>
        /// <returns>A list of string primary surrogate keys from a table.</returns>
        /// <exception cref="Exception">If we are unable to acquire a connection to the database.</exception>
        private List<string> GetStringKeys(string select, object key)
        {
            List<string> keys = new List<string>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(select, conn))
                        {
                            comm.Parameters.AddWithValue("@0", key);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                keys.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return keys;
        }

        /// <summary>
        /// Gets the instructors for a department using the department's identifier.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <returns></returns>
        public List<User> GetDepartmentInstructors(int departmentID)
        {
            List<User> instructors = new List<User>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT Users.UnivID, Users.FName, Users.LName, Users.Email, Users.Role " +
                                "FROM Users INNER JOIN DepartmentInstructors ON " +
                                "Users.UnivID = DepartmentInstructors.InstructorID WHERE " +
                                "DepartmentInstructors.DepartmentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!UserManager.InstanceOf().HasUserCached(reader.GetString(0))) {
                                        instructors.Add(new User(reader.GetString(0), reader.GetString(1),
                                            reader.GetString(2), reader.GetString(3), reader.GetString(4)));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            foreach (var instructor in instructors)
            {
                SetupUser(instructor);
            }
            return instructors;
        }

        /// <summary>
        /// Gets departments from the database that are listed with the given school.
        /// </summary>
        /// <param name="schoolID">The ID of the school the departments belong to.</param>
        /// <returns>A list containing departments that exist within the given school.</returns>
        public List<Department> GetDepartments(int schoolID)
        {
            List<Department> departments = new List<Department>();
            // Try to acquire the read-lock
            ReadWriteLock.EnterReadLock();
            // Only attempt to read from the database if we hold the read lock
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * from Departments WHERE SchoolID = @0", conn))
                        {
                            // Add the schoolID as a parameter to the SQL command
                            comm.Parameters.AddWithValue("@0", schoolID);

                            // Execute the SQL command
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!UniversityManager.InstanceOf().HasDepartmentCached(reader.GetInt32(0)))
                                    {
                                        departments.Add(new Department(reader.GetInt32(0), reader.GetString(2))
                                        {
                                            SchoolID = reader.GetInt32(1),
                                            Abbreviation = reader.GetString(3)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to database!");
            }
            return departments;
        }

        /// <summary>
        /// Gets the department identifier for a given department using it's abbreviation.
        /// </summary>
        /// <param name="abbreviation">A department's name abbreviation.</param>
        /// <returns>A department identifier if it exists, NOT_FOUND otherwise.</returns>
        public int GetDepartmentID(string abbreviation)
        {
            int departmentID = NOT_FOUND;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT DepartmentID FROM Departments WHERE UPPER(abbreviation) = UPPER(@0)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", abbreviation);
                            object obj = comm.ExecuteScalar();
                            if (obj != null) {
                                departmentID = Convert.ToInt32(obj);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            if (departmentID == -1)
            {
                throw new Exception($"Error: No department found for the abbreviation: {abbreviation}!");
            }
            return departmentID;
        }

        /// <summary>
        /// Gets a major from the database with the given major identifier.
        /// </summary>
        /// <param name="majorID">The major's identifier.</param>
        /// <returns>The major if it exists.</returns>
        /// <exception cref="Exception">If the major does not exist, or we were not able to connect to the database.</exception>
        public Major GetMajor(int majorID)
        {
            Major major = null;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Majors WHERE MajorID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", majorID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    major = new Major(reader.GetInt32(0), reader.GetString(2))
                                    {
                                        DepartmentID = reader.GetInt32(1)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception($"Unable to connect to the database!");
            }
            if (major == null)
            {
                throw new Exception($"No major found with the given major identifier {majorID}!");
            }
            return major;
        }

        /// <summary>
        /// Gets a major identifier from the database using the specified major name.
        /// </summary>
        /// <param name="name">A major name.</param>
        /// <returns>A major identifier if the major exists, NOT_FOUND otherwise.</returns>
        public int GetMajorID(string name)
        {
            int result = NOT_FOUND;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT MajorID FROM Majors WHERE UPPER(Name) = UPPER(@0)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            object obj = comm.ExecuteScalar();
                            if (obj != null)
                            {
                                result = (int)obj;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            return result;
        }
        
        /// <summary>
        /// Gets all of the majors from the database for a given department.
        /// </summary>
        /// <param name="departmentID">The department to retrieve majors from</param>
        /// <returns>A list of majors corresponding to the given department.</returns>
        /// <exception cref="Exception">If we were unable to connect to the database.</exception>
        public List<Major> GetMajors(int departmentID)
        {
            List<Major> majors = new List<Major>();
            // Try to acquire the read-lock
            ReadWriteLock.EnterReadLock();
            // Only attempt to read from the database if we hold the read lock
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * from Majors WHERE DepartmentID = @0", conn))
                        {
                            // Add the schoolID as a parameter to the SQL command
                            comm.Parameters.AddWithValue("@0", departmentID);

                            // Execute the SQL command
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!UniversityManager.InstanceOf().HasMajorCached(reader.GetInt32(0)))
                                    {
                                        majors.Add(new Major(reader.GetInt32(0), reader.GetString(2))
                                        {
                                            DepartmentID = reader.GetInt32(1)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return majors;
        }

        /// <summary>
        /// Gets the user's password from the database.
        /// </summary>
        /// <param name="univID">The user's university ID.</param>
        /// <returns>The user's password.</returns>
        public Tuple<String, String> GetPassword(string univID)
        {
            Tuple<string, string> stored = null;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT Password, Salt FROM Users WHERE UnivID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    stored = new Tuple<string, string>(reader.GetString(0), reader.GetString(1));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            if (stored == null)
            {
                throw new Exception($"No user exists with the given university identifier {univID}!");
            }
            return stored;
        }

        /// <summary>
        /// Gets the role of a user using the UniversityID from the database.
        /// </summary>
        /// <param name="univID">The user's university ID.</param>
        /// <returns>The role of the user if the user exists, null otherwise.</returns>
        public string GetRole(string univID)
        {
            string role = "";
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT Role FROM Users WHERE UnivID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    role = reader.GetString(0);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            if (role.Length == 0)
            {
                throw new Exception($"No user exists with given university identifier {univID}!");
            }
            return role;
        }

        public School GetSchool(int schoolID)
        {
            School school = null;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Schools WHERE SchoolID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", schoolID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    school = new School(reader.GetInt32(0), reader.GetString(1))
                                    {
                                        Abbreviation = reader.GetString(2)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception($"Unable to connect to the database!");
            }
            if (school == null)
            {
                throw new Exception($"No school exists with the given school identifier {schoolID}!");
            }
            return school;
        }

        /// <summary>
        /// Gets a school identifier from the database using the school name.
        /// </summary>
        /// <param name="name">A school name.</param>
        /// <returns>A school identifier if the school exists, NOT_FOUND otherwise.</returns>
        public int GetSchoolID(string name)
        {
            int result = NOT_FOUND;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand("" +
                            "SELECT SchoolID FROM Schools WHERE UPPER(Name) = UPPER(@0)", conn))
                        {
                            comm.Parameters.AddWithValue("@0", name);
                            object obj = comm.ExecuteScalar();
                            if (obj != null)
                            {
                                result = (int)obj;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the user's schedule from the database.
        /// </summary>
        /// <param name="term">The term to retrieve the schedule for.</param>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="student">Whether the user is a student or instructor. True if the 
        /// user is a student, false if they are an instructor.</param>
        /// <returns></returns>
        public List<int> GetSchedule(Tuple<string, int> term, string univID, bool student)
        {
            List<int> schedule = new List<int>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        // Get the users sections alltogether
                        string joinTable = student ? "SectionStudents" : "SectionInstructors";
                        string joinRow = student ? "StudentID" : "InstructorID";
                        using (SqlCommand comm = new SqlCommand(
                            $"SELECT Sections.SectionID FROM Sections " +
                                $"INNER JOIN {joinTable} ON Sections.SectionID = {joinTable}.SectionID WHERE " +
                                $"{joinTable}.{joinRow} = @0 AND Sections.Semester = @1 AND " +
                                $"Sections.Year = @2", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            comm.Parameters.AddWithValue("@1", term.Item1);
                            comm.Parameters.AddWithValue("@2", term.Item2);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    schedule.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return schedule;
        }

        /// <summary>
        /// Reads in schools from the database.
        /// </summary>
        /// <returns>A list of schools if we are able to successfully connect to the database.
        /// Otherwise will return null.</returns>
        public List<School> GetSchools()
        {
            List<School> schools = new List<School>();
            // Try to acquire the read-lock
            ReadWriteLock.EnterReadLock();
            // Only attempt to read from the database if we hold the read lock
            if (ReadWriteLock.IsReadLockHeld) {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Schools", conn))
                        {
                            // Read in the schools
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!UniversityManager.InstanceOf().HasSchoolCached(reader.GetInt32(0)))
                                    {
                                        schools.Add(new School(reader.GetInt32(0), reader.GetString(1))
                                        {
                                            Abbreviation = reader.GetString(2)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database.");
            }
            return schools;
        }

        /// <summary>
        /// Gets a section using the section's identifier.
        /// </summary>
        /// <param name="sectionID">The section's identifier.</param>
        /// <returns>A section if it exists.</returns>
        public Section GetSection(int sectionID)
        {
            Section section = null;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * FROM Sections WHERE SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    section = new Section(sectionID, reader.GetInt32(1), reader.GetString(4),
                                        reader.GetInt32(5))
                                    {
                                        BuildingID = reader.GetInt32(2),
                                        Number = reader.GetString(3),
                                        Room = reader.GetInt32(6),
                                        StartDateAndTime = reader.GetDateTime(7),
                                        EndDateAndTime = reader.GetDateTime(8),
                                        MaxEnrollment = reader.GetInt32(9)
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            if (section == null)
            {
                throw new Exception("Unable to connect to the database!");
            }
            try
            {
                foreach (var student in GetSectionStudents(section.SectionID))
                {
                    section.AddStudent(student);
                }
                foreach (var instructor in GetSectionInstructors(section.SectionID))
                {
                    section.AddInstructor(instructor);
                }
                foreach (var day in GetSectionDays(section.SectionID))
                {
                    section.AddDay(day);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return section;
        }

        /// <summary>
        /// Gets the sections for a given course and term.
        /// </summary>
        /// <param name="courseID">A course identifier.</param>
        /// <param name="term">A term.</param>
        /// <returns>A list of section for the given course and term.</returns>
        public List<Section> GetSections(int courseID, Tuple<string, int> term)
        {
            // Return object
            List<Section> sections = new List<Section>();
            // Try to acquire the read-lock
            ReadWriteLock.EnterReadLock();
            // Only attempt to read from the database if we hold the read lock
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT * from Sections WHERE CourseID = @0 AND Semester = @1 AND Year = @2", conn))
                        {
                            // Add the schoolID as a parameter to the SQL command
                            comm.Parameters.AddWithValue("@0", courseID);
                            comm.Parameters.AddWithValue("@1", term.Item1);
                            comm.Parameters.AddWithValue("@2", term.Item2);

                            // Execute the SQL command
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!CourseManager.InstanceOf().HasSectionCached(reader.GetInt32(0)))
                                    {
                                        sections.Add(new Section(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(4), 
                                            reader.GetInt32(5))
                                        {
                                            BuildingID = reader.GetInt32(2),
                                            Number = reader.GetString(3),
                                            Room = reader.GetInt32(6),
                                            StartDateAndTime = reader.GetDateTime(7),
                                            EndDateAndTime = reader.GetDateTime(8),
                                            MaxEnrollment = reader.GetInt32(9)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            try
            {
                foreach (var section in sections)
                {
                    foreach (var student in GetSectionStudents(section.SectionID))
                    {
                        section.AddStudent(student);
                    }
                    foreach (var instructor in GetSectionInstructors(section.SectionID)) {
                        section.AddInstructor(instructor);
                    }
                    foreach (var day in GetSectionDays(section.SectionID))
                    {
                        section.AddDay(day);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sections;
        }

        /// <summary>
        /// Gets the days associated with a section.
        /// </summary>
        /// <param name="sectionID">The section identifier.</param>
        /// <returns>A list of strings representing the days the section is held.</returns>
        public List<string> GetSectionDays(int sectionID)
        {
            List<string> days = new List<string>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT Day FROM SectionDays WHERE SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    days.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return days;
        }

        /// <summary>
        /// Gets the instructors for a section.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>A list of university identifiers.</returns>
        public List<string> GetSectionInstructors(int sectionID)
        {
            List<string> instructors = new List<string>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT Users.UnivID FROM Users " +
                                "INNER JOIN SectionInstructors ON Users.UnivID = SectionInstructors.InstructorID " +
                                "WHERE SectionInstructors.SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    instructors.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return instructors;
        }

        /// <summary>
        /// Gets the students for a section.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>A list of university identifiers.</returns>
        public List<string> GetSectionStudents(int sectionID)
        {
            List<string> students = new List<string>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT Users.UnivID FROM Users " +
                                "INNER JOIN SectionStudents ON Users.UnivID = SectionStudents.StudentID " +
                                "WHERE SectionStudents.SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    students.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return students;
        }

        /// <summary>
        /// Gets a student's advisors.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <returns>A list of integers representing the student's advisors.</returns>
        public List<string> GetStudentAdvisors(string univID)
        {
            List<string> advisors = new List<string>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT AdvisorID FROM Advisees WHERE StudentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    advisors.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return advisors;
        }

        /// <summary>
        /// Gets the majors (as strings) a student is assigned to.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <returns>A list of majors (as strings). List will be empty if the student has not declared a major 
        /// or the student does not exist (which should never happen).</returns>
        public List<int> GetStudentMajors(string univID)
        {
            List<int> majors = new List<int>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT MajorID FROM StudentMajors WHERE StudentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    majors.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return majors;
        }

        /// <summary>
        /// Gets a user from the database using their university identifier.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>A user if one exists with the given identifier, otherwise null.</returns>
        public User GetUser(string univID)
        {
            User user = null;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "SELECT UnivID, FName, LName, Email, Role FROM Users WHERE UnivID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", univID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    user = new User(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                                        reader.GetString(3), reader.GetString(4));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            if (user == null)
            {
                throw new Exception($"No user found with the given university identifier: {univID}!");
            }
            SetupUser(user);
            return user;
        }

        /// <summary>
        /// Modifies a building with the specified parameters.
        /// </summary>
        /// <param name="buildingID">The building's identifier.</param>
        /// <param name="abbreviation">The new abbreviation.</param>
        /// <returns>True if the building was successfully modified, false otherwise.</returns>
        public bool ModifyBuilding(int buildingID, string name, string abbreviation)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Buildings SET Name = @1, Abbreviation = @2 WHERE BuildingID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", buildingID);
                            comm.Parameters.AddWithValue("@1", name);
                            comm.Parameters.AddWithValue("@2", abbreviation);
                            comm.ExecuteNonQuery();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Modifies a course with the specified parameters.
        /// </summary>
        /// <param name="courseID">The course's identifier.</param>
        /// <param name="number">The number for the course.</param>
        /// <param name="credits">The credit hours for the course.</param>
        /// <param name="archived">Whether the course is archived or unarchived.</param>
        /// <param name="description">The description of the course.</param>
        /// <returns>True if the course was successfully modified, false otherwise.</returns>
        public bool ModifyCourse(int courseID, string number, int credits, bool archived, string description)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Courses SET Number = @1, Credits = @2, Archived = @3, Description = @4 " +
                                "WHERE CourseID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            comm.Parameters.AddWithValue("@1", number);
                            comm.Parameters.AddWithValue("@2", credits);
                            comm.Parameters.AddWithValue("@3", archived == false ? 0 : 1);
                            comm.Parameters.AddWithValue("@4", description);
                            comm.ExecuteNonQuery();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Modifies a department with the specified parameters.
        /// </summary>
        /// <param name="departmentID">The department's identifier.</param>
        /// <param name="schoolID">The identifier for the school the department belongs to.</param>
        /// <param name="abbreviation">The department's abbreviation.</param>
        /// <returns>True if the department was successfully modified, false otherwise.</returns>
        public bool ModifyDepartment(int departmentID, int schoolID, string abbreviation)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Departments SET SchoolID = @1, abbreviation = @2 WHERE DepartmentID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", departmentID);
                            comm.Parameters.AddWithValue("@1", schoolID);
                            comm.Parameters.AddWithValue("@2", abbreviation);
                            comm.ExecuteNonQuery();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Modifies a major with the specified parameters.
        /// </summary>
        /// <param name="majorID">The major's identifier.</param>
        /// <param name="departmentID">The identifier of the department the major belongs to.</param>
        /// <returns>True if the major was successfully modified, false otherwise.</returns>
        public bool ModifyMajor(int majorID, int departmentID)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Majors SET DepartmentID = @1 WHERE MajorID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", majorID);
                            comm.Parameters.AddWithValue("@1", departmentID);
                            comm.ExecuteNonQuery();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="prerequisites"></param>
        /// <returns></returns>
        public bool ModifyPrerequisites(int courseID, List<Tuple<int, int>> prerequisites)
        {
            // TODO: Improve this algorithm so it is more selectively updates prerequisites
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        // Step 1: Delete the current prerequisites
                        using (SqlCommand comm = new SqlCommand(
                            "DELETE FROM Prerequisites WHERE CourseID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", courseID);
                            comm.ExecuteNonQuery();
                        }
                        // Step 2: Update to the new prerequisites
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO Prerequisites VALUES (@0, @1, @2)", conn))
                        {
                            foreach (var prereq in prerequisites)
                            {
                                comm.Parameters.AddWithValue("@0", courseID);
                                comm.Parameters.AddWithValue("@1", prereq.Item1);
                                comm.Parameters.AddWithValue("@2", prereq.Item2);
                                comm.ExecuteNonQuery();
                            }
                            comm.Parameters.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return true;
        }

        /// <summary>
        /// Modifies a school with the specified parameters.
        /// </summary>
        /// <param name="schoolID">The school's identifier.</param>
        /// <param name="abbreviation">The abbreviation of the school's name.</param>
        /// <returns>True if the school was successfully modified, false otherwise.</returns>
        public bool ModifySchool(int schoolID, string abbreviation)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Schools SET Abbreviation = @1 WHERE SchoolID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", schoolID);
                            comm.Parameters.AddWithValue("@1", abbreviation);
                            comm.ExecuteNonQuery();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Modifies a section with the specified parameters.
        /// </summary>
        /// <param name="sectionID">The section's identifier.</param>
        /// <param name="buildingID">The identifier of the building the section is assigned to.</param>
        /// <param name="room">The room the section is assigned to.</param>
        /// <param name="startDateAndTime">The start date and time of the section.</param>
        /// <param name="endDateAndTime">The end date and time of the section.</param>
        /// <param name="maxEnrollment">The maximum enrollment for the section.</param>
        /// <returns>True if the section was successfully modified, false otherwise.</returns>
        public bool ModifySection(int sectionID, int buildingID, int room, 
            DateTime startDateAndTime, DateTime endDateAndTime, int maxEnrollment)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "UPDATE Sections SET BuildingID = @1, Room = @2, StartDateAndTime = @3, " +
                                "EndDateAndTime = @4, MaxEnrollment = @5 WHERE SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.Parameters.AddWithValue("@1", buildingID);
                            comm.Parameters.AddWithValue("@2", room);
                            comm.Parameters.AddWithValue("@3", startDateAndTime);
                            comm.Parameters.AddWithValue("@4", endDateAndTime);
                            comm.Parameters.AddWithValue("@5", maxEnrollment);
                            comm.ExecuteNonQuery();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Updates the days in a section to those spcified.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <param name="days">The days to update the section to.</param>
        /// <returns>True if the days in the section were modified successfully, false otherwise.</returns>
        public bool ModifySectionDays(int sectionID, List<string> days)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand comm = new SqlCommand(
                            "DELETE FROM SectionDays WHERE SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.ExecuteNonQuery();
                        }
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO SectionDays VALUES (@0, @1)", conn))
                        {
                            foreach (var day in days)
                            {
                                comm.Parameters.AddWithValue("@0", sectionID);
                                comm.Parameters.AddWithValue("@1", day);
                                comm.ExecuteNonQuery();
                            }
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Updates the instructors in a section to those specified.
        /// </summary>
        /// <param name="sectionID">A section identifier.</param>
        /// <param name="instructors">The instructors to update the section to.</param>
        /// <returns>True if the instructors in the section were modified successfully, false otherwise.</returns>
        public bool ModifySectionInstructors(int sectionID, List<string> instructors)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld) {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand comm = new SqlCommand(
                            "DELETE FROM SectionInstructors WHERE SectionID = @0", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.ExecuteNonQuery();
                        }
                        using (SqlCommand comm = new SqlCommand(
                            "INSERT INTO SectionInstructors VALUES (@0, @1)", conn))
                        {
                            foreach (var instructor in instructors)
                            {
                                comm.Parameters.AddWithValue("@0", sectionID);
                                comm.Parameters.AddWithValue("@1", instructor);
                                comm.ExecuteNonQuery();
                            }
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }

        /// <summary>
        /// Sets up a user's unique collections determined by their role.
        /// </summary>
        /// <param name="user">A user.</param>
        private void SetupUser(User user)
        {
            if (user.Role.Equals(User.INSTRUCTOR))
            {
                try
                {
                    foreach (var department in GetInstructorDepartments(user.UnivID))
                    {
                        user.AddDepartment(department);
                    }
                    foreach (var advisee in GetInstructorAdvisees(user.UnivID))
                    {
                        user.AddRelationship(advisee);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (user.Role.Equals(User.STUDENT))
            {
                try
                {
                    foreach (var major in GetStudentMajors(user.UnivID))
                    {
                        user.AddMajor(major);
                    }
                    foreach (var advisor in GetStudentAdvisors(user.UnivID))
                    {
                        user.AddRelationship(advisor);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Attempts to unenroll a student from a section.
        /// </summary>
        /// <param name="univID">A student's university identifier.</param>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>True if the student was unenrolled from the section, false otherwise.</returns>
        public bool UnenrollFromSection(string univID, int sectionID)
        {
            bool result = false;
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(
                            "DELETE FROM SectionStudents WHERE SectionID = @0 AND StudentID = @1", conn))
                        {
                            comm.Parameters.AddWithValue("@0", sectionID);
                            comm.Parameters.AddWithValue("@1", univID);
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ReadWriteLock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Error: Unable to connect to the database!");
            }
            return result;
        }
    }
}