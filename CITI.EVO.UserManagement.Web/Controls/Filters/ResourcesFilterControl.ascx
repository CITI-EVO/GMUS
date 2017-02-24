<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResourcesFilterControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.Filters.ResourcesFilterControl" %>

<div class="col-sm-2">
    <ce:DropDownList ID="cmbProject" DataTextField="Value" DataValueField="Key" CssClass="chosen-select" runat="server" AutoPostBack="true" Property="{ResourcesFilterModel.ProjectID=SelectedValue}">
    </ce:DropDownList>
</div>
<div class="col-sm-2">
    <asp:TextBox runat="server" class="form-control" ID="tbxKeyword" Property="{ResourcesFilterModel.Keyword=Text}"></asp:TextBox>
</div>
