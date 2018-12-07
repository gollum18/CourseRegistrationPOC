using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

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

            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Student Record";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            if (StudentRecord.Rows.Count == 0)
            {
                try
                {
                    StudentRecord.Rows.Clear();
                    List<string> record = UserManager.InstanceOf().GetAcademicRecord(
                        Session["UnivID"].ToString());
                    if (record.Count > 0) {
                        foreach (var row in WebControls.GenerateRecordRows(
                            UserManager.InstanceOf().GetUser(Session["UnivID"].ToString()).ToString(), record))
                        {
                            StudentRecord.Rows.Add(row);
                        }
                        if (!StatusLabel.Text.Equals(""))
                        {
                            StatusLabel.Text = "";
                        }
                    }
                    else
                    {
                        WebControls.SetLabel(StatusLabel, WebControls.DARK_ORANGE, "Info: You do not have any grades to display!");
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
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