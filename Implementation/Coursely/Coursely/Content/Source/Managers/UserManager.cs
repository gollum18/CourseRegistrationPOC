using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

using Coursely.Content.Cache;
using Coursely.Content.Classes;
using Coursely.Content.Web;

namespace Coursely.Content.Managers
{
    /// <summary>
    /// Manages instances of the User class. Alos handles authentication and session management for users.
    /// </summary>
    public class UserManager
    {
        //
        // CONSTANTS
        //
        public static readonly int SaltLength = 32;
        public static readonly string SeedFile = "~/Seed.txt";
        public static readonly string RandomChars = "aA1bB2cC3dD4eE5fF6gG7hH8iI9jJ0kKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ";

        //
        // ATTRIBUTES
        //
        private MemoryCache<string, User> UserCache;

        /// <summary>
        /// Stores authenticated users, only authenticated users may view pages that require login.
        /// </summary>
        private HashSet<string> AuthenticatedUsers = new HashSet<string>(); 

        /// <summary>
        /// Private static instance only accessbiel through InstanceOf. Enforces Singleton design pattern.
        /// </summary>
        private static UserManager Instance { get; set; }

        /// <summary>
        /// Used to generate hashes.
        /// </summary>
        /// <remarks>Not secure, change to RNGCryptoServiceProvider in the future.</remarks>
        private Random HashGenerator { get; } = new Random();

        /// <summary>
        /// Private constructor to enforce the Singleton design pattern.
        /// </summary>
        private UserManager()
        {
            UserCache = new MemoryCache<string, User>();
        }

