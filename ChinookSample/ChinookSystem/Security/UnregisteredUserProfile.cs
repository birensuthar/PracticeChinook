using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Security
{
    public enum UnRegisteredUserType { Undefined, Employee, Customer}
    public class UnRegisteredUserProfile
    {
        public string UserId { get; set; } //Generated
        public string UserName { get; set; } //Collected
        public string Email { get; set; } //Collected
        public string FirstName { get; set; } //Comes from the User Table
        public string LastName { get; set; } //Comes from the User Table
        public UnRegisteredUserType UserType { get; set; }
    }
}
