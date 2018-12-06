using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class UserAdmin : System.Web.UI.Page, IPostBackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // User must be logged in to see this page
            if (Session["UnivID"] == null || Session["Role"] == null ||
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }
            // The user must be an administrator
            if (!Session["Role"].ToString().Equals(Classes.User.ADMINISTRATOR))
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }
            
            // Setup page information
            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Admin - Manage Users";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            if (CreateAdvisor.Items.Count == 0)
            {
                foreach (var school in UniversityManager.InstanceOf().GetSchools())
                {
                    foreach (var department in UniversityManager.InstanceOf().GetDepartments(school.SchoolID))
                    {
                        // Load in the instructors
                        foreach (var instructor in UserManager.InstanceOf().GetDepartmentInstructors(
                            department.DepartmentID))
                        {
                            // Make sure to only load them once since they can be listed more than once
                            ListItem item = new ListItem($"{instructor.LastName}, {instructor.FirstName}",
                                instructor.UnivID);
                            if (!CreateAdvisor.Items.Contains(item)) {
                                CreateAdvisor.Items.Add(item);
                            }
                        }
                        // Load in majors
                        foreach (var major in UniversityManager.InstanceOf().GetMajors(department.DepartmentID))
                        {
                            CreateMajor.Items.Add(new ListItem(major.Name, major.MajorID.ToString()));
                        }
                        // Load in departments
                        CreateDepartment.Items.Add(new ListItem(department.Name, department.DepartmentID.ToString()));
                    }
                }
            }
        }

        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Content/Web/Login.aspx");
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("CreateUser"))
            {
                CreateUser();
            }
            else
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid action specified!");
            }
        }

        protected void CreateUser()
        {
            // Make sure a valid id was entered
            if (!Validation.ValidateUnivID(CreateID.Text, out string errorMessage))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, errorMessage);
                return;
            }
            // Make sure a valid name was entered
            if (!Validation.ValidateFirstName(CreateFirstName.Text, out errorMessage))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, errorMessage);
                return;
            }
            if (!Validation.ValidateLastName(CreateLastName.Text, out errorMessage))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, errorMessage);
                return;
            }
            // Make sure a valid email was entered
            if (!Validation.ValidateEmail(CreateEmail.Text, out errorMessage))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, errorMessage);
                return;
            }
            // Make sure a valid password was entered
            if (!Validation.ValidatePassword(CreatePassword.Text, out errorMessage))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, errorMessage);
                return;
            }
            if (!Validation.ValidatePassword(CreateConfirmPassword.Text, out errorMessage))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, errorMessage);
                return;
            }
            if (!CreatePassword.Text.Equals(CreateConfirmPassword.Text))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Passwords do not match!");
                return;
            }
            // Attempt to create the user
            bool result = false;
            try
            {
                if (CreateRole.SelectedValue.Equals(Classes.User.ADMINISTRATOR))
                {
                    result = UserManager.InstanceOf().CreateUser(CreateID.Text, CreateFirstName.Text,
                        CreateLastName.Text, CreateEmail.Text, CreateRole.SelectedValue, CreatePassword.Text);
                }
                else if (CreateRole.SelectedValue.Equals(Classes.User.INSTRUCTOR))
                {
                    List<int> departments = new List<int>();
                    foreach (ListItem department in CreateDepartment.Items)
                    {
                        if (department.Selected)
                        {
                            departments.Add(int.Parse(department.Value));
                        }
                    }
                    result = UserManager.InstanceOf().CreateInstructor(CreateID.Text, CreateFirstName.Text,
                        CreateLastName.Text, CreateEmail.Text, CreateRole.SelectedValue, CreatePassword.Text,
                        departments);
                }
                else if (CreateRole.SelectedValue.Equals(Classes.User.STUDENT))
                {
                    List<string> advisors = WebControls.GetSelectedItems(CreateAdvisor).ConvertAll(advisor => advisor.Value);
                    if (advisors.Count == 0)
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Unable to create student, no advisor(s) selected!");
                        return;
                    }
                    List<int> majors = WebControls.GetSelectedItems(CreateMajor).ConvertAll(major => int.Parse(major.Value));
                    if (majors.Count == 0)
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Unable to create student, no major(s) selected!");
                        return;
                    }
                    if (advisors.Count != majors.Count)
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Unable to create student, student must have one advisor for each major!");
                        return;
                    }
                    result = UserManager.InstanceOf().CreateStudent(CreateID.Text, CreateFirstName.Text,
                        CreateLastName.Text, CreateEmail.Text, CreateRole.SelectedValue, CreatePassword.Text,
                        advisors, majors);
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
            if (result)
            {
                WebControls.SetLabel(StatusLabel, WebControls.GREEN, $"User: {CreateID.Text} was created successfully!");
            }
        }
    }
}