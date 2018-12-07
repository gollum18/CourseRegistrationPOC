using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class Enroll : System.Web.UI.Page, IPostBackEventHandler
    {
        protected void OnSchoolChanged(object sender, EventArgs e)
        {
            if (!SchoolSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE) && 
                    int.TryParse(SchoolSelector.SelectedValue, out int schoolID))
            {
                try
                {
                    DepartmentSelector.Items.Clear();
                    foreach (var department in UniversityManager.InstanceOf().GetDepartments(schoolID))
                    {
                        DepartmentSelector.Items.Add(new ListItem(department.Name, department.DepartmentID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.ToString());
                }
            }
        }

        protected void OnDepartmentChanged(object sender, EventArgs e)
        {
            if (!DepartmentSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE) && 
                    int.TryParse(DepartmentSelector.SelectedValue, out int departmentID))
            {
                try
                {
                    CourseSelector.Items.Clear();
                    foreach (var course in CourseManager.InstanceOf().GetCourses(departmentID))
                    {
                        CourseSelector.Items.Add(new ListItem(course.Name, course.CourseID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.ToString());
                }
            }
        }

        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] == null || Session["Role"] == null ||
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }

            if (!Session["Role"].Equals(Classes.User.STUDENT))
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }

            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Manage Account";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            // Load in the schools
            if (SchoolSelector.Items.Count == 0)
            {
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

            // Load in the years to the year selector
            if (YearSelector.Items.Count == 0)
            {
                foreach (var year in WebControls.GetYearsForDropDown())
                {
                    YearSelector.Items.Add(new ListItem(year.ToString(), year.ToString()));
                }
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("Enroll"))
            {
                EnrollInSection();
            }
            else if (eventArgument.Equals("ViewSections"))
            {
                LoadSectionDisplay();
            }
            else
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid action!");
            }
        }

        private void EnrollInSection()
        {
            try
            {
                if (!Validation.ValidateSection(SectionSelector.SelectedValue, out string error))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                    return;
                }
                // Validate the semester
                if (!Validation.ValidateSemester(SemesterSelector.SelectedValue, out error))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                    return;
                }
                // Validate the year
                if (!Validation.ValidateYear(YearSelector.SelectedValue, out error))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                    return;
                }
                // Attempt to enroll the student in the section
                int sectionID = int.Parse(SectionSelector.SelectedValue);
                string univID = Session["UnivID"].ToString();
                if (UserManager.InstanceOf().EnrollInSection(univID, sectionID,
                        new Tuple<string, int>(SemesterSelector.SelectedValue,
                        int.Parse(YearSelector.SelectedValue)), false))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.GREEN, "You have successfully enrolled in the section!");
                }
                else
                {
                    // Otherwise display an error
                    WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue enrolling you in the course, you " +
                        "have not been enrolled!");
                }

            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
            SectionSelector.Items.Clear();
        }

        private void LoadSectionDisplay()
        {
            // Validate the course
            if (!Validation.ValidateCourse(CourseSelector.SelectedValue, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            // Validate the semester
            if (!Validation.ValidateSemester(SemesterSelector.SelectedValue, out error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            // Validate the year
            if (!Validation.ValidateYear(YearSelector.SelectedValue, out error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return;
            }
            try
            {
                // Empty the views
                SectionsView.Rows.Clear();
                SectionSelector.Items.Clear();
                // Get the new sections
                List<Section> sections = CourseManager.InstanceOf().GetSections(
                    int.Parse(CourseSelector.SelectedValue),
                    new Tuple<string, int>(SemesterSelector.SelectedValue,
                    int.Parse(YearSelector.SelectedValue)));
                if (sections.Count > 0)
                {
                    // Load the course sections into the table
                    foreach (var row in WebControls.CreateScheduleRows(sections))
                    {
                        SectionsView.Rows.Add(row);
                    }
                    // Load them into the radio button group
                    foreach (var section in sections)
                    {
                        SectionSelector.Items.Add(new ListItem(section.ToString(), section.SectionID.ToString()));
                    }
                    EnrollPanel.Visible = true;
                }
                else
                {
                    WebControls.SetLabel(StatusLabel, WebControls.DARK_ORANGE, "Info: The course you have selected is not offered for the " +
                        $"{UserManager.InstanceOf().GetUser(Session["UnivID"].ToString()).ToString()} semester!");
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }
    }
}