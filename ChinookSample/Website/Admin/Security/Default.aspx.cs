using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Security Namespace
using ChinookSystem.Security;
#endregion

public partial class Admin_Security_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RefreshAll(object sender, EventArgs e)
    {
        DataBind();
    }

    protected void UnregisteredUsersGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //Position the gridview to the selectedindex (row) that caused the postback
        UnregisteredUsersGridView.SelectedIndex = e.NewSelectedIndex;

        //Setup a variable that will be the physical pointer to the selected row.
        GridViewRow agvrow = UnregisteredUsersGridView.SelectedRow;

        //You can always check a pointer to see if something has been obtained.
        if(agvrow != null)
        {
            //Access information contained in a textbox on the gridview row.
            //Use the method .FindControl("controlidname") as controltype
            //Once you have a control, you can access the data content of the control using the control's access method.
            string assignedusername = "";
            TextBox inputControl = agvrow.FindControl("AssignedUserName") as TextBox;
            if(inputControl != null)
            {
                assignedusername = inputControl.Text;
            }
            string assignedemail = (agvrow.FindControl("AssignedEmail") as TextBox).Text;

            //Create the UnregisteredUser instance.
            //During the creation, I will pass to it the needed data to load the instance attributes.

            //Accessing boundfields on a gridview row uses .Cells[index].Text
            //Index represents the column of the grid.
            //Columns are index(starting at 0)

            //Another way of loading it
            //UnRegisteredUserProfile user = new UnRegisteredUserProfile();
            //{
            //    user.UserId = int.Parse(UnregisteredUsersGridView.SelectedDataKey.Value.ToString());
            //    user.UserType = (UnRegisteredUserType)Enum.Parse(typeof(UnRegisteredUserType), agvrow.Cells[1].Text);
            //    user.FirstName = agvrow.Cells[2].Text;
            //    user.LastName = agvrow.Cells[3].Text;
            //    user.UserName = assignedusername;
            //    user.Email = assignedemail;
            //}
            UnRegisteredUserProfile user = new UnRegisteredUserProfile()
            {
                CustomerEmployeeID = int.Parse(UnregisteredUsersGridView.SelectedDataKey.Value.ToString()),
                UserType = (UnRegisteredUserType)Enum.Parse(typeof(UnRegisteredUserType), agvrow.Cells[1].Text),
                FirstName = agvrow.Cells[2].Text,
                LastName = agvrow.Cells[3].Text,
                AssignedUserName = assignedusername,
                AssignedEmail = assignedemail
            };

            //Register the user via the Chinook.UserManager controller.
            UserManager sysmgr = new UserManager();
            sysmgr.RegisterUser(user);

            //Assume successful creation of a user.
            //Refresh the form.
            DataBind();


            // RefreshAll(sender, e); - Can't do this because the objecttype GridViewSelectEventArgs is not equal to EventArgs
        }
    }
}