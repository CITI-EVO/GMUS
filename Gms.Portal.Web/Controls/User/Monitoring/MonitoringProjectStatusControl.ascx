<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringProjectStatusControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringProjectItemControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{MonitoringProjectStatusItemModel.ID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Status</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxStatus" Property="{MonitoringProjectStatusItemModel.DoneStatus=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Select an option"></asp:ListItem>
                <asp:ListItem Text="შესრულებული" Value="შესრულებული"></asp:ListItem>
                <asp:ListItem Text="შეუსრულებული" Value="შეუსრულებული"></asp:ListItem>
            </Items>
        </ce:DropDownList>
    </div>
</div>


<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Description</ce:Label>
    <div class="col-lg-10">
        <ce:TextBox runat="server" Property="{MonitoringProjectStatusItemModel.Description=Text}" TextMode="MultiLine" Rows="4" CssClass="form-control" />
    </div>
</div>

