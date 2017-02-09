<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateUserControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.CreateUserControl" %>

<%@ Register TagPrefix="local" TagName="UserDataControl" Src="~/Controls/UserDataControl.ascx" %>
<%@ Register TagPrefix="local" TagName="SelectGroupsControl" Src="~/Controls/SelectGroupsControl.ascx" %>

<div>
    <local:UserDataControl runat="server" ID="userDataControl" Property="{CreateUserModel.User=Model}" />
</div>
<div>
    <local:SelectGroupsControl runat="server" ID="SelectGroupsControl" Property="{CreateUserModel.Groups=Model}" />
</div>
