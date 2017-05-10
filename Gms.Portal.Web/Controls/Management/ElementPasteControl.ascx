<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ElementPasteControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.ElementPasteControl" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdfldElementId" Property="{ElementPasteModel.DestinationId=Value}" />

<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Form</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxForms" CssClass="chosen-select" AutoPostBack="true" OnSelectedIndexChanged="cbxForms_SelectedIndexChanged"  DataTextField="Name" DataValueField="ID" Property="{ElementPasteModel.FormId=SelectedValue}" />
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Element</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxMovingElements" CssClass="chosen-select" DataTextField="Text" DataValueField="Value" Property="{ElementPasteModel.ElementId=SelectedValue}" />
    </div>
</div>
