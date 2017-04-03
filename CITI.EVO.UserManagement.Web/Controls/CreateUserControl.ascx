<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateUserControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.CreateUserControl" %>

<%@ Register TagPrefix="local" TagName="UserDataControl" Src="~/Controls/UserDataControl.ascx" %>
<%@ Register TagPrefix="local" TagName="SelectGroupsControl" Src="~/Controls/SelectGroupsControl.ascx" %>

<asp:Panel runat="server" CssClass="form-group">
    <local:UserDataControl runat="server" ID="userDataControl" Property="{CreateUserModel.User=Model}" OnDataChanged="userDataControl_OnDataChanged" />
</asp:Panel>
<asp:Panel runat="server" CssClass="form-group" Visible="False">
    <local:SelectGroupsControl runat="server" ID="SelectGroupsControl" Property="{CreateUserModel.Groups=Model}" OnDataChanged="SelectGroupsControl_OnDataChanged" />
</asp:Panel>
