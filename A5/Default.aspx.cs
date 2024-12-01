using System;
using System.Collections.Generic;
//using System.ServiceModel.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace A5
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Global.get_user() != null)
            {
                // If user is logged in, display logout button
                btnLogout.Visible = true;
            }
            else
            {
                btnLogout.Visible = false;
            }
            lblWelcome.Text = getWelcomeMessage();

            // Logic for page load
        }

        public static string getWelcomeMessage()
        {
            string message = "";
            if (Global.get_user() != null)
            {
                // If user is logged in, display logout button
                //btnLogout.Visible = true;
                //string username = User.Identity.Name;
                string username = Global.get_user();
                int role = Global.get_role();
                string rolemessage = role == 1 ? "Staff" : "Member";
                message = username + " (" + rolemessage + ")";

            }
            else
            {
                // If user is not logged in, display login button
                message = "Not login";
                //btnLogout.Visible = false;
            }

            return message;
        }

        protected void btnMember_Click(object sender, EventArgs e)
        {
            // Redirect to member page, check if user is logged in
            //if (User.Identity.IsAuthenticated)
            if (Global.get_role() == 2)
            {
                Response.Redirect("Member.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx"); // Not logged in, redirect to login page
            }
        }


        protected void btnStaff_Click(object sender, EventArgs e)
        {
            // Redirect to staff page, check if user is logged in and has staff role
            //if (User.Identity.IsAuthenticated)
            if (Global.get_role() == 1)
            {
                Response.Redirect("Staff.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx"); // Not logged in, redirect to login page
            }
        }

        protected void btnTryLogin_Click(object sender, EventArgs e)
        {
            // Example: Redirect to login test page
            Response.Redirect("LoginTest.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //LoginControl loginControl = (LoginControl)LoadControl("~/LoginControl.ascx");
            Global.logout();
            Response.Redirect("~/Default.aspx", false);
            //loginControl.logOut();
        }

        protected void btnTryit_Click(object sender, EventArgs e)
        {
            // Example: Redirect to try it page
            Response.Redirect("TryIt.aspx");
        }
    }
}
