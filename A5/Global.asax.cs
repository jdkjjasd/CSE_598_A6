using System;
using System.Collections.Generic;
using System.Data;
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
        //static String username;
        //static int role;
        static HttpCookie userCookie;

        public Global()
        {
            userCookie = new HttpCookie("UserProfile");
        }


        public static void set_user(string username, int role)
        {
            //Global.username = username;
            //Global.role = role;

            //HttpCookie userCookie = new HttpCookie("UserProfile");
            userCookie["Username"] = username;
            userCookie["Role"] = role.ToString();
            userCookie.Expires = DateTime.Now.AddMinutes(30); // Expiration time for the cookie
            HttpContext.Current.Response.Cookies.Add(userCookie);
        }

        public static string get_user()
        {
            return userCookie["Username"];
        }
        public static int get_role()
        {
            try 
            {
                return int.Parse(userCookie["Role"]);
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public static void logout()
        {
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
            }
            else
            {
                logout(); // Ensure a clean session if no cookie exists
            }
        }
    }
}