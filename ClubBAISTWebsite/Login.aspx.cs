using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace ClubBAISTWebsite
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void loginControl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            ClubBaistSystem system = new ClubBaistSystem();
            bool bLoginSuccess = false;

            bLoginSuccess = system.LoginUser(loginControl.UserName,loginControl.Password);
            if (bLoginSuccess)
            {
                FormsAuthentication.RedirectFromLoginPage(loginControl.UserName, loginControl.RememberMeSet);
            }
        }
    }
}