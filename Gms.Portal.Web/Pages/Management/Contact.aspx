<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" ValidateRequestMode="Disabled" Inherits="Gms.Portal.Web.Pages.Management.Contact" %>

<%@ Register Src="~/Controls/Management/MailContractControl.ascx" TagPrefix="ce" TagName="MailContractControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="col-lg-12">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server">Send Notification</ce:Label>
                    </h5>
                </div>
                <h2></h2>
            </div>
            <div class="ibox-content">
                <div class="form-group">
                    <ce:MailContractControl runat="server" ID="mailContractControl" />
                </div>
                <div class="form-group">
                    <div class="col-lg-1 control-label"></div>
                    <div class="col-lg-10 text-right">
                        <ce:LinkButton runat="server" ID="btnSend" OnClick="btnSend_OnClick" ToolTip="Send" CssClass="btn btn-sm btn-primary fa fa-send" Text="" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

