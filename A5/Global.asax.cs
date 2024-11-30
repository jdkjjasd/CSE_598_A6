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
        }

        void Application_Start(object sender, EventArgs e)
        {
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Call logout here if needed
            logout();
        }
    }
}