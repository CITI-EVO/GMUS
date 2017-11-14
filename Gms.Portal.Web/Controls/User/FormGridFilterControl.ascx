<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormGridFilterControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormGridFilterControl" %>

<asp:Panel runat="server" ID="pnlMain" CssClass="row">
    <asp:Panel runat="server">
        <asp:Panel runat="server" class="form-group" ID="pnlStatus" Visible="False">
            <ce:Label runat="server" CssClass="col-md-1 control-label">Status</ce:Label>
            <div class="col-lg-4">
                <ce:DropDownList runat="server" ID="cbxStatuses" DataTextField="Name" DataValueField="ID" CssClass="chosen-select">
                </ce:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" class="form-group" ID="pnlDateCreated" Visible="False">
            <ce:Label runat="server" CssClass="col-md-1 control-label">Date</ce:Label>
            <div class="col-lg-2">
                <div class="input-group date">
                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                    <asp:TextBox runat="server" ID="tbxStartDate" placeholder="Start date" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-2">
                <div class="input-group date">
                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                    <asp:TextBox runat="server" ID="tbxEndDate" placeholder="End date" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlLeft" CssClass="col-lg-6">
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlRight" CssClass="col-lg-6">
    </asp:Panel>
</asp:Panel>

