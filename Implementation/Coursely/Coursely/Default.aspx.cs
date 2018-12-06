using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Coursely
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] != null)
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }

            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Home";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Default.aspx";

            LinkButton accountlink = Master.FindControl("AccountLink") as LinkButton;
            accountlink.Visible = false;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Login";
            loglink.Click += OnLogButtonClicked;
        }

        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            Response.Redirect("~/Content/Web/Login.aspx");
        }
    }
}