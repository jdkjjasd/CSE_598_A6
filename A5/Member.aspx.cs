using System;
using System.Web;
using System.Web.UI;
using A5;

namespace A5
{
    public partial class Member : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in
            //if (!User.Identity.IsAuthenticated)
            if (Global.get_role() == 0)
            {
                Response.Redirect("~/Login.aspx"); // Redirect to the login page if not logged in
            }
            //else if (new LoginControl().GetUserRole(User.Identity.Name) == 2)
            else if (Global.get_role() == 2)
            {
                //lblUsername.Text = User.Identity.Name;
                lblUsername.Text = Global.get_user();
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}
