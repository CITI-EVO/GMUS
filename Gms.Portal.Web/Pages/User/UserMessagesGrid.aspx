<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserMessagesGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.UserMessagesGrid" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>
<%@ Register TagPrefix="local" TagName="UserMessageControl" Src="~/Controls/User/UserMessageControl.ascx" %>
<%@ Register TagPrefix="local" TagName="MessageRejectControl" Src="~/Controls/User/MessageRejectControl.ascx" %>
<%@ Register TagPrefix="local" TagName="UserMessagesGridControl" Src="~/Controls/User/UserMessagesGridControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <div class="ibox float-e-margins">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <local:UserMessagesGridControl runat="server" ID="userMessagesGridControl" OnView="userMessagesGridControl_OnView" OnApprove="userMessagesGridControl_OnApprove" OnReject="userMessagesGridControl_OnReject" OnMarkAsRead="userMessagesGridControl_OnMarkAsRead" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeUserMessage" runat="server" Style="display: none" DefaultButton="btnUserMessageOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Status</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblUserMessageErrors" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:UserMessageControl runat="server" ID="userMessageControl" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnUserMessageOK" OnClick="btnUserMessageOK_Click" CssClass="btn btn-success fa fa-paper-plane" />
                        <ce:LinkButton runat="server" ID="btnUserMessageCancel" OnClick="btnUserMessageCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeRejectMessage" runat="server" Style="display: none" DefaultButton="btnRejectMessageOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Status</h5>
                    </div>
                    <div class="modal-body">
                        <local:MessageRejectControl runat="server" ID="messageRejectControl" />
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnRejectMessageOK" OnClick="btnRejectMessageOK_Click" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnRejectMessageCancel" OnClick="btnRejectMessageCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

