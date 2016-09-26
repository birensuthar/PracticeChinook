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

    //Point to the sql table that this file maps
    [Table("Tracks")]

    public class Track
    {
        //Key notations is optional if the sql pkey ends in id.
        //Required if default entity is NOT Identity
        //Required if pkey is compound.

        //Properties can be fully implemented or auto implemented.
        //Property names should use SQL attributes name
        //Properties should be in the same order as sql attributes for ease of maintenance.
        [Key]
        public int TrackId { get; set; }
        public string Name { get; set; }
        public int? AlbumId { get; set; }
        public int MediaTypeId { get; set; }
        public int GenreId { get; set; }
        public string Composer { get; set; }
        public int Milliseconds { get; set; }
        public int? Bytes { get; set; }
        public decimal UnitPrice { get; set; }
        //Navigation properties for use by linq.
        //These properties will be of type virutal
        //There are 2 types of navigation properties. 
        //Properties that point to "children" use ICollection<T>
        //Properties that point to "Parent" use the Parent Name as the datatpye
        public virtual MediaType MediaTypes { get; set; }
        public virtual Album Albums { get; set; }
    }
}

