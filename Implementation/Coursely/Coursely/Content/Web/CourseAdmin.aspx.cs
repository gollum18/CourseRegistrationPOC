using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class CourseAdmin : Page, IPostBackEventHandler
    {
        /// <summary>
        /// Sets up the page on load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] == null || Session["Role"] == null ||
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }
            if (!Session["Role"].ToString().Equals(Classes.User.ADMINISTRATOR))
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }
            
            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Manage Courses";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;
            
            // Initialize the schools selector
            if (SchoolSelector.Items.Count == 1) {
                try
                {
                    foreach (var school in UniversityManager.InstanceOf().GetSchools())
                    {
                        SchoolSelector.Items.Add(new ListItem(school.Name, school.SchoolID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }

            // Initialize the buidlings selector
            if (SectionBuilding.Items.Count == 0)
            {
                try
                {
                    foreach (var building in UniversityManager.InstanceOf().GetBuildings())
                    {
                        SectionBuilding.Items.Add(new ListItem(building.Name, building.BuildingID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }

            // Initialize the year selector
            if (YearSelector.Items.Count == 0) {
                foreach (var year in WebControls.GetYearsForDropDown())
                {
                    YearSelector.Items.Add(new ListItem(year.ToString(), year.ToString()));
                }
            }

            // Initialize the days selector
            if (SectionDays.Items.Count == 0)
            {
                foreach (var day in WebControls.DAYS)
                {
                    SectionDays.Items.Add(new ListItem(day, day));
                }
            }

            // Reset the status label text
            StatusLabel.Text = "";
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }

        /// <summary>
        /// Loads departments into the department selector.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSchoolIndexChanged(object sender, EventArgs e)
        {
            if (SchoolSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.ResetSelector(DepartmentSelector, WebControls.DEFAULT_LISTITEM_DEPARTMENT);
                WebControls.ResetSelector(CourseSelector, WebControls.DEFAULT_LISTITEM_COURSE);
                WebControls.ResetSelector(SectionSelector, WebControls.DEFAULT_LISTITEM_SECTION);
            }
            else
            {
                try
                {
                    WebControls.ResetSelector(DepartmentSelector, WebControls.DEFAULT_LISTITEM_DEPARTMENT);
                    foreach (var department in UniversityManager.InstanceOf().GetDepartments(
                        int.Parse(SchoolSelector.SelectedValue)))
                    {
                        DepartmentSelector.Items.Add(new ListItem(department.Name, department.DepartmentID.ToString()));
                    }
                    if (!StatusLabel.Text.Equals(""))
                    {
                        StatusLabel.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Loads courses into the course selector.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDepartmentIndexChanged(object sender, EventArgs e)
        {
            if (DepartmentSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.ResetSelector(CourseSelector, WebControls.DEFAULT_LISTITEM_COURSE);
                WebControls.ResetSelector(SectionSelector, WebControls.DEFAULT_LISTITEM_SECTION);
            }
            else
            {
                try
                {
                    int department = int.Parse(DepartmentSelector.SelectedValue);
                    WebControls.ResetSelector(CourseSelector, WebControls.DEFAULT_LISTITEM_COURSE);
                    foreach (var course in CourseManager.InstanceOf().GetCourses(
                        department))
                    {
                        CourseSelector.Items.Add(new ListItem(course.Name, course.CourseID.ToString()));
                    }
                    // Load in the instructors to the instructor selector
                    SectionInstructors.Items.Clear();
                    foreach (var instructor in UserManager.InstanceOf().GetDepartmentInstructors(department))
                    {
                        SectionInstructors.Items.Add(new ListItem($"{instructor.LastName}, {instructor.FirstName}", instructor.UnivID));
                    }
                    if (!StatusLabel.Text.Equals(""))
                    {
                        StatusLabel.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Loads course data onto the form and sections into the section selector.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCourseIndexChanged(object sender, EventArgs e)
        {
            if (CourseSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.ResetSelector(SectionSelector, WebControls.DEFAULT_LISTITEM_SECTION);
            }
            else
            {
                try
                {
                    WebControls.ResetSelector(SectionSelector, WebControls.DEFAULT_LISTITEM_SECTION);
                    foreach (var section in CourseManager.InstanceOf().GetSections(
                        int.Parse(CourseSelector.SelectedValue),
                        new Tuple<string, int>(SemesterSelector.SelectedValue, int.Parse(YearSelector.SelectedValue))))
                    {
                        SectionSelector.Items.Add(new ListItem(section.ToString(), section.SectionID.ToString()));
                    }
                    // Load in the course information
                    Course course = CourseManager.InstanceOf().GetCourse(int.Parse(CourseSelector.SelectedValue));
                    CourseArchived.Checked = course.Archived;
                    CourseCredits.Text = course.Credits.ToString();
                    CourseDescription.Text = course.Description;
                    CourseName.Text = course.Name;
                    CourseNumber.Text = course.Number;
                    string prereqStr = "";
                    IEnumerator<Tuple<int, int>> prereqs = course.GetPrerequisites();
                    int prev = 0;
                    while (prereqs.MoveNext())
                    {
                        prereqStr += $"{UniversityManager.InstanceOf().GetDepartment(CourseManager.InstanceOf().GetCourse(prereqs.Current.Item1).DepartmentID).Abbreviation} " +
                            $"{CourseManager.InstanceOf().GetCourse(prereqs.Current.Item1).Number} {(prereqs.Current.Item2 == prev ? "AND " : "OR ")}";
                        prev = prereqs.Current.Item2;
                    }
                    prereqStr = prereqStr.TrimEnd();
                    if (prereqStr.EndsWith("AND"))
                    {
                        prereqStr = prereqStr.Substring(0, prereqStr.LastIndexOf("AND"));
                    }
                    else if (prereqStr.EndsWith("OR"))
                    {
                        prereqStr = prereqStr.Substring(0, prereqStr.LastIndexOf("OR"));
                    }
                    CoursePrereqs.Text = prereqStr.TrimEnd();
                    if (!StatusLabel.Text.Equals(""))
                    {
                        StatusLabel.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Lodas section data onto the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSectionIndexChanged(object sender, EventArgs e)
        {
            if (SectionSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                // Reset the controls
                SectionNumber.Text = "";
                SectionEnrollment.Text = "";
                SectionRoom.Text = "";
                SectionStartDate.Text = "";
                SectionStartTime.Text = "";
                SectionEndDate.Text = "";
                SectionEndTime.Text = "";
                SectionInstructors.Items.Clear();
                foreach (ListItem day in SectionDays.Items)
                {
                    day.Selected = false;
                }
            }
            else
            {
                Section section = CourseManager.InstanceOf().GetSection(int.Parse(SectionSelector.SelectedValue));
                SectionNumber.Text = section.Number;
                SectionEnrollment.Text = section.MaxEnrollment.ToString();
                SectionRoom.Text = section.Room.ToString();
                SectionStartDate.Text = section.StartDateAndTime.ToString("yyyyMMdd");
                SectionStartTime.Text = section.StartDateAndTime.ToString("yyyyMMdd");
                SectionEndDate.Text = section.EndDateAndTime.ToString("hh:mm tt");
                SectionEndTime.Text = section.EndDateAndTime.ToString("hh:mm tt");
                IEnumerator<string> days = section.GetDays();
                while (days.MoveNext())
                {
                    SectionDays.Items.FindByValue(days.Current).Selected = true;
                }
                try
                {
                    IEnumerator<string> instructors = section.GetInstructors();
                    while (instructors.MoveNext())
                    {
                        SectionInstructors.Items.FindByValue(
                            UserManager.InstanceOf().GetUser(instructors.Current).UnivID).Selected = true;
                    }
                    if (!StatusLabel.Text.Equals(""))
                    {
                        StatusLabel.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        // Post back event handler

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("AddCourse"))
            {
                AddCourse();
            }
            else if (eventArgument.Equals("AddSection"))
            {
                AddSection();
            }
            else if (eventArgument.Equals("ModifyCourse"))
            {
                ModifyCourse();
            }
            else if (eventArgument.Equals("ModifySection"))
            {
                ModifySection();
            }
            else
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, $"Error: Invalid action: \"{eventArgument}\" was specified!");
            }
        }
        
        /// <summary>
        /// Validates that the user entered valid course data.
        /// </summary>
        /// <param name="error">An error message.</param>
        /// <returns>True if all course data is valid, false otherwise.</returns>
        private bool ValidateCourseData(bool modifying, out string error)
        {
            error = "";
            // Validate the department
            if (!Validation.ValidateDepartment(DepartmentSelector.SelectedValue, out error))
            {
                return false;
            }
            // Validate the course name
            if (!Validation.ValidateCourseName(CourseName.Text, modifying, out error))
            {
                return false;
            }
            // Validate the course number
            if (!Validation.ValidateCourseNumber(DepartmentSelector.SelectedValue, CourseNumber.Text, modifying, out error))
            {
                return false;
            }
            // Validate the course credits
            if (!Validation.ValidateCourseCredits(CourseCredits.Text, out error))
            {
                return false;
            }
            // Validate the course description
            if (!Validation.ValidateCourseDescription(CourseDescription.Text, out error))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that the user entered valid sectiond data.
        /// </summary>
        /// <param name="message">An error message.</param>
        /// <returns>True if all section data is valid, false otherwise.</returns>
        private bool ValidateSectionData(out string message)
        {
            message = "";
            if (!Validation.ValidateCourse(CourseSelector.SelectedValue, out message))
            {
                return false;
            }
            if (!Validation.ValidateBuilding(SectionBuilding.SelectedValue, out message))
            {
                return false;
            }
            if (!Validation.ValidateRoom(SectionRoom.Text, out message))
            {
                return false;
            }
            if (!Validation.ValidateSectionNumber(SectionNumber.Text, out message))
            {
                return false;
            }
            if (!Validation.ValidateSemester(SemesterSelector.SelectedValue, out message))
            {
                return false;
            }
            if (!Validation.ValidateYear(YearSelector.SelectedValue, out message))
            {
                return false;
            }
            if (!Validation.ValidateDatesAndTimes($"{SectionStartDate.Text} {SectionStartTime.Text}", 
                $"{SectionEndDate.Text} {SectionEndTime.Text}", out message))
            {
                return false;
            }
            if (!Validation.ValidateMaxEnrollment(SectionEnrollment.Text, out message))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a (courseID, group) pairs from a string containing prerequisites.
        /// </summary>
        /// <param name="prereqStr">A prerequisite string.</param>
        /// <returns>A list of (courseID, group) pairs.</returns>
        private List<Tuple<int, int>> GetPrerequisites(string prereqStr)
        {
            if (!Validation.ValidatePrereqs(prereqStr, out string message))
            {
                throw new Exception(message);
            }
            if (string.IsNullOrWhiteSpace(prereqStr))
            {
                return null;
            }
            List<Tuple<int, int>> prereqList = new List<Tuple<int, int>>();
            // Add a dummy entry to the prerequisite string for easier processing
            prereqStr += " DUMMY";
            string[] prereqs = prereqStr.Split();
            int departmentID = -1;
            int courseID = -1;
            // only one prerequisite (department, number, and DUMMY should be in prereqs array)
            try
            {
                if (prereqs.Length == 3)
                {
                    departmentID = UniversityManager.InstanceOf().GetDepartmentID(prereqs[0]);
                    courseID = CourseManager.InstanceOf().GetCourseID(departmentID, prereqs[1]);
                    prereqList.Add(new Tuple<int, int>(courseID, 0));
                }
                // Otherwise we are dealing with more than one prerequisite
                else
                {
                    int groupID = 0;
                    for (int i = 0; i < prereqs.Length - 2; i += 3)
                    {
                        departmentID = UniversityManager.InstanceOf().GetDepartmentID(prereqs[i]);
                        courseID = CourseManager.InstanceOf().GetCourseID(departmentID, prereqs[i + 1]);
                        if (prereqs[i + 2].Equals("OR"))
                        {
                            groupID++;
                        }
                        prereqList.Add(new Tuple<int, int>(courseID, groupID));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return prereqList;
        }

        // Button event handlers

        /// <summary>
        /// Adds a course to the database.
        /// </summary>
        protected void AddCourse()
        {
            if (!ValidateCourseData(false, out string message))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, message);
            }
            else
            {
                try
                {
                    if (!CourseManager.InstanceOf().DoesCourseExist(int.Parse(DepartmentSelector.SelectedValue), 
                            CourseNumber.Text))
                    {
                        if (!CourseManager.InstanceOf().AddCourse(int.Parse(DepartmentSelector.SelectedValue),
                            CourseName.Text, CourseNumber.Text, int.Parse(CourseCredits.Text),
                            CourseArchived.Checked, CourseDescription.Text, GetPrerequisites(CoursePrereqs.Text)))
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue creating the course!");
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Course was created successfully!");
                        }
                    }
                    else
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Cannot create course, course already exists!");
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Adds a section to the database.
        /// </summary>
        protected void AddSection()
        {
            if (!ValidateSectionData(out string message))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, message);
            }
            else
            {
                try
                {
                    if (!CourseManager.InstanceOf().DoesSectionExist(
                            new Tuple<string, int>(SemesterSelector.SelectedValue, int.Parse(YearSelector.Text)),
                            int.Parse(CourseSelector.SelectedValue), SectionNumber.Text))
                    {
                        List<string> instructors = new List<string>();
                        foreach (var item in WebControls.GetSelectedItems(SectionInstructors))
                        {
                            instructors.Add(item.Value);
                        }
                        List<string> days = new List<string>();
                        foreach (var item in WebControls.GetSelectedItems(SectionDays))
                        {
                            days.Add(item.Value);
                        }
                        if (!CourseManager.InstanceOf().AddSection(
                            int.Parse(CourseSelector.SelectedValue), int.Parse(SectionBuilding.SelectedValue), 
                            SectionNumber.Text, SemesterSelector.SelectedValue, 
                            int.Parse(YearSelector.SelectedValue), int.Parse(SectionRoom.Text), 
                            DateTime.Parse($"{SectionStartDate.Text} {SectionStartTime.Text}"), 
                            DateTime.Parse($"{SectionEndDate.Text} {SectionEndTime.Text}"), 
                            int.Parse(SectionEnrollment.Text), instructors, days))
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue creating the section!");
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Section created successfully!");
                        }
                    }
                    else
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Cannot create section, section already exists!");
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Modifies a course in the database.
        /// </summary>
        protected void ModifyCourse()
        {
            if (!ValidateCourseData(true, out string message))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, message);
            }
            else
            {
                try
                {
                    int courseID = int.Parse(CourseSelector.SelectedValue);
                    if (CourseManager.InstanceOf().DoesCourseExist(courseID))
                    {
                        if (!CourseManager.InstanceOf().ModifyCourse(courseID, 
                            CourseNumber.Text, int.Parse(CourseCredits.Text), 
                            CourseArchived.Checked, CourseDescription.Text))
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue modifying the course!");
                        }
                        else
                        {
                            if (CourseManager.InstanceOf().ModifyPrerequisites(courseID, 
                                GetPrerequisites(CoursePrereqs.Text)))
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Course modified successfully!");
                            }
                            else
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue modifying the course!");
                            }
                        }
                    }
                    else
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Cannot modify course, course does not exist!");
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Modifies a section in the database.
        /// </summary>
        protected void ModifySection()
        {
            if (!ValidateSectionData(out string message))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, message);
            }
            else
            {
                try
                {
                    if (CourseManager.InstanceOf().DoesSectionExist(int.Parse(SectionSelector.Text)))
                    {
                        List<string> days = new List<string>();
                        foreach (var day in WebControls.GetSelectedItems(SectionDays)) {
                            days.Add(day.Value);
                        }
                        List<string> instructors = new List<string>();
                        foreach (var instructor in WebControls.GetSelectedItems(SectionInstructors)) {
                            instructors.Add(instructor.Value);
                        }
                        if (!CourseManager.InstanceOf().ModifySection(int.Parse(SectionSelector.Text), 
                            int.Parse(SectionBuilding.SelectedValue), int.Parse(SectionRoom.Text), 
                            DateTime.Parse($"{SectionStartDate.Text} {SectionStartTime.Text}"), 
                            DateTime.Parse($"{SectionEndDate.Text} {SectionEndTime.Text}"), 
                            int.Parse(SectionEnrollment.Text), days, instructors))
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue modifying the section!");
                        }
                        days = null;
                        instructors = null;
                    }
                    else
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Cannot modify section, section does not exist!");
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }
    }
}