using System;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class Grades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] == null || Session["Role"] == null ||
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }
            if (Session["Role"].ToString().Equals(Classes.User.ADMINISTRATOR))
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }
            
            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            if (StudentRecordList.Items.Count == 0)
            {
                try
                {
                    foreach (var record in UserManager.InstanceOf().GetAcademicRecord(
                        Session["UnivID"].ToString()))
                    {
                        StudentRecordList.Items.Add(new ListItem(record));
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

            if (string.IsNullOrEmpty(RecordHeader.Text))
            {
                RecordHeader.Text = $"Display record for {UserManager.InstanceOf().GetUser(Session["UnivID"].ToString()).ToString()}";
            }
        }

        private void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }
    }
}