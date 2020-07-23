using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace TrackTraceService.Classes
{
    public class Sqlhelper
    {
        
        public static void GetConnectionString()
        {
            SqlDataLayer.strLocal = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
            
        }
    }
}