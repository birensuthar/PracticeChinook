using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Data.POCOs
{
    public class TracksForPlaylist
    {
        public int TrackId { get; set; }
        public int TrackNumber { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Milliseconds { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Purchased { get; set; }
    }
}
