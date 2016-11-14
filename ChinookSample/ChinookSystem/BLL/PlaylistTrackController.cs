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
using System.Transactions;
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
                              orderby x.TrackNumber
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
            /*
            two approaches:
            one:
             wrap using context inside a TransactionScope. this alows for multiple
             SaveChanges() within code. Note: SaveChanges() internally has its own
             TransactionScope. One can nest TransactionsScopes.
             requires reference setting System.TransactionScope and using System.TransactionScope

            commented code is this approach

            two:
            Add a HashSet<T> to any entity that has ICollection<T> see Playlist entity
            This HashSet<T> works with EntityFramework to internally create all entries
            to the database and the need for generated values (identity).

            All work of adding or updating is done at one time (SaveChanges()). Logically
            you "assume" that the identity value is known when you do your coding.

            The adding of ICollection<T> records is done using a navigatitional approach.

                parent.NavigationProperty.Add(childentity)

            */
            //using (TransactionScope scope = new TransactionScope())
            //{ 
                using (var context = new ChinookContext())
                {
                    int tracknumber = 0;
                    Playlist existing = (from x in context.PlayLists
                                    where x.Name.Equals(playlistname)
                                    && x.CustomerId == customerid
                                    select x).FirstOrDefault();
                    PlaylistTrack newtrack = null;

                    if (existing == null)
                    {
                        existing = new Playlist();
                        existing.Name = playlistname;
                        existing.CustomerId = customerid;
                        existing = context.PlayLists.Add(existing);
                        //context.SaveChanges();
                        
                        tracknumber = 1;
                    }
                    else
                    {

                        tracknumber = existing.PlaylistTracks.Count() + 1;
                        newtrack = existing.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackid);
                    }

                    // PlaylistTrack 
                    if (newtrack != null)
                    {
                        throw new Exception("Playlist already has requested track.");
                    }
                    //for testing
                    //if (playlistname.Equals("Boom"))
                    //{
                    //    throw new Exception("Playlist test rollback.");
                    //}
                    newtrack = new PlaylistTrack();
                    //newtrack.PlaylistId = existing.PlaylistId;
                    newtrack.TrackId = trackid;
                    newtrack.TrackNumber = tracknumber;
                    //context.PlaylistTracks.Add(newtrack); //approach one
                    existing.PlaylistTracks.Add(newtrack); //approach two remove for approach one
                    context.SaveChanges();
                } //eouc
            //    scope.Complete();
            //}//eout
        }

        public void MoveTrack(string playlistname, int trackid, int tracknumber, int customerid, string updown)
        {
            using (var context = new ChinookContext())
            {
                Playlist existing = (from x in context.PlayLists
                                    where x.Name.Equals(playlistname)
                                    && x.CustomerId == customerid
                                    select x).FirstOrDefault();
                PlaylistTrack movetrack = (from x in context.PlaylistTracks
                                            where x.PlaylistId == existing.PlaylistId
                                            && x.TrackId == trackid
                                            select x).FirstOrDefault();
                PlaylistTrack othertrack = null;
                if (updown.Equals("up"))
                {
                     othertrack = (from x in context.PlaylistTracks
                                                where x.PlaylistId == existing.PlaylistId
                                                && x.TrackNumber == tracknumber - 1
                                                select x).FirstOrDefault();
                    movetrack.TrackNumber -= 1;
                    othertrack.TrackNumber += 1;
                }
                else
                {
                    othertrack = (from x in context.PlaylistTracks
                                  where x.PlaylistId == existing.PlaylistId
                                  && x.TrackNumber == tracknumber + 1
                                  select x).FirstOrDefault();
                    movetrack.TrackNumber += 1;
                    othertrack.TrackNumber -= 1;
                }
                context.Entry(movetrack).Property(y => y.TrackNumber).IsModified = true;
                context.Entry(othertrack).Property(y => y.TrackNumber).IsModified = true;
                context.SaveChanges();
            }
        }

        public void RemovePlaylistTrack(string playlistname, int trackid, int tracknumber, int customerid)
        {
            using (var context = new ChinookContext())
            {
                Playlist existing = (from x in context.PlayLists
                                     where x.Name.Equals(playlistname)
                                     && x.CustomerId == customerid
                                     select x).FirstOrDefault();
                var tracktoremove = context.PlaylistTracks.Find(existing.PlaylistId, trackid);
                List<PlaylistTrack> trackskept = (from x in existing.PlaylistTracks
                                                    where x.TrackNumber > tracknumber
                                                    orderby x.TrackNumber
                                                    select x).ToList();
                context.PlaylistTracks.Remove(tracktoremove);
                foreach (var track in trackskept)
                {
                    track.TrackNumber -= 1;
                    context.Entry(track).Property("TrackNumber").IsModified = true;
                }
                context.SaveChanges();

            }
        }
    }
}
