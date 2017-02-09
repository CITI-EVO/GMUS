<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ProjectControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ProjectModel.ID=Value}" />

<table>
    <tr>
        <td><ce:Label runat="server">მოდულის სახელი</ce:Label></td>
        <td><asp:TextBox ID="tbxName" runat="server" Property="{ProjectModel.Name=Text}"></asp:TextBox></td>
    </tr>
    <tr>
        <td><ce:Label ID="Label1" runat="server">მოდულის სტატუსი</ce:Label></td>
        <td><ce:CheckBox ID="chkIsActive" runat="server" Property="{ProjectModel.IsActive=Checked}" /></td>
    </tr>
</table>