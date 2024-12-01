using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace A5
{
    public class Global : HttpApplication
    {
        static String username;
        static int role;
        public static void set_user(string username, int role)
        {
            Global.username = username;
            Global.role = role;

            // Create a cookie to persist user data
            HttpCookie userCookie = new HttpCookie("UserProfile");
            userCookie["Username"] = username;
            userCookie["Role"] = role.ToString();
            userCookie.Expires = DateTime.Now.AddMinutes(30); // Expiration time for the cookie
            HttpContext.Current.Response.Cookies.Add(userCookie);
        }

        public static string get_user()
        {
            return Global.username;
        }
        public static int get_role()
        {
            return Global.role;
        }

        public static void logout()
        {
            Global.username = null;
            Global.role = 0;

            // Clear the cookie
            if (HttpContext.Current.Request.Cookies["UserProfile"] != null)
            {
                HttpCookie userCookie = new HttpCookie("UserProfile");
                userCookie.Expires = DateTime.Now.AddDays(-1); // Expire the cookie immediately
                HttpContext.Current.Response.Cookies.Add(userCookie);
            }
        }

        void Application_Start(object sender, EventArgs e)
        {
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Initialize the session and check for a valid cookie
            if (HttpContext.Current.Request.Cookies["UserProfile"] != null)
            {
                HttpCookie userCookie = HttpContext.Current.Request.Cookies["UserProfile"];
                username = userCookie["Username"];
                role = int.Parse(userCookie["Role"]);
            }
            else
            {
                logout(); // Ensure a clean session if no cookie exists
            }
        }
    }
}