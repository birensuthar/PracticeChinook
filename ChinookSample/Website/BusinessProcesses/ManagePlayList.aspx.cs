using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.POCOs;
using Chinook.UI;
using ChinookSystem.Security;
using Microsoft.AspNet.Identity;
#endregion

public partial class BusinessProcesses_ManagePlayList : System.Web.UI.Page
{
    #region Key Page Event Handlers and Overrides
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            TrackSearchList.DataSource = null;
        }
    }
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        // PreRenderComplete occurs just after databindings page events
        // And saves to viewstate
        if ((TrackSearchList.FindControl("DataPager2") as DataPager) != null)
        {
            // Trick on search to avoid "No data" on results when old page is greater than actual row count                
            if ((TrackSearchList.FindControl("DataPager2") as DataPager).StartRowIndex > (TrackSearchList.FindControl("DataPager2") as DataPager).TotalRowCount)
                (TrackSearchList.FindControl("DataPager2") as DataPager).SetPageProperties(0, (TrackSearchList.FindControl("DataPager2") as DataPager).MaximumRows, true);
        }
    }

    protected override void Render(HtmlTextWriter writer)
    { 
        //this code sets up the ability to click anywhere on a row of a specified gridview :CurrentPlayList
        foreach (GridViewRow r in CurrentPlayList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                r.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                r.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                r.ToolTip = "Click to select row";
                r.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.CurrentPlayList, "Select$" + r.RowIndex, true);

            }
        }
        
        base.Render(writer);
    }
    #endregion

    #region Search Fetch buttons
    protected void ArtistFetch_Click(object sender, EventArgs e)
    {
       
        MessageUserControl.TryRun((ProcessRequest)FetchTracksForArtist);
    }
    public void FetchTracksForArtist()
    {
        int id = int.Parse(ArtistList.SelectedValue);
        TracksBy.Text = "Artist";
        SearchArgID.Text = id.ToString();
        TrackSearchList.DataBind();
    }
    protected void MediaTypeFetch_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun((ProcessRequest)FetchTracksForMedia);
    }
    public void FetchTracksForMedia()
    {
        int id = int.Parse(MediaTypeList.SelectedValue);
        TracksBy.Text = "Media";
        SearchArgID.Text = id.ToString();
        TrackSearchList.DataBind();
    }

    protected void GenreFetch_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun((ProcessRequest)FetchTracksForGenre);
    }
    public void FetchTracksForGenre()
    {
        int id = int.Parse(GenreList.SelectedValue);
        TracksBy.Text = "Genre";
        SearchArgID.Text = id.ToString();
        TrackSearchList.DataBind();
    }

    protected void AlbumFetch_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun((ProcessRequest)FetchTracksForAlbum);
    }
    public void FetchTracksForAlbum()
    {
        int id = int.Parse(AlbumList.SelectedValue);
        TracksBy.Text = "Album";
        SearchArgID.Text = id.ToString();
        TrackSearchList.DataBind();
    }

    #endregion

    #region Get a customerid via the log in
    protected int GetUserCustomerId()
    {
        int customerid = 0;
        //is the current user logged on
        if (Request.IsAuthenticated)
        {
            //get the current user name from aspnet User.Identity
            //this name will be the name shown in the right hand corner
            //of the form
            string username = User.Identity.Name;

            //use the security UserManager controller we coded
            //which ties into aspnet.Identity
            //this will be used to get an ApplicationUser instance of the 
            //current user
            //include a using to the controller
            UserManager sysmgr = new UserManager();

            //needs using Mircosoft.Aspnet.Identity
            var applicationuser = sysmgr.FindByName(username);

            //get the customerid from the applicationuser
            customerid = applicationuser.CustomerId == null ? 0 : (int)applicationuser.CustomerId;
        }
        else
        {
            MessageUserControl.ShowInfo("You must log in to manage a playlist.");
        }
        return customerid;
    }
    #endregion

    #region Add a track to a playlist
    protected void TrackSearchList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int customerid = GetUserCustomerId();

        if (customerid > 0)  //is the user a customer
        {
            ListViewDataItem rowcontents = e.Item as ListViewDataItem;
            string playlistname = PlayListName.Text;
            if (string.IsNullOrEmpty(PlayListName.Text))
            {
                MessageUserControl.ShowInfo("Please enter a playlist name.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    //the trackid is attached to each ListView row via the CommandArgument parameter

                    //this method does not make the value visible to the user (or in view source unless
                    //   the hacker decompressed the hidden data)

                    //access to the trackid is done via the ListViewCommandEventsArgs e and is treated
                    //as an object, thus it needs to be cast to a string for the Parse to work

                    PlaylistTrackController sysmgr = new PlaylistTrackController();
                    sysmgr.AddTrackToPlayList(playlistname, int.Parse(e.CommandArgument.ToString()), customerid);
                    List<TracksForPlaylist> results = sysmgr.Get_PlaylistTracks(playlistname, customerid);
                    CurrentPlayList.DataSource = results;
                    CurrentPlayList.DataBind();
                });
            }
        }
        else
        {
            MessageUserControl.ShowInfo("Please use your customer account to manage playlists.");
        }
    }
    #endregion

    #region Fetch a specified playlist for logged customer
    protected void PlayListFetch_Click(object sender, EventArgs e)
    {
        int customerid = GetUserCustomerId();
        if (customerid > 0)
        {
            MessageUserControl.TryRun(() =>
            {
                if (string.IsNullOrEmpty(PlayListName.Text))
                {
                    throw new Exception("Enter a playlist name.");
                }
                else
                {
                    PlaylistTrackController sysmgr = new PlaylistTrackController();
                    List<TracksForPlaylist> results = sysmgr.Get_PlaylistTracks(PlayListName.Text, customerid);
                    CurrentPlayList.DataSource = results;
                    CurrentPlayList.DataBind();
                    CurrentPlayList.SelectedIndex = -1;
                }
            });
        }
    }

    #endregion

    #region Arrange or Remove Tracks
    protected void MoveUp_Click(object sender, EventArgs e)
    {
        if (CurrentPlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("You must have a playlist with entries before trying to rearrange the tracks.");
        }
        else
        {
            int selectedrowindex = CurrentPlayList.SelectedIndex;
            if (selectedrowindex > -1)
            {
                if (selectedrowindex > 0)
                {
                    //MessageUserControl.ShowInfo("selected index is " + selectedrowindex.ToString() + " and can be moved");
                    MoveTrack(selectedrowindex, "up");
                }
            }
        }
    }

    protected void MoveDown_Click(object sender, EventArgs e)
    {
        if (CurrentPlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("You must have a playlist with entries before trying to rearrange the tracks.");
        }
        else
        {
            int selectedrowindex = CurrentPlayList.SelectedIndex;
            if (selectedrowindex > -1)
            {
                if (CurrentPlayList.Rows.Count > selectedrowindex + 1)
                {
                    //MessageUserControl.ShowInfo("selected index is " + selectedrowindex.ToString() + " and can be moved");
                    MoveTrack(selectedrowindex, "down");
                }
            }
        }
    }

    protected void MoveTrack(int selectedrowindex, string direction)
    {
        int customerid = GetUserCustomerId();
        int trackid = int.Parse((CurrentPlayList.Rows[selectedrowindex].FindControl("TrackId") as Label).Text);
        int tracknumber = int.Parse((CurrentPlayList.Rows[selectedrowindex].FindControl("TrackNumber") as Label).Text);
        MessageUserControl.TryRun(() =>
        {
            PlaylistTrackController sysmgr = new PlaylistTrackController();
            sysmgr.MoveTrack(PlayListName.Text, trackid, tracknumber, customerid, direction);
            List<TracksForPlaylist> results = sysmgr.Get_PlaylistTracks(PlayListName.Text, customerid);
            CurrentPlayList.DataSource = results;
            if (direction.Equals("up"))
            {
                CurrentPlayList.SelectedIndex = selectedrowindex - 1;
            }
            else
            {
                CurrentPlayList.SelectedIndex = selectedrowindex + 1;
            }
            CurrentPlayList.DataBind();
        });
    }
    protected void DeleteTrack_Click(object sender, EventArgs e)
    {
        if (CurrentPlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("You must have a playlist with entries before trying to remove the track.");
        }
        else
        {
            int selectedrowindex = CurrentPlayList.SelectedIndex;

            if (selectedrowindex > -1)
            {
                int customerid = GetUserCustomerId();
                int trackid = int.Parse((CurrentPlayList.Rows[selectedrowindex].FindControl("TrackId") as Label).Text);
                int tracknumber = int.Parse((CurrentPlayList.Rows[selectedrowindex].FindControl("TrackNumber") as Label).Text);
                //MessageUserControl.ShowInfo("track is >" + (CurrentPlayList.Rows[selectedrowindex].FindControl("TrackId") as Label).Text + "<");
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTrackController sysmgr = new PlaylistTrackController();
                    sysmgr.RemovePlaylistTrack(PlayListName.Text, trackid, tracknumber, customerid);
                    List<TracksForPlaylist> results = sysmgr.Get_PlaylistTracks(PlayListName.Text, customerid);
                    CurrentPlayList.DataSource = results;
                    CurrentPlayList.SelectedIndex = -1;
                    CurrentPlayList.DataBind();
                });
            }

        }
    }
    #endregion
    
}