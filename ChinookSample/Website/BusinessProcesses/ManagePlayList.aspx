<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" 
    AutoEventWireup="true" CodeFile="ManagePlayList.aspx.cs" 
    Inherits="BusinessProcesses_ManagePlayList" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>





<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="jumbotron">
        <h3>Manage Playlist</h3>
    </div>
    <uc1:MessageUserControl runat="server" id="MessageUserControl" />
  
    <div class="row">
        <div class="col-sm-3">
            
            <asp:DropDownList ID="ArtistList" runat="server"
                 DataSourceID="ArtistListODS"
                 DataTextField="Name" 
                DataValueField="ArtistId"  
                Width="300px">
            </asp:DropDownList><br />
            <asp:Label ID="Label6" runat="server" Text="Artist" Width="50px"></asp:Label>&nbsp;&nbsp;
            <asp:Button ID="ArtistFetch" runat="server" Text="Fetch" CssClass="btn btn-primary" OnClick="ArtistFetch_Click" /><br /><br />
            
            <asp:DropDownList ID="MediaTypeList" runat="server"
                Width="300px" 
                DataSourceID="MediaTypeODS" 
                DataTextField="DisplayText" 
                DataValueField="PFKeyIdentifier">
            </asp:DropDownList><br />
            <asp:Label ID="Label9" runat="server" Text="Media" Width="50px"></asp:Label>&nbsp;&nbsp;
            <asp:Button ID="MediaTypeFetch" runat="server" Text="Fetch" CssClass="btn btn-primary" OnClick="MediaTypeFetch_Click"  /><br /><br />
            
            <asp:DropDownList ID="GenreList" runat="server"
                Width="300px" 
                DataSourceID="GenreListODS" 
                DataTextField="DisplayText" 
                DataValueField="PFKeyIdentifier">
            </asp:DropDownList><br />
            <asp:Label ID="Label7" runat="server" Text="Genre" Width="50px"></asp:Label>&nbsp;&nbsp;
            <asp:Button ID="GenreFetch" runat="server" Text="Fetch" CssClass="btn btn-primary" OnClick="GenreFetch_Click" /><br /><br />
           
             <asp:DropDownList ID="AlbumList" runat="server"
                Width="300px" 
                DataSourceID="AlbumListODS" 
                DataTextField="DisplayText" 
                DataValueField="PFKeyIdentifier">
            </asp:DropDownList><br />
            <asp:Label ID="Label8" runat="server" Text="Album" Width="50px"></asp:Label>&nbsp;&nbsp;
            <asp:Button ID="AlbumFetch" runat="server" Text="Fetch" CssClass="btn btn-primary" OnClick="AlbumFetch_Click" /><br />
        </div>
        <div class="col-sm-9">
            <%-- TracksBy and SearchArgID (can be hidden) are needed by the ListView ODS --%>
            <asp:Label ID="Label1" runat="server" Text="Tracks by "></asp:Label>
            <asp:Label ID="TracksBy" runat="server" ></asp:Label>
            <asp:Label ID="SearchArgID" runat="server" ></asp:Label>

            <%-- ItemCommand use for the + event on each row
                 I am hiding the TrackId for each row in ListView using CommandArgument --%>
            <asp:ListView ID="TrackSearchList" runat="server" 
                OnItemCommand="TrackSearchList_ItemCommand" DataSourceID="TracksForSelectionODS">

                <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
    
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                    <tr runat="server" style="">
                                        <th runat="server">TrackId</th>
                                        <th runat="server">Name</th>
                                        <th runat="server">Title</th>
                                        <th runat="server">MediaName</th>
                                        <th runat="server">GenreName</th>
                                        <th runat="server">Composer</th>
                                        <th runat="server">Milliseconds</th>
                                        <th runat="server">Bytes</th>
                                        <th runat="server">UnitPrice</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="">
                                <asp:DataPager runat="server" ID="DataPager2" PageSize="5" >
                                    <Fields>
                                        <asp:NumericPagerField ButtonType="Button" ButtonCount="4"  />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
              
                <ItemTemplate>
                <tr style="background-color: #DCDCDC; color: #000000;">
                    <td>
                        <asp:LinkButton ID="AddToPlayList" runat="server" CommandArgument='<%# Eval("TrackId") %>'
                            CssClass="btn" ><span aria-hidden="true" class="glyphicon glyphicon-plus"></span>
                        </asp:LinkButton>
                   
                    </td>
               
                    <td>
                        <asp:Label Text='<%# Eval("Name") %>' runat="server" ID="NameLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="Label2" /></td>
                    <td>
                       <asp:Label Text='<%# Eval("MediaName") %>' runat="server" ID="Label3" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("GenreName") %>' runat="server" ID="Label4" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Composer") %>' runat="server" ID="ComposerLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Milliseconds") %>' runat="server" ID="MillisecondsLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Bytes") %>' runat="server" ID="BytesLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("UnitPrice") %>' runat="server" ID="UnitPriceLabel" /></td>
                
                </tr>
            </ItemTemplate>
                  <AlternatingItemTemplate>
                <tr style="background-color: #FFF8DC;">
                    <td>
                       <asp:LinkButton ID="AddToPlayList" runat="server"  CommandArgument='<%# Eval("TrackId") %>' 
                            CssClass="btn"><span aria-hidden="true" class="glyphicon glyphicon-plus"></span></asp:LinkButton>
                    </td>
             
                    <td>
                        <asp:Label Text='<%# Eval("Name") %>' runat="server" ID="NameLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="Label2" /></td>
                    <td>
                       <asp:Label Text='<%# Eval("MediaName") %>' runat="server" ID="Label3" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("GenreName") %>' runat="server" ID="Label4" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Composer") %>' runat="server" ID="ComposerLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Milliseconds") %>' runat="server" ID="MillisecondsLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Bytes") %>' runat="server" ID="BytesLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("UnitPrice") %>' runat="server" ID="UnitPriceLabel" /></td>
               
                </tr>
            </AlternatingItemTemplate>
        
            </asp:ListView>
            <br />
       
            <asp:Label ID="Label5" runat="server" Text="PlayList"></asp:Label>&nbsp;&nbsp;
             <asp:TextBox ID="PlayListName" runat="server"></asp:TextBox>&nbsp;&nbsp;
            <asp:Button ID="PlayListFetch" runat="server" Text="Fetch" CssClass="btn btn-primary" OnClick="PlayListFetch_Click" />&nbsp;&nbsp;
            <asp:LinkButton ID="MoveUp" runat="server" CssClass="btn" OnClick="MoveUp_Click" >
                <span aria-hidden="true" class="glyphicon glyphicon-chevron-up"></span>
            </asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="MoveDown" runat="server" CssClass="btn"  OnClick="MoveDown_Click">
                <span aria-hidden="true" class="glyphicon glyphicon-chevron-down"></span>
            </asp:LinkButton>&nbsp;&nbsp;
             <asp:LinkButton ID="DeleteTrack" runat="server" CssClass="btn"  OnClick="DeleteTrack_Click">
                <span aria-hidden="true" class="glyphicon glyphicon-remove" style="color:red"></span>
            </asp:LinkButton>
           <br /><br />

            <asp:GridView ID="CurrentPlayList" runat="server" AutoGenerateColumns="False" 
                Caption="PlayList" GridLines="Horizontal" BorderStyle="None">
                <Columns>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="Label1"></asp:Label>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Album">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("Title") %>' ID="Label2"></asp:Label>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MSec">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("Milliseconds") %>' ID="Label3"></asp:Label>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="$">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("UnitPrice") %>' ID="Label4"></asp:Label>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pd">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Bind("Purchased") %>' Enabled="false" ID="CheckBox1"></asp:CheckBox>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("TrackNumber") %>' ID="TrackNumber" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="TrackId" runat="server" Text='<%# Eval("TrackId") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No current tracks on play list.
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="Aqua" />
            </asp:GridView>
       </div>
    </div>
    <asp:ObjectDataSource ID="ArtistListODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Artist_ListAll" TypeName="ChinookSystem.BLL.ArtistController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="MediaTypeODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="MediaTypeList" TypeName="ChinookSystem.BLL.MediaTypeController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GenreListODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GenreList" TypeName="ChinookSystem.BLL.GenreController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="AlbumListODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="AlbumList" TypeName="ChinookSystem.BLL.AlbumController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="TracksForSelectionODS" runat="server" 
        DataObjectTypeName="ChinookSystem.Data.Entities.Track" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Get_TracksForPlaylistSelection" 
        TypeName="ChinookSystem.BLL.TrackController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SearchArgID" PropertyName="Text" Name="id" Type="Int32"></asp:ControlParameter>
            <asp:ControlParameter ControlID="TracksBy" PropertyName="Text" Name="fetchby" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

