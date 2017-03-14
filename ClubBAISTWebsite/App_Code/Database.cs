using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ClubBAISTWebsite
{
    public class Database
    {
        public class ConnStrings //Will need to add one more string here and in the connstring.config for PROD when doing transition
        {
            public static string local = ConfigurationManager.ConnectionStrings["local"].ConnectionString;
        }
    }
}