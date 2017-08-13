<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChooseTemplateControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.ChooseTemplateControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdRecordID" Property="{ChooseTemplateModel.RecordID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Template</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxTemplates" CssClass="chosen-select" DataTextField="Name" DataValueField="ID" Property="{ChooseTemplateModel.TemplateID=SelectedValue}">
        </asp:DropDownList>
    </div>
</div>