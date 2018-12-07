using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class ManageAdvisees : System.Web.UI.Page, IPostBackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] == null || Session["Role"] == null ||
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }
            if (!Session["Role"].ToString().Equals(Classes.User.INSTRUCTOR))
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }

            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Manage Advisees";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            if (AdviseeDD.Items.Count == 0) {
                // Get the instructors advisees, and load them into the selector
                try
                {
                    foreach (var advisee in UserManager.InstanceOf().GetInstructorAdvisees(
                        Session["UnivID"].ToString()))
                    {
                        AdviseeDD.Items.Add(new ListItem($"{advisee.LastName}, {advisee.FirstName}", 
                            advisee.UnivID));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }

            // Load in the years for the year selector
            if (YearDD.Items.Count == 0)
            {
                foreach (var year in WebControls.GetYearsForDropDown())
                {
                    YearDD.Items.Add(new ListItem(year.ToString(), year.ToString()));
                }
            }

            // Load in the schools to the school selector
            if (SchoolDD.Items.Count == 1) {
                try
                {
                    foreach (var school in UniversityManager.InstanceOf().GetSchools())
                    {
                        SchoolDD.Items.Add(new ListItem(school.Name, school.SchoolID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("OverrideSection"))
            {
                OverrideSection();
            }
            else if (eventArgument.Equals("ViewRecord"))
            {
                ViewRecord();
            }
            else if (eventArgument.Equals("ViewSchedule"))
            {
                ViewSchedule();
            }
            else
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid action specified!");
            }
        }

        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }

        private bool ValidateAdvisee(string advisee, out string error)
        {
            error = "";
            // Validate the advisee
            if (!Validation.ValidateUnivID(advisee, out error))
            {
                return false;
            }
            // Make sure the advisee is a student
            if (!UserManager.InstanceOf().GetRole(advisee).Equals(Classes.User.STUDENT))
            {
                error = "Error: Cannot display record for user, user is not a student!";
                return false;
            }
            return true;
        }

        private void OverrideSection()
        {
            try
            {
                // Get the advisee
                string advisee = AdviseeDD.SelectedValue;
                // Validate the advisee
                if (!ValidateAdvisee(advisee, out string error))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                    return;
                }
                // Validate the section
                if (!Validation.ValidateSection(SectionSelector.SelectedValue, out error))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                    return;
                }
                // Get the section
                int section = int.Parse(SectionSelector.SelectedValue);
                // Attempt the student in the section
                // TODO: Validate the term
                Tuple<string, int> term = new Tuple<string, int>(SemesterDD.SelectedValue, 
                    int.Parse(YearDD.SelectedValue));
                if (!UserManager.InstanceOf().EnrollInSection(advisee, section, term, true))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue overriding " +
                        $"the course for {AdviseeDD.SelectedItem.Text}!");
                }
                else
                {
                    WebControls.SetLabel(StatusLabel, WebControls.GREEN, $"Student {AdviseeDD.SelectedItem.Text} was successfully " +
                        $"enrolled in {SectionSelector.SelectedItem.Text}!");
                    SectionSelector.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        private void ViewRecord()
        {
            // Clear out the previous record
            RecordTable.Rows.Clear();
            try
            {
                // Get the advisee
                string advisee = AdviseeDD.SelectedValue;
                // Validate the advisee
                if (!ValidateAdvisee(advisee, out string error))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                    return;
                }
                // Get the advisees record
                List<string> record = UserManager.InstanceOf().GetAcademicRecord(advisee);
                // If the student has records them in, otherwise display a info message
                if (record.Count > 0)
                {
                    // Create the table rows and load them into the table
                    foreach (var row in WebControls.GenerateRecordRows(AdviseeDD.SelectedItem.Text, record))
                    {
                        RecordTable.Rows.Add(row);
                    }
                    if (!StatusLabel.Text.Equals(""))
                    {
                        StatusLabel.Text = "";
                    }
                    RecordView.Visible = true;
                }
                else
                {
                    WebControls.SetLabel(StatusLabel, WebControls.DARK_ORANGE, $"Info: Advisee {AdviseeDD.SelectedItem.Text} does not have any grades to display!");
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        protected void OnSchoolChanged(object sender, EventArgs e)
        {
            if (!Validation.ValidateSchool(SchoolDD.SelectedValue, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            else
            {
                int schoolID = int.Parse(SchoolDD.SelectedValue);
                try
                {
                    DepartmentDD.Items.Clear();
                    foreach (var department in UniversityManager.InstanceOf().GetDepartments(schoolID))
                    {
                        DepartmentDD.Items.Add(new ListItem(department.Name, department.DepartmentID.ToString()));
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

        protected void OnDepartmentChanged(object sender, EventArgs e)
        {
            if (!Validation.ValidateDepartment(DepartmentDD.SelectedValue, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            else
            {
                int departmentID = int.Parse(DepartmentDD.SelectedValue);
                try
                {
                    CourseDD.Items.Clear();
                    foreach (var course in CourseManager.InstanceOf().GetCourses(departmentID))
                    {
                        CourseDD.Items.Add(new ListItem(course.Name, course.CourseID.ToString()));
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
                if (!StatusLabel.Text.Equals(""))
                {
                    StatusLabel.Text = "";
                }
            }
        }

        protected void OnCourseChanged(object sender, EventArgs e)
        {
            if (!Validation.ValidateCourse(CourseDD.SelectedValue, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            else
            {
                int courseID = int.Parse(CourseDD.SelectedValue);
                try
                {
                    Tuple<string, int> term = new Tuple<string, int>(SemesterDD.SelectedValue,
                        int.Parse(YearDD.SelectedValue));
                    List<Section> sections = CourseManager.InstanceOf().GetSections(
                        courseID, term);
                    // if the selected course has sections, populate the section display and selector
                    if (sections.Count > 0) {
                        SectionTable.Rows.Clear();
                        SectionSelector.Items.Clear();
                        foreach (var row in WebControls.CreateScheduleRows(sections))
                        {
                            SectionTable.Rows.Add(row);
                        }
                        foreach (var section in sections)
                        {
                            SectionSelector.Items.Add(new ListItem(section.ToString(),
                                section.SectionID.ToString()));
                        }
                        if (!StatusLabel.Text.Equals(""))
                        {
                            StatusLabel.Text = "";
                        }
                    }
                    else
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.DARK_ORANGE, "Info: Selected course has no sections " +
                            $"for {term.Item1} {term.Item2}!");
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        private void ViewSchedule()
        {
            // Get the advisee
            string advisee = AdviseeDD.SelectedValue;
            // Validate the advisee
            if (!ValidateAdvisee(advisee, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            // Get the semester
            string semester = SemesterDD.SelectedValue;
            // Validate the semester
            if (!Validation.ValidateSemester(semester, out error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            // Validate the year
            if (int.TryParse(YearDD.SelectedValue, out int year))
            {
                if (!WebControls.GetYearsForDropDown().Contains(year))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid year selected!");
                    return;
                }
                // Load in the schedule for the selected advisee
                try
                {
                    ScheduleTable.Rows.Clear();
                    Tuple<string, int> term = new Tuple<string, int>(semester, year);
                    List<Section> schedule = UserManager.InstanceOf().GetSchedule(term, advisee, true);
                    if (schedule.Count > 0) {
                        foreach (var row in WebControls.CreateScheduleRows(schedule))
                        {
                            ScheduleTable.Rows.Add(row);
                        }
                        if (!StatusLabel.Text.Equals(""))
                        {
                            StatusLabel.Text = "";
                        }
                        ScheduleView.Visible = true;
                    }
                    else
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.DARK_ORANGE, $"Info: Selected advisee has no " +
                            $"courses scheduled for {term.Item1} {term.Item2}!");
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
            else
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid year selected!");
                return;
            }
        }
    }
}