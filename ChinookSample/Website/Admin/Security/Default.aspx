<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Security_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row jumbotron">
        <h1>User and Role Administration</h1>
    </div>
    <div class="row">
        <div class="col-md-12">
            <!-- Nav Tabs -->
            <ul class="nav nav-tabs">
                <li class="active"><a href="#users" data-toggle="tab">Users</a></li>
                <li><a href="#roles" data-toggle="tab">Roles</a></li>
                <li><a href="#unregistered" data-toggle="tab">UnRegistered Users</a></li>
            </ul>
            <!-- This will be the tab content area -->
            <div class="tab-content">
                <!-- User tab -->
                <div class="tab-pane fade in active" id="users">
                    <h1>Users</h1>
                </div> <%--eop--%>

                <!-- Role tab -->
                <div class="tab-pane fade" id="roles">
                    <h1>Roles</h1>
                </div> <%--eop--%>

                <!-- UnRegistered User tab -->
                <div class="tab-pane fade" id="unregistered">
                    <h1>UnRegistered Users</h1>
                </div> <%--eop--%>
            </div>
        </div>

    </div>
</asp:Content>

