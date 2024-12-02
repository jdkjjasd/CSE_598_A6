using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

            //LoginControl.Instance.addAccount("staff", "staff", 1); // example of adding a staff account
            if (!IsPostBack)
            {
                BindAccountGrid();
            }


        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        private void BindAccountGrid()
        {
            var accountList = LoginControl.Instance.accounts.Values.Select(account => new
            {
                username = account.username,
                enc_password = account.enc_password,
                role = account.role == 1 ? "Staff" : account.role == 2 ? "Member" : "Unknown",
                memoCount = account.Memos?.Count ?? 0 
            }).ToList();

            gvAccounts.DataSource = accountList;
            gvAccounts.DataBind();
        }
        protected void gvAccounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string username = gvAccounts.DataKeys[e.RowIndex].Value.ToString();
            int result = LoginControl.Instance.removeAccount(username);

            if (result == 1)
            {
                lblMessage.Text = $"Account '{username}' deleted successfully.";
            }
            else
            {
                lblMessage.Text = $"Account '{username}' not found.";
            }

            BindAccountGrid();

        }

        protected void gvAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ClearMemos")
            {
                string username = e.CommandArgument.ToString();

                LoginControl.Instance.RemoveMemo(username, -1);

                lblMessage.Text = $"All memos for user '{username}' have been cleared.";

                BindAccountGrid();
            }
        }


        protected void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text;
            string reEnterPassword = txtReEnterPassword.Text;
            int role = int.Parse(ddlRole.SelectedValue);

            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(reEnterPassword))
            {
                lblMessage.Text = "All fields are required.";
                return;
            }

            if (password != reEnterPassword)
            {
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            if (role == 0)
            {
                lblMessage.Text = "Please select a valid role.";
                return;
            }

            // addAccount
            int result = LoginControl.Instance.addAccount(username, password, role);
            if (result == 1)
            {
                lblMessage.Text = "Account added successfully.";
                
                txtNewUsername.Text = string.Empty;
                txtNewPassword.Text = string.Empty;
                txtReEnterPassword.Text = string.Empty;
                ddlRole.SelectedIndex = 0;

                
                BindAccountGrid();
            }
            else
            {
                lblMessage.Text = "Username already exists.";
            }
        }










    }
}
