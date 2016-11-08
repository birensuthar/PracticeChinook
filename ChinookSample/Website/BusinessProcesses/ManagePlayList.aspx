<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManagePlayList.aspx.cs" Inherits="BusinessProcesses_ManagePlayList" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="jumbotron">
        <h3>Manage Playlist</h3>
    </div>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <div class="row">
        <div class="col-sm-3">
            <asp:DropDownList ID="ArtistList" runat="server"
                 DataSourceID="ArtistListODS"
                 DataTextField="Name" 
                DataValueField="ArtistId"  Width="300px"></asp:DropDownList><br />
            <asp:Button ID="ArtistFetch" runat="server" Text="Fetch" CssClass="btn btn-primary" OnClick="ArtistFetch_Click" />
        </div>
        <div class="col-sm-9">
       
            <asp:Label ID="Label1" runat="server" Text="Tracks by "></asp:Label>
            <asp:Label ID="TracksBy" runat="server" ></asp:Label>
            <asp:ListView ID="TrackSearchList" runat="server"
                 OnPagePropertiesChanging="TrackSearchList_PagePropertiesChanging" 
                 OnItemCommand ="TrackSearchList_ItemCommand">
                <EmptyDataTemplate>
                    No data to display at this time
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; 
                                border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                    <th runat="server"></th>
                                    <th runat="server">Name</th>
                                    <th runat="server">Title</th>
                                    <th runat="server">Media</th>
                                    <th runat="server">Genre</th>
                                    <th runat="server">Composer</th>
                                    <th runat="server">MSec</th>
                                    <th runat="server">Bytes</th>
                                    <th runat="server">$</th>

                                </tr>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                            <asp:DataPager runat="server" ID="DataPager1" PageSize="5" PagedControlID="TrackSearchList" >
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                    <asp:NumericPagerField></asp:NumericPagerField>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
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
                            CssClass="btn" ><span aria-hidden="true" class="glyphicon glyphicon-plus"></span></asp:LinkButton>
                   
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
             <asp:TextBox ID="PlayListName" runat="server"></asp:TextBox><br />
            <asp:Button ID="PlayListFetch" runat="server" Text="Fetch" CssClass="btn btn-primary" OnClick="PlayListFetch_Click" />&nbsp;&nbsp;
           <asp:Button ID="PlayListSave" runat="server" Text="Save" CssClass="btn btn-primary" /><br />
            <asp:GridView ID="CurrentPlayList" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name"></asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Album"></asp:BoundField>
                    <asp:BoundField DataField="Milliseconds" HeaderText="MSec"></asp:BoundField>
                    <asp:BoundField DataField="UnitPrice" HeaderText="$"></asp:BoundField>
                    <asp:CheckBoxField DataField="Purchased" Text="Paid"></asp:CheckBoxField>
                    <asp:BoundField DataField="TrackId" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="TrackNumber"></asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    No current tracks on play list.
                </EmptyDataTemplate>
            </asp:GridView>
       </div>
    </div>
    <asp:ObjectDataSource ID="ArtistListODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Artist_ListAll" TypeName="ChinookSystem.BLL.ArtistController"></asp:ObjectDataSource>
</asp:Content>

