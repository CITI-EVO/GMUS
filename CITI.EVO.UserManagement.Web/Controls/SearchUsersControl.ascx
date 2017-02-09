<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchUsersControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.SearchUsersControl" %>
<div>
    <asp:TextBox ID="tbUsersKeyword" Width="320" runat="server"></asp:TextBox>
    <asp:ImageButton ID="btSearchUsers" ImageUrl="~/App_Themes/default/images/view.png" runat="server" OnClick="btSearchUsers_Click" />
</div>
<div>
    <asp:ListBox ID="lstUsers" DataTextField="LoginName" Style="width: 320px; height: 150px;" DataValueField="ID" runat="server" Property="{SearchUsersModel.UserID=selectedValue}" />
</div>
