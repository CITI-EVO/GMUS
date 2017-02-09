<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResourceControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ResourceControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ResourceModel.ID=Value}" />

<table>
    <tr>
        <td>
            <asp:Label runat="server">რესურსის სახელი</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbResourceName" runat="server" Property="{ResourceModel.Name=Text}"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server">რესურსის ტიპი</asp:Label>
        </td>
        <td>
            <ce:ASPxComboBox ID="cmbResourceType" TextField="Key" ValueField="Value" runat="server" ValueType="System.Int32" Property="{ResourceModel.Type=Value}">
            </ce:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server">მნიშვნელობა</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbResourceValue" runat="server" Property="{ResourceModel.Value=Text}"></asp:TextBox>
        </td>
    </tr>
</table>
