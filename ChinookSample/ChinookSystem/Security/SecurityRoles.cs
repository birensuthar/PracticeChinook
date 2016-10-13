using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Security
{
    internal static class SecurityRoles
    {
        public const string WebSiteAdmins = "WebsiteAdmins";
        public const string RegisteredUsers = "RegisteredUsers";
        public const string Staff = "Staff";
        public const string Auditor = "Auditor";


        //readonly
        public static List<string> StartupSecurityRoles
        {
            get
            {
                List<string> roleList = new List<string>();
                roleList.Add(WebSiteAdmins);
                roleList.Add(RegisteredUsers);
                roleList.Add(Staff);
                roleList.Add(Auditor);
                return roleList;
            }
        }
    }
}
