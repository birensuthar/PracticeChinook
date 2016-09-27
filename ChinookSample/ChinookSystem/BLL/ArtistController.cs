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
    public class ArtistController
    {
        //Dump the entire artist entity;
        //This will use Entity Framework Access
        //Entity classes will be used to define the data.
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Artist> Artist_ListAll()
        {
            //Setup transaction area.
            using (var context = new ChinookContext())
            {
                return context.Artists.ToList();
            }
        }


        //Report a dataset containing data from multiple entities.
        //This will use linq to Entity Cases
        //Poco classes will be used to define the data
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ArtistAlbum> ArtistAlbums_Get()
        {
            //Setup transaction area.
            using (var context = new ChinookContext())
            {
                //When you bring your query from linqpad to your program you must change the references to the data source.
                //You may also need to change your navigation referencing used in linqpad to the navigation properties you stated in
                //the entity class definitions.
                var results = from x in context.Albums
                              where x.ReleaseYear == 2008
                              orderby x.Artists.Name, x.Title
                              select new ArtistAlbum
                              {
                                  //Name and Title are POCO class property names.
                                  Name = x.Artists.Name,
                                  Title = x.Title
                              };
                //The following requires the query data in memory.
                //.ToList()
                //At this point the query will actually execute.
                return results.ToList();
            }
        }
    }
}
