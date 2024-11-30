using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;


namespace A5
{
    //public class User
    //{
    //    public string username { get; }
    //    public string password { get; }
    //    public int role { get; }
    //    public User(string username, string password, int role)
    //    {
    //        this.username = username;
    //        this.password = password;
    //        this.role = role;
    //    }
    //}
    public partial class LoginControl : System.Web.UI.UserControl
    {
        private RandoomStringService.ServiceClient rsclient;
        private string rstring;

        Dictionary<string, List<string>> users;

        public void Page_Init()
        {
            if (rsclient == null)
            {
                rsclient = new RandoomStringService.ServiceClient();
            }
            users = new Dictionary<string, List<string>>();
            users["admin"] = new List<string> { "123", "1" };
            users["user"] = new List<string> { "123", "2" };
            users["TA"] = new List<string> { "Cse445", "1" };
            users["member"] = new List<string> { "123", "2" };

            // Check if user.xml exists
            if (!File.Exists(Server.MapPath("user.xml")))
            {
                // Create user.xml
                File.Create(Server.MapPath("user.xml")).Close();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page_Init();
            // Page load logic
            if (!IsPostBack)
            {
                rstring = rsclient.GetRandomString("5");

                Session["RandomString"] = rstring;
                lblRString.Text = rstring;
            }


        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rsclient != null)
            {
                rsclient.Close();
            }
        }

        protected void btnNewString_Click(object sender, EventArgs e)
        {
            rstring = newString();
            lblRString.Text = rstring;
            Session["RandomString"] = rstring;
        }

        public string newString()
        {
            if (rsclient == null)
            {
                rsclient = new RandoomStringService.ServiceClient();
            }
            string rstr = rsclient.GetRandomString("5");

            return rstr;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string inputRString = txtRString.Text;
            string targetRString = Session["RandomString"] as string;

            if (!ValidateCaptcha(inputRString, targetRString))
            {
                lblMessage.Text = "Invalid random string.";
                return;
            }

            int role = Login(username, password);

            if (role == 0)
            {
                lblMessage.Text = "Invalid username or password.";
            }
            else
            {
                lblMessage.Text = "Login successful!";
                RedirectUser(role, Response);
            }
        }

        public int Login(string username, string password)
        {
            int role = 0;
            if (ValidateUser(username, password))
            {
                role = GetUserRole(username);
                Global.set_user(username, role);
            }
            return role;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            logOut();
        }

        public void logOut()
        {
            // Clear authentication ticket and set user as logged out
            //FormsAuthentication.SignOut();
            Global.logout();
            // Redirect to login page
            //Response.Redirect("~/Login.aspx");
        }

        public bool ValidateCaptcha(string response, string targetstring)
        {
            //string targetstring = Session["RandomString"] as string;
            if (targetstring != response)
            {
                return false;
            }
            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                List<string> user = users[username];
                if (user[0] == password)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetUserRole(string username) //get by name
        {
            if (users.ContainsKey(username))
            {
                List<string> user = users[username];
                return int.Parse(user[1]);
            }
            return 0; // Return 0 if user not found
        }



        //public int GetUserRole(User user) //get by User
        //{
        //    return user.role;
        //}

        public static void RedirectUser(int role, HttpResponse response)
        {
            switch (role)
            {
                case 1:
                    response.Redirect("~/Staff.aspx", false);
                    break;
                case 2:
                    response.Redirect("~/Member.aspx", false);
                    break;
                default:
                    response.Redirect("~/Default.aspx", false);
                    break;
            }
        }

    }
}

