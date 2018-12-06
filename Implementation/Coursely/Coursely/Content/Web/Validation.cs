using System;
using System.Net.Mail;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public static class Validation
    {
        /// <summary>
        /// Gets the current term.
        /// </summary>
        /// <returns>A tuple containing a semester and a year.</returns>
        public static Tuple<string, int> GetCurrentTerm()
        {
            string semester = "";
            DateTime now = DateTime.Now;
            // Fall semester
            if (now.Month >= 8 && now.Month <= 12)
            {
                if (now.Month == 8)
                {
                    if (now.Date.Day < 15)
                    {
                        semester = "Summer";
                    }
                    else
                    {
                        semester = "Fall";
                    }
                }
                else
                {
                    semester = "Fall";
                }
            }
            // Spring semester
            else if (now.Month >= 1 || now.Month <= 5)
            {
                if (now.Month == 5)
                {
                    if (now.Date.Day < 15)
                    {
                        semester = "Spring";
                    }
                    else
                    {
                        semester = "Summer";
                    }
                }
                else
                {
                    semester = "Spring";
                }
            }
            // Summer semester
            else
            {
                if (now.Month == 8)
                {
                    if (now.Date.Day < 15)
                    {
                        semester = "Summer";
                    }
                    else
                    {
                        semester = "Fall";
                    }
                }
                else
                {
                    semester = "Summer";
                }
            }
            return new Tuple<string, int>(semester, now.Year);
        }

        /// <summary>
        /// Determines whether a single prerequisite is valid or not.
        /// </summary>
        /// <param name="abbreviation">A department name abbreviation.</param>
        /// <param name="courseNumber">A course number.</param>
        /// <returns>True if a prerequisite is valid, false otherwise.</returns>
        private static bool IsValidPrereq(string abbreviation, string courseNumber)
        {
            try
            {
                return CourseManager.InstanceOf().DoesCourseExist(
                    UniversityManager.InstanceOf().GetDepartmentID(abbreviation), courseNumber);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates that a selected building is valid and that its corresponding building exists.
        /// </summary>
        /// <param name="building">A building identifier.</param>
        /// <param name="error">An error message if one is generated.</param>
        /// <returns>True if the building identifier is valid, false otherwise.</returns>
        public static bool ValidateBuilding(string building, out string error)
        {
            error = "";
            if (building.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                error = "";
                return false;
            }
            if (!int.TryParse(building, out int b))
            {
                error = "";
                return false;
            }
            try
            {
                if (!UniversityManager.InstanceOf().DoesBuildingExist(b))
                {
                    error = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether a given building name is valid.
        /// </summary>
        /// <param name="name">A building name.</param>
        /// <param name="error">An error message if one occurs.</param>
        /// <returns>True if the building name is valid, false otherwise.</returns>
        public static bool ValidateBuildingName(string name, out string error)
        {
            error = "";
            if (!ValidateString(name, 8, 64, out error))
            {
                return false;
            }
            // See if the building already exists
            try
            {
                if (UniversityManager.InstanceOf().DoesBuildingExist(name))
                {
                    error = $"Error: Building with name: {name} already exists!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a building's name abbreviation is valid.
        /// </summary>
        /// <param name="abbreviation">A building name abbreviaton.</param>
        /// <param name="error">An error message, if one occurs.</param>
        /// <returns>True if the name is valid, false otherwise.</returns>
        public static bool ValidateBuildingAbbreviation(string abbreviation, out string error) =>
            ValidateString(abbreviation, 2, 4, out error);

        /// <summary>
        /// Validates that a course identifer is valid and that its corresponding course exists.
        /// </summary>
        /// <param name="course">A course identifier.</param>
        /// <param name="error">An error message, if one is generated.</param>
        /// <returns>True if the course identifier is valid, false otherwise.</returns>
        public static bool ValidateCourse(string course, out string error)
        {
            error = "";
            if (course.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                error = "Error: You must select a valid course!";
                return false;
            }
            if (!int.TryParse(course, out int c))
            {
                error = "Error: You must select a valid course!";
                return false;
            }
            try
            {
                if (!CourseManager.InstanceOf().DoesCourseExist(c))
                {
                    error = "Error: Selected course does not exist!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credits"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateCourseCredits(string credits, out string error) =>
            ValidateNumber(credits, 1, 5, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="error"></param>
        /// <returns></returns>Y
        public static bool ValidateCourseDescription(string description, out string error) =>
            ValidateString(description, 32, 1024, out error);

        /// <summary>
        /// Validates a course name.
        /// </summary>
        /// <param name="name">The course name.</param>
        /// <param name="error">An error, if one is generated.</param>
        /// <returns>True if the course name validates correctly, false otherwise.</returns>
        public static bool ValidateCourseName(string name, bool modifying, out string error)
        {
            if (!ValidateString(name, 8, 64, out error))
            {
                return false;
            }
            if (!modifying)
            {
                try
                {
                    if (CourseManager.InstanceOf().DoesCourseExist(name))
                    {
                        error = "Error: A course with the specfied name already exists!";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates that a given course number is valid, and that a course does not already exist with the
        /// given course number.
        /// </summary>
        /// <param name="department">The identifier for the department the course will belong to.</param>
        /// <param name="number">The course number itself.</param>
        /// <param name="error">An error message, if one occurs.</param>
        /// <returns>True if a course number is valid, false otherwise.</returns>
        public static bool ValidateCourseNumber(string department, string number, bool modifying,
            out string error)
        {
            error = "";
            if (!ValidateNumber(number, 1, 9999, out error))
            {
                return false;
            }
            if (!ValidateDepartment(department, out error))
            {
                return false;
            }
            if (!modifying) {
                try
                {
                    if (CourseManager.InstanceOf().DoesCourseExist(int.Parse(department), number))
                    {
                        error = "Error: A course already exists for the given department and course number!";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDateAndTime"></param>
        /// <param name="endDateAndTime"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateDatesAndTimes(string startDateAndTime, string endDateAndTime, 
            out string error)
        {
            error = "";
            // Make sure a valid date time was entered for the start date and time
            if (!DateTime.TryParse(startDateAndTime, out DateTime sdt))
            {
                error = "Error: Start date and time not valid!";
                return false;
            }
            // Make sure a valid date time was entered for the end date and time
            if (!DateTime.TryParse(endDateAndTime, out DateTime edt))
            {
                error = "Error: End date and time not valid!";
                return false;
            }
            // Make sure that the end date comes after the start date
            if (sdt > edt)
            {
                error = "Error: Start date and time must be before the end date and time!";
                return false;
            }
            // Make sure there is at least a 50 minute time difference between the start and end time
            if (edt.Subtract(sdt).TotalMinutes < 50)
            {
                error = "Error: Start time and end time must be at least 50 minutes apart!";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static bool ValidateDay(string day, out string error)
        {
            error = "";
            if (!WebControls.DAYS.Contains(day))
            {
                error = "Error: You have entered an invalid day!";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="department"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateDepartment(string department, out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(department))
            {
                error = "Error: You must select a department!";
                return false;
            }
            if (department.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                error = "Error: You must select a department for the course!";
                return false;
            }
            if (!int.TryParse(department, out int d))
            {
                error = "Error: You must enter a number for the department!";
                return false;
            }
            try
            {
                if (!UniversityManager.InstanceOf().DoesDepartmentExist(d))
                {
                    error = "Error: Selected department does not exist!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateDepartmentAbbreviation(string abbreviation, out string error) =>
            ValidateString(abbreviation, 3, 4, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateDepartmentName(string name, out string error) =>
            ValidateString(name, 8, 64, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email, out string error)
        {
            if (!ValidateString(email, 6, 128, out error))
            {
                return false;
            }
            try
            {
                new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool ValidateFirstName(string name, out string error) =>
            ValidateString(name, 2, 32, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool ValidateLastName(string name, out string error) =>
            ValidateString(name, 2, 32, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateMajor(string major, out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(major))
            {
                error = "Error: You must select a major!";
                return false;
            }
            if (major.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                error = "Error: You must select a major!";
                return false;
            }
            if (!int.TryParse(major, out int m))
            {
                error = "Error: You have selected an invalid major!";
                return false;
            }
            try
            {
                if (!UniversityManager.InstanceOf().DoesMajorExist(m))
                {
                    error = "Error: Selected major does not exist!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates a major name.
        /// </summary>
        /// <param name="name">A major name.</param>
        /// <param name="error">An error if one occurred.</param>
        /// <returns>True if the major name is valid, false otherwise.</returns>
        public static bool ValidateMajorName(string name, out string error) =>
            ValidateString(name, 8, 64, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateMaxEnrollment(string enrollment, out string error) =>
            ValidateNumber(enrollment, 5, 200, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static bool ValidateNumber(string number, int lower, int upper, out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(number))
            {
                error = "Error: You must enter a number!";
                return false;
            }
            if (!int.TryParse(number, out int n))
            {
                error = "Error: You must enter an integer for the number!";
                return false;
            }
            if (n < lower || n > upper)
            {
                error = $"Error: Number {n} does not satisfy {lower} <= {n} <= {upper}";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, out string error) =>
            ValidateString(password, 8, 64, out error);

        /// <summary>
        /// Determines whether a set of prerequisites are valid.
        /// </summary>
        /// <param name="prereqs">A string containing the prerequisites.</param>
        /// <param name="error">A string to capture the error.</param>
        /// <returns>True if the all prerequisites are valid, false otherwise.</returns>
        public static bool ValidatePrereqs(string prereqs, out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(prereqs))
            {
                return true;
            }

            string upper = prereqs.ToUpper();

            if (upper.EndsWith("AND") || upper.EndsWith("OR"))
            {
                error = "Error: Course prerequisites are not in the correct format!";
                return false;
            }

            var current = "";
            var split = upper.Split(' ');

            if (split.Length == 1)
            {
                error = "Error: Course prerequisites are not in the correct format!";
                return false;
            }

            for (var i = 0; i < split.Length - 1; i++)
            {
                current = split[i];
                if (current.Equals("AND") || current.Equals("OR"))
                {
                    // Error if next is a number or we end with a department abbreviation
                    if (!split[i + 1].IsNaN() || i + 2 >= split.Length)
                    {
                        error = "Error: Course prerequisites are not in the correct format!";
                        return false;
                    }
                    // Check that the prerequisite exists
                    if (!IsValidPrereq(current, split[i + 1]))
                    {
                        error = $"Error: Course: {current} {split[i + 1]}, does not exist!";
                        return false;
                    }
                }
                else
                {
                    // Check for a string, we are at a department abbreviation
                    if (current.IsNaN())
                    {
                        // Error if next is a string
                        if (split[i + 1].IsNaN())
                        {
                            error = "Error: Course prerequisites are not in the correct format!";
                            return false;
                        }
                    }
                    else
                    {
                        // Error if we are not at the end of the prerequisites or the next is not a combinator
                        if (!(string.IsNullOrWhiteSpace(split[i + 1]) || split[i + 1].Equals("AND") || split[i + 1].Equals("OR")))
                        {
                            error = "Error: Course prerequisites are not in the correct format!";
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateRoom(string room, out string error) =>
            ValidateNumber(room, 1, 9999, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns></returns>
        public static bool ValidateSection(string sectionID, out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(sectionID))
            {
                error = "Error: You must select a section!";
                return false;
            }
            if (!int.TryParse(sectionID, out int s))
            {
                error = "Error: You have selected an invalid section!";
                return false;
            }
            if (s == -1)
            {
                error = "Error: You have selected an invalid section!";
                return false;
            }
            if (!CourseManager.InstanceOf().DoesSectionExist(s))
            {
                error = "Error: Selected section does not exist!";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateSectionNumber(string number, out string error) =>
            ValidateNumber(number, 1, 999, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="school"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateSchool(string school, out string error)
        {
            error = "";
            if (school.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                error = "Error: You must select a school!";
                return false;
            }
            if (!int.TryParse(school, out int s))
            {
                error = "Error: Selectee school does not exist!";
                return false;
            }
            try
            {
                if (!UniversityManager.InstanceOf().DoesSchoolExist(s))
                {
                    error = "Error: Selected school does not exist!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateSchoolAbbreviation(string abbreviation, out string error) =>
            ValidateString(abbreviation, 0, 4, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateSchoolName(string name, out string error) =>
            ValidateString(name, 8, 64, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semester"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateSemester(string semester, out string error)
        {
            error = "";
            if (!WebControls.SEMESTERS.Contains(semester))
            {
                error = "Error you have entered an invalid semester!";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a string is not empty or null, and falls within the specified length.
        /// </summary>
        /// <param name="str">A string to validate.</param>
        /// <param name="lower">The lower exclusive bound on the string length.</param>
        /// <param name="upper">The upper exclusive bound on the string length.</param>
        /// <param name="error">An error, if it occurs.</param>
        /// <returns>True if a string validates correctly, false otherwise.</returns>
        private static bool ValidateString(string str, int lower, int upper, out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(str))
            {
                error = "Error: You must enter a string!";
                return false;
            }
            if (int.TryParse(str, out int n))
            {
                error = "Error: String cannot be a number!";
                return false;
            }
            if (str.Length < lower || str.Length > upper)
            {
                error = $"Error: String must be [{lower}, {upper}] characters in length!";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="univID"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateUnivID(string univID, out string error) => 
            ValidateNumber(univID, 0, 999999999, out error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ValidateYear(string year, out string error) =>
            ValidateNumber(year, WebControls.MinYear(), WebControls.MaxYear(), out error);



        /// <summary>
        /// Determines whether a user's email conforms to security standards.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns>Determines whether a user's email conforms to security standards.</returns>
        public static bool ValidateEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            if (email.Length > 64)
            {
                return false;
            }
            try
            {
                MailAddress address = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether a user's password conforms to security standards.
        /// </summary>
        /// <param name="password">The user's password.</param>
        /// <returns>True if the user's password conforms to security standards, false otherwise.</returns>
        public static bool ValidatePassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            if (password.Length < 8 || password.Length > 64)
            {
                return false;
            }
            return true;
        }
    }
}