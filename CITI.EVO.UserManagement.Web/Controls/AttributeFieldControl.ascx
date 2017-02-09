<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeFieldControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.AttributeFieldControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{AttributeSchemaNodeModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdSchemaID" Property="{AttributeSchemaNodeModel.SchemaID=Value}" />

<div class="box">
    <h3>
        <asp:Label runat="server">სახელი</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbxName" runat="server" Property="{AttributeSchemaNodeModel.Name=Text}"></asp:TextBox>
    </div>
</div>