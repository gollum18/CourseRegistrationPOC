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
    public partial class ManageSections : System.Web.UI.Page, IPostBackEventHandler
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
            title.Text = "Coursely - Instructor Sections";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            // Load in the years for the year selector
            if (YearDD.Items.Count == 0)
            {
                foreach (var year in WebControls.GetYearsForDropDown())
                {
                    YearDD.Items.Add(new ListItem(year.ToString(), year.ToString()));
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

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("ViewSections"))
            {
                ViewSections();
            }
            else
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid action specified!");
            }
        }

        private void ViewSections()
        {
            // TODO: Validate the semester and year
            try
            {
                Tuple<string, int> term = new Tuple<string, int>(SemesterDD.SelectedValue, 
                    int.Parse(YearDD.SelectedValue));
                List<Section> sections = UserManager.InstanceOf().GetSchedule(term, Session["UnivID"].ToString(), false);
                if (sections.Count > 0)
                {
                    ScheduleView.Rows.Clear();
                    foreach (var row in WebControls.CreateScheduleRows(sections))
                    {
                        ScheduleView.Rows.Add(row);
                    }
                    if (!StatusLabel.Text.Equals(""))
                    {
                        StatusLabel.Text = "";
                    }
                }
                else
                {
                    WebControls.SetLabel(StatusLabel, WebControls.DARK_ORANGE, "You are not teaching any courses for the " +
                        $"{term.Item1} {term.Item2} semester!");
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }
    }
}