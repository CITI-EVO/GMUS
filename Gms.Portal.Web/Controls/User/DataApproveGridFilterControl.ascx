<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataApproveGridFilterControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.DataApproveGridFilterControl" %>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Form</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxForm" DataTextField="Name" DataValueField="ID" Property="{DataApproveGridFilterModel.FormID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True">
        </ce:DropDownList>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Field</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxField" DataTextField="Name" DataValueField="ID" Property="{DataApproveGridFilterModel.FieldID=SelectedValue}" CssClass="chosen-select">
        </ce:DropDownList>
    </div>
</div>