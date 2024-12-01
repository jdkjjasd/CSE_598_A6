using System;
using System.Web;
using System.Web.UI;

namespace A5
{
    public partial class Staff : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!User.Identity.IsAuthenticated)
            if (Global.get_role() == 0)
            {
                Response.Redirect("~/Login.aspx"); 
            }
            //else if (new LoginControl().GetUserRole(User.Identity.Name) == 1)
            else if (Global.get_role() == 1)
            {
                //lblStaffName.Text = User.Identity.Name;
                lblStaffName.Text = Global.get_user();
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
