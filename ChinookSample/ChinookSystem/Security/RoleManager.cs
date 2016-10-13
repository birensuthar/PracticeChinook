using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional NameSpaces
using Microsoft.AspNet.Identity;                   //RoleManager
using Microsoft.AspNet.Identity.EntityFramework;   //IdentityRole, RoleStore
#endregion

namespace ChinookSystem.Security
{
    public class RoleManager: RoleManager<IdentityRole>
    {
        public RoleManager():base (new RoleStore<IdentityRole>(new ApplicationDbContext()))
        {

        }
    }
}
