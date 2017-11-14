<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringProjectFlawControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringProjectFlawControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{MonitoringProjectFlawItemModel.ID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Status</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxStatus" DataTextField="Name" DataValueField="ID" Property="{MonitoringProjectFlawItemModel.Status=SelectedValue}" CssClass="chosen-select" AutoPostBack="True">
            <Items>
                <asp:ListItem Text="Select an Option" Value="" />
                <asp:ListItem Text="დადასტურებული" Value="დადასტურებული" />
                <asp:ListItem Text="ხარვეზიანი" Value="ხარვეზიანი" />
            </Items>
        </ce:DropDownList>
    </div>
</div>
<asp:Panel runat="server" CssClass="form-group" ID="pnlFlaw">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Flaw</ce:Label>
    <div class="col-lg-10">
        <ce:ListBox SelectionMode="Multiple" runat="server" ID="cbxFlaw" DataTextField="Name" DataValueField="ID" CssClass="chosen-select">
        </ce:ListBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" CssClass="form-group" ID="pnlExpireDate">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Expire date</ce:Label>
    <div class="col-lg-10">
        <div class="input-group date">
            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
            <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control" Property="{MonitoringProjectFlawItemModel.ExpireDate=Text}"></asp:TextBox>
        </div>
    </div>
</asp:Panel>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Comment</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxComment" Property="{MonitoringProjectFlawItemModel.Comment=Text}" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
    </div>
</div>