        /// <summary>
        /// Returns an instance of a UserManager.
        /// </summary>
        /// <returns>UserManager</returns>
        public static UserManager InstanceOf()
        {
            if (Instance == null)
            {
                Instance = new UserManager();
            }
            return Instance;
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>True if the user authenticates correctly, false otherwise.</returns>
        public bool Authenticate(string univID, string password, bool changePassword)
        {
            if (!DatabaseManager.InstanceOf().DoesUserExist(univID))
            {
                return false;
            }
            bool result = false;
            Tuple<string, string> stored = null;
            try
            {
                stored = DatabaseManager.InstanceOf().GetPassword(univID);
                if (stored.Item1.Equals(HashAndSalt(password, stored.Item2)))
                {
                    result = true;
                    if (!changePassword) {
                        AuthenticatedUsers.Add(univID);
                        if (!UserCache.Contains(univID))
                        {
                            UserCache.Add(univID, DatabaseManager.InstanceOf().GetUser(univID));
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
                stored = null;
            }
            return result;
        }

        /// <summary>
        /// Changes the user's email to the specified email.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="email">The user's new email.</param>
        /// <returns>True if the email was changed successfully, false otherwise.</returns>
        /// <exception cref="Exception">If there was an issue changing the user's email.</exception>
        public bool ChangeEmail(string univID, string email)
        {
            // Check that the user exists
            if (!UserCache.Contains(univID) && !DatabaseManager.InstanceOf().DoesUserExist(univID))
            {
                return false;
            }
            // Change the email
            bool result = false;
            try
            {
                DatabaseManager.InstanceOf().ChangeEmail(univID, email);
                if (!UserCache.Contains(univID))
                {
                    UserCache.Add(univID, DatabaseManager.InstanceOf().GetUser(univID));
                }
                User user = UserCache.Get(univID);
                if (!user.Email.Equals(email)) {
                    user.Email = email;
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="password">The user's new password.</param>
        /// <returns></returns>
        /// <exception cref="Exception">If there was an issue changing the user's password.</exception>
        public bool ChangePassword(string univID, string oldPassword, string newPassword)
        {
            bool result = false;
            try {
                if (Authenticate(univID, oldPassword, true)) {
                    string salt = GenerateSaltString(32);
                    string saltedPassword = HashAndSalt(newPassword, salt);
                    if (DatabaseManager.InstanceOf().ChangePassword(univID, saltedPassword, salt))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Creates a user with the given specifications.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="role">The user's role.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>True if the user was successfully created, false otherwise.</returns>
        public bool CreateUser(string univID, string firstName, string lastName, string email,
            string role, string password)
        {
            try
            {
                if (DatabaseManager.InstanceOf().DoesUserExist(univID))
                {
                    throw new Exception($"Cannot create user, university identifer {univID} already exists!");
                }
                string salt = GenerateSaltString(32);
                string saltedPassword = HashAndSalt(password, salt);
                DatabaseManager.InstanceOf().CreateUser(univID, firstName, lastName, email, role, saltedPassword, salt);
                saltedPassword = "";
                if (!UserCache.Contains(univID)) {
                    UserCache.Add(univID, new User(univID, firstName, lastName, email, role));
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates an instructor with the given parameters.
        /// </summary>
        /// <param name="univID">The instructor's university identifier.</param>
        /// <param name="firstName">The instructor's first name.</param>
        /// <param name="lastName">The instructor's last name.</param>
        /// <param name="email">The instructor's email.</param>
        /// <param name="role">The instructor's role.</param>
        /// <param name="password">The instructor's password.</param>
        /// <param name="departments">The departments the instructor belongs to.</param>
        /// <returns>True if the instructor was created properly.</returns>
        public bool CreateInstructor(string univID, string firstName, string lastName, string email,
            string role, string password, List<int> departments)
        {
            try
            {
                if (!CreateUser(univID, firstName, lastName, email, role, password))
                {
                    return false;
                }
                if (!DatabaseManager.InstanceOf().AssignDepartmentsToInstructor(univID, departments))
                {
                    return false;
                }
                if (HasUserCached(univID))
                {
                    User user = UserCache.Get(univID);
                    if (user.Role.Equals(User.INSTRUCTOR)) {
                        foreach (var department in departments)
                        {
                            user.AddDepartment(department);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Creates a student with the given parameters.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <param name="firstName">The student's first name.</param>
        /// <param name="lastName">The student's last name.</param>
        /// <param name="email">The student's email.</param>
        /// <param name="role">The student's role.</param>
        /// <param name="password">The student's password.</param>
        /// <param name="advisors">The student's advisors.</param>
        /// <param name="majors">The student's majors.</param>
        /// <returns>True if the student was successfully created, false otherwise.</returns>
        public bool CreateStudent(string univID, string firstName, string lastName, string email,
            string role, string password, List<string> advisors, List<int> majors)
        {
            try
            {
                if (!CreateUser(univID, firstName, lastName, email, role, password))
                {
                    return false;
                }
                if (!DatabaseManager.InstanceOf().AssignAdvisorsToStudent(univID, advisors))
                {
                    return false;
                }
                if (!DatabaseManager.InstanceOf().AssignMajorsToStudent(univID, majors))
                {
                    return false;
                }
                if (HasUserCached(univID))
                {
                    User user = UserCache.Get(univID);
                    if (user.Role.Equals(User.STUDENT)) {
                        foreach (var advisor in advisors)
                        {
                            user.AddRelationship(advisor);
                        }
                        foreach (var major in majors)
                        {
                            user.AddMajor(major);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Determines whether a user exists.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>True if the user exists, false otherwise.</returns>
        /// <exception cref="Exception">If there is an issue with the database.</exception>
        public bool DoesUserExist(string univID)
        {
            if (HasUserCached(univID))
            {
                return true;
            }
            try
            {
                if (DatabaseManager.InstanceOf().DoesUserExist(univID))
                {
                    User user = DatabaseManager.InstanceOf().GetUser(univID);
                    UserCache.Add(univID, user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Attempts to enroll a student in a section.
        /// </summary>
        /// <param name="univID">A student's university identifier.</param>
        /// <param name="sectionID">A section identifier.</param>
        /// <returns>True if the student successfully enrolled in the section, false otherwise.</returns>
        public bool EnrollInSection(string univID, int sectionID, Tuple<string, int> term, bool advisorOverride)
        {
            try
            {
                // Check that the user exists
                if (!DoesUserExist(univID))
                {
                    throw new Exception($"Error: User {univID} does not exist!");
                }
                // Check thet the user is a student
                if (!GetRole(univID).Equals(User.STUDENT))
                {
                    throw new Exception($"Error: User {univID} is not a student!");
                }
                // Check that the section exists
                if (!CourseManager.InstanceOf().DoesSectionExist(sectionID))
                {
                    throw new Exception("Error: Cannot enroll student in section, section does not exist!");
                }
                // Get the section itself as well as the students schedule
                Section section = CourseManager.InstanceOf().GetSection(sectionID);
                if (section.IsSectionFull())
                {
                    throw new Exception("Error: Cannot enroll student in section, section is full!");
                }
                if (!advisorOverride)
                {
                    List<Section> schedule = GetSchedule(term, univID, true);
                    // Check if the student hasn't already taken the course
                    if (DatabaseManager.InstanceOf().GetAcademicRecord(univID).Find(
                        r => r.Item1.Equals(section.CourseID)) != null)
                    {
                        throw new Exception("Error: Cannot enroll student in section, student has already taken the " +
                            "course!");
                    }
                    // Check if the student isn't already enrolled in the course
                    if (schedule.Find(s => s.CourseID.Equals(section.CourseID)) != null)
                    {
                        throw new Exception("Error: Cannot enroll student in section, student is already enrolled in the " +
                            "course!");
                    }
                    // Determine the student's total credits for this term
                    int credits = 0;
                    foreach (var s in schedule)
                    {
                        credits += CourseManager.InstanceOf().GetCourse(s.CourseID).Credits;
                    }
                    // Make sure that adding this course won't put them above 18 credits
                    if (CourseManager.InstanceOf().GetCourse(section.CourseID).Credits + credits > 18)
                    {
                        throw new Exception("Error: Cannot enroll in section, enrolling would place you " +
                            "above the credit limit!");
                    }
                    // Verify the student meets the prerequisites for the course.
                    if (!VerifyPrerequisites(univID, section.CourseID))
                    {
                        throw new Exception($"Error: User {univID} does not meet the prerequisites to enroll in the course!");
                    }
                }
                // Attempt to enroll the student in the section
                if (DatabaseManager.InstanceOf().EnrollInSection(univID, sectionID))
                {
                    if (CourseManager.InstanceOf().HasSectionCached(sectionID))
                    {
                        CourseManager.InstanceOf().GetSection(sectionID).AddStudent(univID);
                    }
                    return true;
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
        }

        /// <summary>
        /// Gets a student's academic record.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <returns>A list of grades (as strings).</returns>
        public List<string> GetAcademicRecord(string univID)
        {
            try
            {
                if (!DoesUserExist(univID))
                {
                    throw new Exception($"Error: No student exists with the identifier {univID}!");
                }
                List<string> grades = new List<string>();
                Course course = null;
                foreach (var record in DatabaseManager.InstanceOf().GetAcademicRecord(univID))
                {
                    course = CourseManager.InstanceOf().GetCourse(record.Item1);
                    grades.Add($"{UniversityManager.InstanceOf().GetDepartment(course.DepartmentID).Abbreviation}" +
                        $"{course.Number} - {course.Name}: {record.Item2}");
                }
                return grades;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all of the instructors for a department.
        /// </summary>
        /// <param name="departmentID">The department's identifier.</param>
        /// <returns>A list of instructors who belong to the department.</returns>
        public List<User> GetDepartmentInstructors(int departmentID)
        {
            try
            {
                List<User> instructors = DatabaseManager.InstanceOf().GetDepartmentInstructors(departmentID);
                foreach (var user in UserCache.GetValues())
                {
                    if (user.Role.Equals(User.INSTRUCTOR) && user.HasDepartment(departmentID))
                    {
                        instructors.Add(user);
                    }
                }
                foreach (var instructor in instructors)
                {
                    if (!HasUserCached(instructor.UnivID))
                    {
                        UserCache.Add(instructor.UnivID, instructor);
                    }
                }
                instructors.Sort();
                return instructors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a user's email from the database.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>The user's email.</returns>
        public string GetEmail(string univID)
        {
            if (UserCache.Contains(univID))
            {
                return UserCache.Get(univID).Email;
            }
            try
            {
                User user = DatabaseManager.InstanceOf().GetUser(univID);
                UserCache.Add(univID, user);
                return user.Email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets an advisor's advisees.
        /// </summary>
        /// <param name="instructorID"> </param>
        /// <returns></returns>
        public List<User> GetInstructorAdvisees(string instructorID)
        {
            if (!DoesUserExist(instructorID))
            {
                throw new Exception("Unable to retrieve advisees for instructor, no instructor found with " +
                    $"university identifier: {instructorID}!");
            }
            List<User> advisees = new List<User>();
            try
            {
                foreach (var univID in DatabaseManager.InstanceOf().GetInstructorAdvisees(instructorID))
                {
                    if (HasUserCached(univID))
                    {
                        advisees.Add(UserCache.Get(univID));
                    }
                    else
                    {
                        try
                        {
                            User user = DatabaseManager.InstanceOf().GetUser(univID);
                            UserCache.Add(univID, user);
                            advisees.Add(user);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return advisees;
        }


        /// <summary>
        /// Gets a user's password from the database.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>A tuple containing the user's password and salt.</returns>
        private string GetPassword(string univID)
        {
            try
            {
                return DatabaseManager.InstanceOf().GetPassword(univID).Item1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user's role.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>The user's role. One of Administrator, Instructor, or Student.</returns>
        public string GetRole(string univID)
        {
            try
            {
                if (UserCache.Contains(univID))
                {
                    return UserCache.Get(univID).Role;
                }
                else
                {
                    User user = DatabaseManager.InstanceOf().GetUser(univID);
                    UserCache.Add(univID, user);
                    return user.Role;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user's schedule.
        /// </summary>
        /// <param name="term">The term to retrieve the schedule for.</param>
        /// <param name="univID">The user's university identifier.</param>
        /// <param name="student">Whether the user is a student or instructor. True if the 
        /// user is a student, false if they are an instructor.</param>
        /// <returns>A list of sections reprenting the student schedule.</returns>
        public List<Section> GetSchedule(Tuple<string, int> term, string univID, bool student)
        {
            if (!DoesUserExist(univID))
            {
                throw new Exception($"Unable to retrieve schedule, no user with the specified university " +
                    $"identifier: {univID} exists!");
            }
            List<Section> schedule = new List<Section>();
            try
            {
                foreach (var sectionID in DatabaseManager.InstanceOf().GetSchedule(term, univID, student))
                {
                    schedule.Add(CourseManager.InstanceOf().GetSection(sectionID));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return schedule;
        }

        /// <summary>
        /// Gets an advisee's advisors.
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public List<User> GetStudentAdvisors(string studentID)
        {
            if (!DoesUserExist(studentID))
            {
                throw new Exception("Unable to retrieve advisors for student, no student found with " +
                    $"university identifier: {studentID}!");
            }
            List<User> advisors = new List<User>();
            try
            {
                foreach (var univID in DatabaseManager.InstanceOf().GetStudentAdvisors(studentID))
                {
                    if (HasUserCached(univID))
                    {
                        advisors.Add(UserCache.Get(univID));
                    }
                    else
                    {
                        try
                        {
                            User user = DatabaseManager.InstanceOf().GetUser(univID);
                            UserCache.Add(univID, user);
                            advisors.Add(user);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return advisors;
        }

        /// <summary>
        /// Gets the user with the specified university identifier.
        /// </summary>
        /// <param name="univID">A university identifier.</param>
        /// <returns>The user if they exist.</returns>
        /// <exception cref="Exception">If the user does not exist.</exception>
        public User GetUser(string univID)
        {
            if (UserCache.Contains(univID))
            {
                return UserCache.Get(univID);
            }
            try
            {
                User user = DatabaseManager.InstanceOf().GetUser(univID);
                if (!UserCache.Contains(univID))
                {
                    UserCache.Add(univID, user);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generates a salted string of the specified length.
        /// </summary>
        /// <param name="length">The length of the salted string.</param>
        /// <returns>A randomly generated salted string.</returns>
        private string GenerateSaltString(int length)
        {
            char[] salt = new char[length];
            // Generate the salt
            for (int i = 0; i < length; i++)
            {
                salt[i] = RandomChars[HashGenerator.Next(RandomChars.Length)];
            }
            // Shuffle the salt
            int r = 0;
            for (int j = 0; j < 7; j++) {
                for (int i = 0; i < length; i++)
                {
                    r = HashGenerator.Next(salt.Length);
                    char c = salt[i];
                    salt[i] = salt[r];
                    salt[r] = c;
                }
            }
            return new String(salt);
        }

        /// <summary>
        /// Determines whether there is a user with the indicated university identifier cached in the manager.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>True if there is a user cached with the identifier, false otherwise.</returns>
        public bool HasUserCached(string univID)
        {
            return UserCache.Contains(univID);
        }

        /// <summary>
        /// Determines of the user is authenticated or not.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        /// <returns>True if the user is authenticated, false otherwise.</returns>
        public bool IsUserAuthenticated(string univID)
        {
            return AuthenticatedUsers.Contains(univID);
        }

        /// <summary>
        /// Hashes and salts a given item with the given salt.
        /// </summary>
        /// <param name="item">An item to hash and salt.</param>
        /// <param name="salt">The salt to combine with the item.</param>
        /// <returns></returns>
        public static string HashAndSalt(string item, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(salt + item);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        /// <summary>
        /// Logs a user out of the system.
        /// </summary>
        /// <param name="univID">The user's university identifier.</param>
        public void Logout(string univID)
        {
            if (IsUserAuthenticated(univID))
            {
                AuthenticatedUsers.Remove(univID);
                if (UserCache.Contains(univID)) {
                    UserCache.Remove(univID);
                }
            }
        }

        /// <summary>
        /// Verifies that a student meets the prerequisites for a course.
        /// </summary>
        /// <param name="univID">The student's univesrity identifier.</param>
        /// <param name="courseID">The course identifier.</param>
        /// <returns>True if a student meets the course's prerequisites.</returns>
        private bool VerifyPrerequisites(string univID, int courseID)
        {
            try
            {
                // Get the students record
                List<int> record = DatabaseManager.InstanceOf().GetAcademicRecord(univID).ConvertAll(r => r.Item1);
                // Get the courses prerequisites
                List<Tuple<int, int>> prereqs = CourseManager.InstanceOf().GetCourse(courseID)
                    .GetPrerequisites().AsList();
                // Determine whether they meet the prereqs or not
                int maxGroup = prereqs.Max(p => p.Item2);
                bool match = false;
                for (int group = 0; group <= maxGroup; group++)
                {
                    foreach (var prereq in prereqs.Where(p => p.Item2 == group))
                    {
                        if (record.Contains(prereq.Item1))
                        {
                            match = true;
                        }
                        else
                        {
                            match = false;
                            break;
                        }
                        if (match)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // If we made it here, we did not find a group where all prerequisites were met, therefore
            //  the student cannot register for the section
            return false;
        }

        /// <summary>
        /// Unenrolls a student from a section.
        /// </summary>
        /// <param name="univID">The student's university identifier.</param>
        /// <param name="sectionID">The section identifier.</param>
        /// <returns>True if the student was removed from the section, false otherwise.</returns>
        public bool UnenrollFromSection(string univID, int sectionID)
        {
            try
            {
                if (!DoesUserExist(univID))
                {
                    throw new Exception($"Error: User {univID} does not exist!");
                }
                if (!GetRole(univID).Equals(User.STUDENT))
                {
                    throw new Exception($"Error: User {univID} is not a student!");
                }
                if (!CourseManager.InstanceOf().DoesSectionExist(sectionID))
                {
                    throw new Exception("Error: Cannot unenroll student from section, section does not exist!");
                }
                if (DatabaseManager.InstanceOf().UnenrollFromSection(univID, sectionID))
                {
                    if (CourseManager.InstanceOf().HasSectionCached(sectionID))
                    {
                        CourseManager.InstanceOf().GetSection(sectionID).RemoveStudent(univID);
                    }
                    return true;
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
        }
    }
}