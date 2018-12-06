using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class Login : System.Web.UI.Page, IPostBackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] != null)
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }

            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Login";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Default.aspx";

            LinkButton accountlink = Master.FindControl("AccountLink") as LinkButton;
            accountlink.Visible = false;
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            try
            {
                // Attempt to login the user
                if (UserManager.InstanceOf().Authenticate(TextID.Text, TextPassword.Text, false))
                {
                    Session["UnivID"] = TextID.Text;
                    Session["Role"] = UserManager.InstanceOf().GetRole(TextID.Text);
                    Response.Redirect("~/Content/Web/Home.aspx");
                }
                else
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: You have entered an incorrect username or password!");
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }
    }
}