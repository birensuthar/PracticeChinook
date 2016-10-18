using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Security
{
    public class UserProfile
    {
        public string UserId { get; set; }      //AspNetUser
        public string UserName { get; set; }    //AspNetUser
        public int? EmployeeId { get; set; }    //AspNetUser
        public int? CustomerId { get; set; }    //AspNetUser
        public string FirstName { get; set; }   //Employee or Customer table
        public string LastName { get; set; }    //Employee or Customer table
        public string Email { get; set; }       //AspNetUser table
        public string EmailConfirmed { get; set; }   //AspNetUser table
        public IEnumerable<string> RoleMemberships { get; set; }
    }
}
