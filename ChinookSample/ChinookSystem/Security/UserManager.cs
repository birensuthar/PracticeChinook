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
using ChinookSystem.Data.Entities;              //Entity Classes
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
                                            where !registeredEmployees.Any(eid => emp.EmployeeId == eid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                CustomerEmployeeID = emp.EmployeeId,
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
                                            where !registeredCustomers.Any(cid => cus.CustomerId == cid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                CustomerEmployeeID = cus.CustomerId,
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
                        newuseraccount.CustomerId = userinfo.CustomerEmployeeID;

                        //this.AddToRole(newuseraccount.Id, SecurityRoles.RegisteredUsers);
                        //Cannot do this because the user and the ID has not been created yet in the table.

                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        newuseraccount.EmployeeId = userinfo.CustomerEmployeeID;
                        break;
                    }
            }

            //Create the actual AspNetUser record
            this.Create(newuseraccount, STR_DEFAULT_PASSWORD);

            //Assign User to appropriate role
            //Uses the GUID like userId from the User's table
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

        //List all current users
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<UserProfile> ListAllUsers()
        {
            //We will be using the role manager to get the roles
            var rm = new RoleManager();

            //Get the current users off the User security table.
            var results = from person in Users.ToList()
                          select new UserProfile()
                          {
                              UserId = person.Id,
                              UserName = person.UserName,
                              Email = person.Email,
                              EmailConfirmed = person.EmailConfirmed,
                              CustomerId = person.CustomerId,
                              EmployeeId = person.EmployeeId,
                              RoleMemberships = person.Roles.Select(r => rm.FindById(r.RoleId).Name)
                          };

            //Using our own data tables, gather the user First Name and Last Name
            using (var context = new ChinookContext())
            {
                Employee etemp;
                Customer ctemp;

                foreach(var person in results)
                {
                    if(person.EmployeeId.HasValue)
                    {
                        etemp = context.Employees.Find(person.EmployeeId);
                        person.FirstName = etemp.FirstName;
                        person.LastName = etemp.LastName;
                    }

                    else if (person.CustomerId.HasValue)
                    {
                        ctemp = context.Customers.Find(person.CustomerId);
                        person.FirstName = ctemp.FirstName;
                        person.LastName = ctemp.LastName;
                    }

                    else
                    {
                        person.FirstName = "Unknown";
                        person.LastName = "";
                    }
                }
            }
            return results.ToList();
        }//EOM

        //Add a user to the User Table (ListView)
        [DataObjectMethod(DataObjectMethodType.Insert,true)]
        public void AddUser(UserProfile userinfo)
        {
            //Create an instance representing the new user.
            var useraccount = new ApplicationUser()
            {
                UserName = userinfo.UserName,
                Email = userinfo.Email
            };

            //Create the new user on the physical Users table.
            this.Create(useraccount, STR_DEFAULT_PASSWORD);

            //Create the UserRoles which were chosen at Insert time.
            foreach(var roleName in userinfo.RoleMemberships)
            {
                this.AddToRole(useraccount.Id, roleName);
            }
        }//EOM

        //Delete a user from the User Table (ListView)
        [DataObjectMethod(DataObjectMethodType.Delete,true)]
        public void RemoveUser(UserProfile userinfo)
        {
            //Business rule:
            //The webmaster cannot be deleted.

            //Realize that the only information at this time is the DataKeyNames value which is the User ID 
            //(On the User security table the field is ID)

            //Obtain the UserName from the security User table using the UserId value.

            string UserName = this.Users.Where(u => u.Id == userinfo.UserId).Select(u => u.UserName).SingleOrDefault().ToString();

            //Remove the user
            if(UserName.Equals(STR_WEBMASTER_USERNAME))
            {
                throw new Exception("The webmaster account cannot be removed.");
            }
            this.Delete(this.FindById(userinfo.UserId));
        }
    }//EOC
}//EON
