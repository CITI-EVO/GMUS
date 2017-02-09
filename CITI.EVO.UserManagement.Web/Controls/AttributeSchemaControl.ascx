<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeSchemaControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.AttributeSchemaControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{AttributeSchemaModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdProjectID" Property="{AttributeSchemaModel.ProjectID=Value}" />

<div class="box">
    <h3>
        <asp:Label runat="server">სახელი</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbxName" runat="server" Property="{AttributeSchemaModel.Name=Text}"></asp:TextBox>
    </div>
</div>