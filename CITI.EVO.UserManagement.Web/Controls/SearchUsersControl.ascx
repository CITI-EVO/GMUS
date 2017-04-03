<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchUsersControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.SearchUsersControl" %>

<div class="form-group">
    <div class="col-lg-10">
        <asp:TextBox ID="tbUsersKeyword" CssClass="form-control" runat="server"></asp:TextBox>
    </div>
    <div class="col-sm-2">
        <ce:LinkButton ID="btSearchUsers" CssClass="btn btn-warning fa fa-share-square-o" runat="server" OnClick="btSearchUsers_Click" />
    </div>
</div>
<div class="form-group">
    <div class="col-lg-12">
        <asp:ListBox ID="lstUsers" DataTextField="LoginName" CssClass="form-control" Style="height: 150px;" DataValueField="ID" runat="server" Property="{SearchUsersModel.UserID=SelectedValue}" />
    </div>
</div>
