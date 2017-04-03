<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeSchemaControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.AttributeSchemaControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{AttributeSchemaModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdProjectID" Property="{AttributeSchemaModel.ProjectID=Value}" />
<div class="form-group">
    <label class="col-sm-2 font-normal">სახელი</label>
    <div class="col-sm-12">
        <asp:TextBox ID="tbxName" runat="server" class="form-control" Property="{AttributeSchemaModel.Name=Text}"></asp:TextBox>
    </div>
</div>
