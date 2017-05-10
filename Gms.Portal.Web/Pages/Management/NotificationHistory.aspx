<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NotificationHistory.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.NotificationHistory" %>

<%@ Register Src="~/Controls/Management/MailNotificationsHistoryControl.ascx" TagPrefix="ce" TagName="MailNotificationsHistoryControl" %>
<%@ Register Src="~/Controls/Management/PhoneNotificationsHistoryControl.ascx" TagPrefix="ce" TagName="PhoneNotificationsHistoryControl" %>





<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnClose" OnClick="btnClose_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>

                    <ul class="nav nav-pills">
                        <li class="active"><a data-toggle="pill" href="#mail">E-mail</a></li>
                        <li><a data-toggle="pill" href="#phone">Phone</a></li>
                    </ul>

                    <div class="tab-content">
                        <div id="mail" class="tab-pane fade in active">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="ibox float-e-margins">
                                        <div class="ibox-content">
                                            <div class="form-group">
                                                <ce:MailNotificationsHistoryControl runat="server" ID="mailNotificationsHistoryControl" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="phone" class="tab-pane fade">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="ibox float-e-margins">
                                        <div class="ibox-content">
                                            <div class="form-group">
                                                <ce:PhoneNotificationsHistoryControl runat="server" ID="phoneNotificationsHistoryControl" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

