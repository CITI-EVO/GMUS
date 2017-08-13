<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringItemControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringItemControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{MonitoringItemModel.ID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Budget</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxBudget" DataTextField="Name" DataValueField="ID" Property="{MonitoringItemModel.BudgetID=SelectedValue}" CssClass="chosen-select" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Goal</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxGoals" DataTextField="Name" DataValueField="ID" Property="{MonitoringItemModel.GoalID=SelectedValue}" CssClass="chosen-select" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Type</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxType" Property="{MonitoringItemModel.Type=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Incoming" Value="Incoming" />
                <asp:ListItem Text="Outgoing" Value="Outgoing" />
            </Items>
        </ce:DropDownList>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Amount</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxAmount" Property="{MonitoringItemModel.Amount=Text}" CssClass="form-control" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Date Of Transfer</ce:Label>
    <div class="col-lg-10">
        <div class="input-group date">
            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
            <asp:TextBox runat="server" ID="tbxDateOfTransfer" Property="{MonitoringItemModel.DateOfTransfer=Text}" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
</div>
