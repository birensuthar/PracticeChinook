using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additonal Namespaces
using System.ComponentModel;        //ODS
using ChinookSystem.Data.Entities;  //Track Entity
using ChinookSystem.Data.POCOs;     //May or may not be needed
using ChinookSystem.DAL;            //Will Definitely needed
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class TrackController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Track> ListTracks()
        {
            using (var context = new ChinookContext())
            {
                //Return all records all attributes.
                return context.Tracks.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Track Get_Track(int trackid)
        {
            using (var context = new ChinookContext())
            {
                //Return a records all attributes.
                return context.Tracks.Find(trackid);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void AddTrack(Track trackinfo)
        {
            using (var context = new ChinookContext())
            {
                //Any business rules that may prevent you from doing an actual add.
                if (trackinfo.UnitPrice > 1.0m)
                    throw new Exception("Bob's your uncle");
                //Any data refinements

                //This is a review of using iif (Immediate IF)
                //Composer can be a null string, we dont with to store an empty string.
                trackinfo.Composer = string.IsNullOrEmpty(trackinfo.Composer) ? null : trackinfo.Composer;

                //Add the instance of trackinfo to the database.
                context.Tracks.Add(trackinfo);

                //Commit of the transaction.
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void UpdateTrack(Track trackinfo)
        {
            using (var context = new ChinookContext())
            {
                //Any business rules that may prevent you from doing an actual add.

                //Any data refinements

                //This is a review of using iif (Immediate IF)
                //Composer can be a null string, we dont with to store an empty string.
                trackinfo.Composer = string.IsNullOrEmpty(trackinfo.Composer) ? null : trackinfo.Composer;

                //Update the existing instance of trackinfo on the database.
                context.Entry(trackinfo).State = System.Data.Entity.EntityState.Modified;

                //Commit of the transaction.
                context.SaveChanges();
            }
        }

        //The delete is an overloaded method technique

        [DataObjectMethod(DataObjectMethodType.Delete,true)]
        public void DeleteTrack(Track trackinfo)
        {
            DeleteTrack(trackinfo.TrackId);
        }

        public void DeleteTrack(int trackid)
        {
            using (var context = new ChinookContext())
            {
                //Any business rules

                //Do the delete.
                //Find the existing record on the database.

                var existing = context.Tracks.Find(trackid);
                //Delete the record from the database.
                context.Tracks.Remove(existing);
                //commit the transaction
                context.SaveChanges();
            }
        }

        #region Business Processes
        public List<TracksForPlaylistSelection> Get_TracksForPlaylistSelection(int id, string fetchby)
        {
            List<TracksForPlaylistSelection> results = null;
            using (var context = new ChinookContext())
            {
                switch (fetchby)
                {
                    case "Artist":
                        {
                            results = (from x in context.Tracks
                                       where x.Album.ArtistId == id
                                       select new TracksForPlaylistSelection
                                       {
                                           TrackId = x.TrackId,
                                           Name = x.Name,
                                           Title = x.Album.Title,
                                           MediaName = x.MediaType.Name,
                                           GenreName = x.Genre.Name,
                                           Composer = x.Composer,
                                           Milliseconds = x.Milliseconds,
                                           Bytes = x.Bytes,
                                           UnitPrice = x.UnitPrice
                                       }).ToList();
                            break;
                        }
                }
            }
            return results;
        }
        #endregion
    }
}
