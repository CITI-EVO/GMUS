<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResourcesFilterControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.Filters.ResourcesFilterControl" %>

<ce:ASPxComboBox ID="cmbProject" TextField="Value" ValueField="Key" ValueType="System.Guid" runat="server" AutoPostBack="true" Property="{ResourcesFilterModel.ProjectID=Value}">
</ce:ASPxComboBox>
<asp:TextBox runat="server" ID="tbxKeyword" Property="{ResourcesFilterModel.Keyword=Text}"></asp:TextBox>