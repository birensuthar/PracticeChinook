using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Microsoft.AspNet.Identity;                //UserManager
using Microsoft.AspNet.Identity.EntityFramework; //Userstore
using System.ComponentModel;                    //ODS
using ChinookSystem.DAL;                        //Context Class
#endregion

namespace ChinookSystem.Security
{
    [DataObject]
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
        }

        //Setting up the default webMaster
        #region Constants
        private const string STR_DEFAULT_PASSWORD = "Pa$$word1";
        private const string STR_USERNAME_FORMAT = "{0}.{1}";
        private const string STR_EMAIL_FORMAT = "{0}@Chinook.ca";
        private const string STR_WEBMASTER_USERNAME = "Webmaster";
        #endregion
        public void AddWebmaster()
        {
            if (!Users.Any(u => u.UserName.Equals(STR_WEBMASTER_USERNAME)))
            {
                var webMasterAccount = new ApplicationUser()
                {
                    UserName = STR_WEBMASTER_USERNAME,
                    Email = string.Format(STR_EMAIL_FORMAT, STR_WEBMASTER_USERNAME)

                };
                //This create command is from the inherited UserManager Class
                //This command creates a record on the security Users Table. (AspNetUsers)
                this.Create(webMasterAccount, STR_DEFAULT_PASSWORD);

                //This AddToRole command is from the inherited UserManager class
                //This record creates a record on the security UserRole table (AspNetUserRoles)
                this.AddToRole(webMasterAccount.Id, SecurityRoles.WebSiteAdmins);
            }
        }//EOM

        //Create the crud methods for adding a user to the security User table.
        //Read of Data to display on GridView.
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<UnRegisteredUserProfile> ListAllUnregisteredUsers()
        {
            using (var context = new ChinookContext())
            {
                //The data needs to be in memory for execution by the next query. To accomplish this, use .ToList() which will force the
                //query to execute. 

                //List() set containing the list of employeeids
                var registeredEmployees = (from emp in Users
                                          where emp.EmployeeId.HasValue
                                          select emp.EmployeeId).ToList();
                //Compare the List() set to the user data table Employees.
                var unregisteredEmployees = (from emp in context.Employees
                                            where !registeredEmployees.Any(eid => emp.EmployeeID == eid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                CustomerEmployeeID = emp.EmployeeID,
                                                FirstName = emp.FirstName,
                                                LastName = emp.LastName,
                                                UserType = UnRegisteredUserType.Employee
                                            }).ToList();

                //IEnumerable set containing the list of customerids
                var registeredCustomers = (from cus in Users
                                          where cus.CustomerId.HasValue
                                          select cus.CustomerId).ToList();
                //Compare the IEnumerable set to the user data table Customers.
                var unregisteredCustomers = (from cus in context.Customers
                                            where !registeredCustomers.Any(cid => cus.CustomerID == cid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                CustomerEmployeeID = cus.CustomerID,
                                                FirstName = cus.FirstName,
                                                LastName = cus.LastName,
                                                UserType = UnRegisteredUserType.Customer
                                            }).ToList();
                //Combine and return the two physically identcal layout datasets
                return unregisteredEmployees.Union(unregisteredCustomers).ToList();
            }
        }//EOM
        
        //Register user to the User Table (GridView).
        public void RegisterUser(UnRegisteredUserProfile userinfo)
        {
            //The basic information needed for the security User Record:- Password, Email, UserName
            //You could randomly generate a password, we will use the default password.
            //The instance of the required user is based on our ApplicationUser.
            var newuseraccount = new ApplicationUser()
            {
                UserName = userinfo.AssignedUserName,
                Email = userinfo.AssignedEmail
            };

            //Set the customerid or the employeeid
            switch(userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
                        newuseraccount.Id = userinfo.AssignedUserName.ToString();

                        //this.AddToRole(newuseraccount.Id, SecurityRoles.RegisteredUsers);
                        //Cannot do this because the user and the ID has not been created yet in the table.

                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        newuseraccount.Id = userinfo.AssignedUserName.ToString();
                        break;
                    }
            }

            //Create the actual AspNetUser record
            this.Create(newuseraccount, STR_DEFAULT_PASSWORD);

            //Assign User to appropriate role
            switch (userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.RegisteredUsers);
                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.Staff);
                        break;
                    }
            }

        }//EOM

        //Add a user to the User Table (ListView)

        //Delete a user from the User Table (ListView)

    }//EOC
}//EON
