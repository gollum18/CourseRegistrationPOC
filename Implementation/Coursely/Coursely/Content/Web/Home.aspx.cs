using System;
using System.Web.UI.WebControls;

using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] == null || Session["Role"] == null ||  
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            string role = Session["Role"].ToString();
            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            Label header = Master.FindControl("LblTitle") as Label;
            // Admin
            if (role.Equals(Classes.User.ADMINISTRATOR))
            {
                MenuAdmin.Visible = true;
                header.Text = "Coursely - Admin Home";
            }
            // Instructor
            else if (role.Equals(Classes.User.INSTRUCTOR))
            {
                MenuInstructor.Visible = true;
                header.Text = "Coursely - Instructor Home";
            }
            // Student
            else if (role.Equals(Classes.User.STUDENT))
            {
                MenuStudent.Visible = true;
                header.Text = "Coursely - Admin Home";
            }
        }

        private void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Content/Web/Login.aspx");
        }
    }
}