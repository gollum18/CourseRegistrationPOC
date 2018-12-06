using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class Schedule : System.Web.UI.Page
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
            title.Text = "Coursely - Schedule";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            // Load in the years to the year selector
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year <= currentYear + 2; year++)
            {
                YearDropDown.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }

        /// <summary>
        /// Displays the user's schedule on page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnViewScheduleClicked(object sender, EventArgs e)
        {
            ScheduleView.Rows.Clear();
            UnenrollSelector.Items.Clear();
            try
            {
                // Get the students schedule
                List<Section> schedule = UserManager.InstanceOf().GetSchedule(
                    // The term
                    new Tuple<string, int>(SemesterDropDown.Value, int.Parse(YearDropDown.SelectedValue)),
                    // The students university identifier
                    Session["UnivID"].ToString(),
                    // The student's role
                    Session["Role"].ToString().Equals(Classes.User.STUDENT) ? true : false);
                // Fill in the schedule table
                ScheduleView.Rows.AddRange(WebControls.CreateScheduleRows(schedule).ToArray());
                // Fill in the unenroll list
                foreach (var section in schedule)
                {
                    UnenrollSelector.Items.Add(new ListItem(section.ToString(), 
                        section.SectionID.ToString()));
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

        protected void OnUnenrollButtonClicked(object sender, EventArgs e)
        {
            // Make sure a section was selected
            if (WebControls.GetSelectedItems(UnenrollSelector).Count == 0)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: You must select a section to unenroll from!");
                return;
            }
            try
            {
                if (!int.TryParse(UnenrollSelector.SelectedValue, out int sectionID))
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid section selected!");
                }
                string univID = Session["UnivID"].ToString();
                if (UserManager.InstanceOf().UnenrollFromSection(univID, sectionID))
                {
                    UnenrollSelector.Items.Clear();
                    WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Successfully unenrolled from section");
                    if (!StatusLabel.Text.Equals(""))
                    {
                        StatusLabel.Text = "";
                    }
                }
                else
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: There was an issue removing you from the section, " +
                        "you have note been removed!");
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Content/Web/Login.aspx");
        }
    }
}