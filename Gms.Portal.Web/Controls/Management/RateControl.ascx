<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RateControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.RateControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{RateModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{RateModel.ParentID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Number</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxNumber" Property="{RateModel.Number=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" Property="{RateModel.Name=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Min</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxMinScore" Property="{RateModel.MinScore=Text}" CssClass="intSpinEdit"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Max</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxMaxScore" Property="{RateModel.MaxScore=Text}" CssClass="intSpinEdit"></asp:TextBox>
    </div>
</div>