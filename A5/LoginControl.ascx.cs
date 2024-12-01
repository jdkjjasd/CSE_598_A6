using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using System.Xml.Serialization;
using WebGrease.Extensions;


namespace A5
{
    [Serializable]
    public class Account
    {
        public string username { get; set; }
        public string enc_password { get; set; }
        public int role { get; set; }
        public Account() 
        {
        }
        public Account(string username, string enc_password, int role)
        {
            this.username = username;
            this.enc_password = enc_password;
            this.role = role;
        }
    }

    [Serializable]
    public class AccountEntry
    {
        [XmlAttribute("Username")]
        public string Username { get; set; }

        [XmlElement("Account")]
        public Account Account { get; set; }
    }

    [Serializable]
    [XmlRoot("Accounts")]
    public class AccountList
    {
        [XmlElement("AccountEntry")]
        public List<AccountEntry> AccountEntries { get; set; }

        [XmlIgnore]
        public Dictionary<string, Account> AccountsDictionary
        {
            get
            {
                return AccountEntries?.ToDictionary(entry => entry.Username, entry => entry.Account);
            }
            set
            {
                AccountEntries = value?.Select(kvp => new AccountEntry { Username = kvp.Key, Account = kvp.Value }).ToList();
            }
        }
    }

    public partial class LoginControl : System.Web.UI.UserControl
    {
        private static LoginControl _instance; // singleton instance
        private static readonly object _lock = new object(); // thread lock object

        private RandoomStringService.ServiceClient rsclient;
        private string rstring;

        private string logFilePath;
        private string accountFilePath;

        AccountList accountList;


        //Dictionary<string, List<string>> users;
        public Dictionary<string, Account> accounts;

        public LoginControl()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
            accountFilePath = root + "Accounts.xml";
            logFilePath = root + "log.log";
            rsclient = new RandoomStringService.ServiceClient();
            if (!File.Exists(accountFilePath))
            {
                // Create user.xml
                File.Create(accountFilePath).Close();
                Init_User_Xml();
            }
            Read_User_Xml();
        }

        public void Page_Init()
        {
            //accountFilePath = Server.MapPath("Accounts.xml");
            //logFilePath = Server.MapPath("log.log");
            if (rsclient == null)
            {
                rsclient = new RandoomStringService.ServiceClient();
            }
            //users = new Dictionary<string, List<string>>();
            //users["admin"] = new List<string> { "123", "1" };
            //users["user"] = new List<string> { "123", "2" };
            //users["TA"] = new List<string> { "Cse445", "1" };
            //users["member"] = new List<string> { "123", "2" };

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
                lblRString1.Text = rstring;
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
            lblRString1.Text = rstring;
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

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtSignUpUsername.Text.Trim();
            string password = txtSignUpPassword.Text;
            string rePassword = txtSignUpRePassword.Text;
            string inputRString = txtRString1.Text;
            string targetRString = Session["RandomString"] as string;

            if (!ValidateCaptcha(inputRString, targetRString))
            {
                lblSignUpMessage.Text = "Invalid random string.";
                lblSignUpMessage.ForeColor = System.Drawing.Color.Red;

                return;
            }


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword))
            {
                lblSignUpMessage.Text = "All fields are required.";
                lblSignUpMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (password != rePassword)
            {
                lblSignUpMessage.Text = "Passwords do not match.";
                lblSignUpMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int result = addAccount(username, password, 2);

            if (result == 1)
            {
                lblSignUpMessage.Text = "Sign up successful!";
                lblSignUpMessage.ForeColor = System.Drawing.Color.Green;

                txtSignUpUsername.Text = string.Empty;
                txtSignUpPassword.Text = string.Empty;
                txtSignUpRePassword.Text = string.Empty;
                txtRString1.Text = string.Empty;
            }
            else
            {
                lblSignUpMessage.Text = "Username already exists.";
                lblSignUpMessage.ForeColor = System.Drawing.Color.Red;
            }
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
            Session["RandomString"] = rsclient.GetRandomString("5");
            //string targetstring = Session["RandomString"] as string;
            if (targetstring != response)
            {
                return false;
            }
            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            if (accounts.ContainsKey(username))
            {
                string enc = PasswordEncrypt(password);
                Account user = accounts[username];
                if (user.enc_password == enc)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// get user role by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>1 for staff,2 for member, 0 for error</returns>
        public int GetUserRole(string username) //get by name
        {
            if (accounts.ContainsKey(username))
            {
                Account user = accounts[username];
                return user.role;
            }
            return 0; // Return 0 if user not found
        }

        private void Init_User_Xml()
        {
            this.accountList = new AccountList
            {
                AccountsDictionary = new Dictionary<string, Account>
                {
                    { "admin", new Account("admin", "123", 1) },
                    { "user", new Account("user", "123", 2) },
                    { "TA", new Account("TA", "Cse445", 1) },
                    { "member", new Account("member", "123", 2) }
                }
            };
            Save_User_Xml(accountList, accountFilePath);
        }

        public static void Save_User_Xml(AccountList accountList,string path )
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AccountList));
            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, accountList);
            }
        }

        /// <summary>
        /// Add account to the account list
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns>1 for succssed, 0 for known user</returns>
        public int addAccount(string username, string password, int role)
        {
            
            if (accounts.ContainsKey(username))
            {
                return 0;
            }
            Account account = new Account(username, PasswordEncrypt(password), role);
            accountList.AccountEntries.Add(new AccountEntry { Username = username, Account = account });
            Save_User_Xml(accountList, accountFilePath);
            Read_User_Xml();

            //foreach (var kvp in accountList.AccountsDictionary)
            //{
            //    using (StreamWriter writer = new StreamWriter(logFilePath, true))
            //    {
            //        Console.SetOut(writer);
            //        Console.WriteLine("new dict.");
            //        Console.WriteLine($"Username: {kvp.Key}, Role: {kvp.Value.role}");
            //        writer.Flush();
            //    }
            //}

            return 1;
        }

        /// <summary>
        /// delete account from the account list
        /// </summary>
        /// <param name="username"></param>
        /// <returns>1 for succssed, 0 for unknown user</returns>
        public int removeAccount(string username)
        {
            if (accounts.ContainsKey(username))
            {
                accountList.AccountEntries.RemoveAll(entry => entry.Username == username);
                Save_User_Xml(accountList, accountFilePath);
                Read_User_Xml();
                return 1;
            }
            return 0;
        }

        private void Read_User_Xml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AccountList));
            using (TextReader reader = new StreamReader(accountFilePath))
            {
                this.accountList = (AccountList)serializer.Deserialize(reader);
                accounts = accountList.AccountsDictionary;

                //foreach (var kvp in accounts)
                //{
                //    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                //    {
                //        Console.SetOut(writer);
                //        Console.WriteLine("This is a log message." + AppDomain.CurrentDomain.BaseDirectory);
                //        Console.WriteLine($"Username: {kvp.Key}, Role: {kvp.Value.role}");
                //        writer.Flush();
                //    }
                //}
            }
        }

        //public int GetUserRole(User user) //get by User
        //{
        //    return user.role;
        //}
        public static string PasswordEncrypt(string password)
        {
            //byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            //data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            //String hash = System.Text.Encoding.ASCII.GetString(data);
            string hash = password;
            return hash;
        }

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

        public static LoginControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoginControl();
                        }
                    }
                }
                return _instance;
            }
        }


    }
}

