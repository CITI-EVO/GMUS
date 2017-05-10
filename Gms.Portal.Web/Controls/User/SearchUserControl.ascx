<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchUserControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.SearchUserControl" %>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Users</ce:Label>
    <div class="col-lg-10">
        <ce:ListBox runat="server" ID="lstUsers" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" CssClass="chosen-select" SelectionMode="Multiple" multiple="multiple">
        </ce:ListBox>
    </div>
</div>
