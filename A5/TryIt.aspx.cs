using System;
using System.Web.UI;

namespace A5
{
    public partial class TryIt : Page
    {
        private LoginControl loginControl;

        protected void Page_Load(object sender, EventArgs e)
        {
            loginControl = new LoginControl();
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
            loginControl = new LoginControl();
            loginControl.Page_Init();

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
            loginControl = new LoginControl();
            loginControl.Page_Init();
            loginControl.logOut();
            Response.Redirect(Request.RawUrl);
        }

        private void GenerateNewRandomString()
        {
            loginControl = new LoginControl();
            loginControl.Page_Init();
            string rstring = loginControl.newString();
            Session["RandomString"] = rstring;
            lblRString.Text = Session["RandomString"].ToString();
        }

        protected void btnFetchWeather_Click(object sender, EventArgs e)
        {
            try
            {
                string zipCode = txtZipCode.Text.Trim();
                if (string.IsNullOrEmpty(zipCode))
                {
                    lblWeatherForecast.Text = "Please enter a valid ZIP Code.";
                    return;
                }

                // Call the Weather5day method
                WeatherService weatherService = new WeatherService();
                string[] forecast = weatherService.Weather5day(zipCode);

                // Display the forecast
                lblWeatherForecast.Text = string.Join("<br />", forecast);
            }
            catch (Exception ex)
            {
                lblWeatherForecast.Text = "Error: " + ex.Message;
            }
        }
    }
}
