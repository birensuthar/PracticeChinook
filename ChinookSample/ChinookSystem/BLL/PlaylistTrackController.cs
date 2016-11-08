using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additonal Namespaces
using System.ComponentModel; //ODS
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class PlaylistTrackController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<TracksForPlaylist> Get_PlaylistTracks(string playlistname, int customerid)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.PlaylistTracks
                              where x.PlayList.Name.Equals(playlistname)
                              && x.PlayList.CustomerId == customerid
                              select new TracksForPlaylist
                              {
                                  TrackId = x.TrackId,
                                  TrackNumber = x.TrackNumber,
                                  Name = x.Track.Name,
                                  Title = x.Track.Album.Title,
                                  Milliseconds = x.Track.Milliseconds,
                                  UnitPrice = x.Track.UnitPrice,
                                  Purchased = true
                              };
                return results.ToList();

            }
        }//eom

        public void AddTrackToPlayList(string playlistname, int trackid, int? customerid)
        {
            using (var context = new ChinookContext())
            {
                int tracknumber = 0;
                Playlist existing = (from x in context.PlayLists
                                     where x.Name.Equals(playlistname)
                                     && x.CustomerId == customerid
                                     select x).FirstOrDefault();
                if (existing == null)
                {
                    existing = new Playlist();
                    existing.Name = playlistname;
                    existing.CustomerId = customerid;
                    existing = context.PlayLists.Add(existing);
                    existing.PlaylistId = context.PlayLists.Count() + 1;
                    tracknumber = 1;
                }
                else
                {
                    tracknumber = existing.PlaylistTracks.Count() + 1;
                    //newtrack = existing.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackid);
                }

                PlaylistTrack newtrack = context.PlaylistTracks.Find(existing.PlaylistId, trackid);
                if (newtrack != null)
                {
                    throw new Exception("Playlist already has requested track.");
                }
                newtrack = new PlaylistTrack();
                newtrack.PlaylistId = existing.PlaylistId;
                newtrack.TrackId = trackid;
                newtrack.TrackNumber = tracknumber;
                context.PlaylistTracks.Add(newtrack);
                context.SaveChanges();

            }
        }
    }
}
