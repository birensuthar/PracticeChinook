using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace ChinookSystem.Data.Entities
{
    //point to the sql table that this file maps
    [Table("Artists")]
    public class Artist
    {
        //Key notations is optional if the sql pkey
        //ends in ID or Id
        //required if default of entity is NOT Identity
        //required if pkey is compound

        //properties can be fully implemented or
        //auto implemented
        //property names should use sql attribute name
        //properties should be listed in the same order
        //     as sql table attributes for easy of maintenance
        [Key]
        public int ArtistId { get; set; }
        public string Name { get; set; }

        //navigation properties for use by Linq
        //these properties will be of type vitural
        //there are two types of navigation properties
        //properties that point to "children" use ICollection<T>
        //properties that point to "Parent" use ParentName as the datatype
        public virtual ICollection<Album> Albums { get; set; }
    }
}
