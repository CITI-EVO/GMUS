<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhaseControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.PhaseControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{PhaseModel.ID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" Property="{PhaseModel.Name=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Start date</ce:Label>
    <div class="col-lg-10">
        <div class="input-group date">
            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
            <asp:TextBox runat="server" ID="tbxStartDate" Property="{PhaseModel.StartDate=Text}" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Start time</ce:Label>
    <div class="col-lg-10">
        <div class="input-group clockpicker">
            <asp:TextBox runat="server" ID="tbxStartTime" Property="{PhaseModel.StartTime=Text}" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">End date</ce:Label>
    <div class="col-lg-10">
        <div class="input-group date">
            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
            <asp:TextBox runat="server" ID="tbxEndDate" Property="{PhaseModel.EndDate=Text}" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">End time</ce:Label>
    <div class="col-lg-10">
        <div class="input-group clockpicker">
            <asp:TextBox runat="server" ID="tbxEndTime" Property="{PhaseModel.EndTime=Text}" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Color</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxColor" Property="{PhaseModel.Color=Text}" CssClass="form-control colorpicker colorpicker-element"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Form</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxForm" DataTextField="Name" DataValueField="ID" Property="{PhaseModel.FormID=SelectedValue}" CssClass="chosen-select" />
    </div>
</div>
