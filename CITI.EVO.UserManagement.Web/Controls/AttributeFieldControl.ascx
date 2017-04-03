<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeFieldControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.AttributeFieldControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{AttributeFieldModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdSchemaID" Property="{AttributeFieldModel.SchemaID=Value}" />


<div class="form-group">
    <label class="col-sm-2 font-normal">სახელი</label>
    <div class="col-sm-12">
        <asp:TextBox ID="tbxName" runat="server" CssClass="form-control" Property="{AttributeFieldModel.Name=Text}"></asp:TextBox>
    </div>
</div>
