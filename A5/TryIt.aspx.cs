using System;
using System.Web.UI;

namespace A5
{
    public partial class TryIt : Page
    {
        private LoginControl loginControl;

        public TryIt()
        {
            loginControl = LoginControl.Instance;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //loginControl = new LoginControl();
            if (!IsPostBack)
            {
                loginControl.Page_Init();
                GenerateNewRandomString();
            }
            lblWelcome.Text = Default.getWelcomeMessage();
        }

        protected void btnNewString_Click(object sender, EventArgs e)
        {
            GenerateNewRandomString();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string inputRString = txtRString.Text;
            string targetRString = Session["RandomString"] as string;
            //loginControl = LoginControl.Instance;
            //loginControl.Page_Init();

            if (!loginControl.ValidateCaptcha(inputRString,targetRString))
            {
                lblMessage.Text = "Invalid random string.";
                return;
            }

            int role = loginControl.Login(username, password);

            if (role == 0)
            {
                lblMessage.Text = "Invalid username or password.";
            }
            else
            {
                lblMessage.Text = "Login successful!";
                LoginControl.RedirectUser(role,Response);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //loginControl = new LoginControl();
            loginControl.Page_Init();
            loginControl.logOut();
            Response.Redirect(Request.RawUrl);
        }

        private void GenerateNewRandomString()
        {
            //loginControl = new LoginControl();
            loginControl.Page_Init();
            string rstring = loginControl.newString();
            Session["RandomString"] = rstring;
            lblRString.Text = Session["RandomString"].ToString();
        }
    }
}
