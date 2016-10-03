using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel; //For ODS
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class EmployeeController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<EmployeeNameList> EmployeeNameList_Get()
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Employees
                              orderby x.LastName, x.FirstName
                              select new EmployeeNameList
                              {
                                  EmployeeID = x.EmployeeID,
                                  Name = x.LastName + ", " + x.FirstName
                              };
                return results.ToList();
                            
            }
        }
    }
}
