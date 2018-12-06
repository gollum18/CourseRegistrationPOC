using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class Catalog : System.Web.UI.Page, IPostBackEventHandler
    {
        private ListItem DefaultCourse = new ListItem("-Course-", "default");
        private ListItem DefaultDepartment = new ListItem("-Department-", "default");
        private ListItem DefaultSchool = new ListItem("-School-", "default");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set up the title and link controls.
            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Catalog";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            if (Session["UnivID"] == null)
            {
                homelink.PostBackUrl = "~/Default.aspx";
            }
            else
            {
                homelink.PostBackUrl = "~/Content/Web/Home.aspx";
            }

            LinkButton accountlink = Master.FindControl("AccountLink") as LinkButton;
            if (Session["UnivID"] == null)
            {
                accountlink.Visible = false;
            }

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            if (Session["UnivID"] == null)
            {
                loglink.Text = "Login";
            }
            else
            {
                loglink.Text = "Logout";
            }
            loglink.Click += OnLogButtonClicked;

            // Setup the schools dropdown
            if (DropSchool.Items.Count == 0) {
                DropSchool.Items.Add(DefaultSchool);
                foreach (School school in UniversityManager.InstanceOf().GetSchools())
                {
                    DropSchool.Items.Add(new ListItem(school.Name, school.SchoolID.ToString()));
                }
            }

            StatusLabel.Text = "";
        }

        private void OnLogButtonClicked(object sender, EventArgs e)
        {
            if (Session["UnivID"] == null)
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }
            else
            {
                if (UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
                {
                    UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
                    Session.Clear();
                    Session.Abandon();
                    if (!(sender is string))
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                }
            }
        }

        /// <summary>
        /// Called when the selector for the school changes. Loads departments into the department selector 
        /// when the selected school changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedSchoolChanged(object sender, EventArgs e)
        {
            WebControls.ResetSelector(DropCourse, WebControls.DEFAULT_LISTITEM_COURSE);
            if (DropSchool.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.ResetSelector(DropDepartment, WebControls.DEFAULT_LISTITEM_DEPARTMENT);
            }
            else
            {
                try
                {
                    WebControls.ResetSelector(DropDepartment, WebControls.DEFAULT_LISTITEM_DEPARTMENT);
                    foreach (Department department in
                        UniversityManager.InstanceOf().GetDepartments(int.Parse(DropSchool.SelectedValue)))
                    {
                        DropDepartment.Items.Add(new ListItem(department.Name, department.DepartmentID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Called when the selector for the department changes. Loads up the courses into the course selector
        /// when the selected department changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDepartmentChanged(object sender, EventArgs e)
        {
            if (DropDepartment.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.ResetSelector(DropCourse, WebControls.DEFAULT_LISTITEM_COURSE);
            }
            else
            {
                try
                {
                    WebControls.ResetSelector(DropCourse, WebControls.DEFAULT_LISTITEM_COURSE);
                    foreach (Course course in
                        CourseManager.InstanceOf().GetCourses(int.Parse(DropDepartment.SelectedValue)))
                    {
                        DropCourse.Items.Add(new ListItem(course.Name, course.CourseID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        /// <summary>
        /// Called when the selector for the course changes. Loads up course information onto the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedCourseChanged(object sender, EventArgs e)
        {
            if (DropCourse.SelectedValue.Equals("default"))
            {
                CourseName.InnerHtml = "";
                CourseHeaders.InnerHtml = "";
                CourseDescription.InnerHtml = "";
                return;
            }
            else
            {
                try
                {
                    Course course = CourseManager.InstanceOf().GetCourse(int.Parse(DropCourse.SelectedValue));
                    CourseName.InnerHtml = $"{UniversityManager.InstanceOf().GetDepartment(course.DepartmentID).Abbreviation} " +
                        $"{course.Number} ({course.Credits}) - {course.Name}";
                    CourseHeaders.InnerHtml = $"Prerequisites: {GetPrerequisitesHeader(course)}";
                    CourseDescription.InnerHtml = $"{course.Description}";
                }
                catch (Exception ex) 
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        private string GetPrerequisitesHeader(Course course)
        {
            int prevGroup = -1;
            string prereqs = "";
            using (var prerequisites = course.GetPrerequisites())
            {
                while (prerequisites.MoveNext())
                {
                    if (prevGroup != -1)
                    {
                        if (prerequisites.Current.Item2 == prevGroup)
                        {
                            prereqs += " OR ";
                        }
                        else
                        {
                            prereqs += " AND ";
                        }
                    }
                    try
                    {
                        prereqs += $"{UniversityManager.InstanceOf().GetDepartment(CourseManager.InstanceOf().GetCourse(prerequisites.Current.Item1).DepartmentID).Abbreviation} " +
                            $"{CourseManager.InstanceOf().GetCourse(prerequisites.Current.Item1).Number}";
                    }
                    catch (Exception ex)
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                    }
                    prevGroup = prerequisites.Current.Item2;
                }
            }
            if (prereqs.Length == 0)
            {
                prereqs = "None";
            }
            return prereqs;
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("WindowClosed"))
            {
                OnLogButtonClicked("PageClosedEvent", null);
            }
        }
    }
}