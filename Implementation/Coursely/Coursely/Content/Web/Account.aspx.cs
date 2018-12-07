using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class Account : System.Web.UI.Page, IPostBackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnivID"] == null || Session["Role"] == null ||
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
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

            // Get the user and load up their information on the page
            User user = UserManager.InstanceOf().GetUser(Session["UnivID"].ToString());

            // Load in the user's information
            UnivIDTextBox.Value = user.UnivID;
            NameTextBox.Value = $"{user.LastName}, {user.FirstName}";
            EmailTextBox.Text = user.Email;
            RoleTextBox.Value = user.Role;

            // Reset the error label
            StatusLabel.Text = "";
        }

        public void UpdatePassword()
        {
            try
            {
                if (NewPasswordTextBox.Text.Equals(ConfirmPasswordTextBox.Text))
                {
                    if (Validation.ValidatePassword(NewPasswordTextBox.Text))
                    {
                        if (UserManager.InstanceOf().ChangePassword(UnivIDTextBox.Value, CurrentPasswordTextBox.Text, NewPasswordTextBox.Text))
                        {
                            WebControls.SetLabel(StatusLabel, Color.Green, "Password changed successfully!");
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, Color.Red, "There was an issue changing your password, please try again!");
                        }
                    }
                    else
                    {
                        throw new Exception("Error: New password must be between 8 and 64 characters in length!");
                    }
                }
                else
                {
                    throw new Exception("Error: New password and confirmation password do not match.");
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        public void UpdateEmail()
        {
            try
            {
                if (UserManager.InstanceOf().GetEmail(UnivIDTextBox.Value).Equals(EmailTextBox.Text))
                {
                    if (NewEmailTextBox.Text.Equals(ConfirmEmailTextBox.Text))
                    {
                        if (Validation.ValidateEmail(NewEmailTextBox.Text))
                        {
                            if (UserManager.InstanceOf().ChangeEmail(UnivIDTextBox.Value, NewEmailTextBox.Text))
                            {
                                WebControls.SetLabel(StatusLabel, Color.Green, "Email changed successfully!");
                            }
                            else
                            {
                                WebControls.SetLabel(StatusLabel, Color.Red, "There was an issue changing your email, please try again!");
                            }
                        }
                        else
                        {
                            throw new Exception("Error: New email must be between 8 and 64 characters!");
                        }
                    }
                    else
                    {
                        throw new Exception("Error: New email and confirmation email do not match!");
                    }
                }
                else
                {
                    throw new Exception("Error: Current email is incorrect!");
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("UpdatePassword"))
            {
                UpdatePassword();
            }
            else if (eventArgument.Equals("UpdateEmail"))
            {
                UpdateEmail();  
            }
            else
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: Invalid action specified!");
            }
        }

        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }
    }
}