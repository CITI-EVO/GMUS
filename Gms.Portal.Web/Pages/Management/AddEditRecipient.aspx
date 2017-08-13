<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditRecipient.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditRecipient" %>

<%@ Register Src="~/Controls/Management/RecipientControl.ascx" TagPrefix="ce" TagName="RecipientControl" %>
<%@ Register Src="~/Controls/Management/RecipientsControl.ascx" TagPrefix="ce" TagName="RecipientsControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server">Recipients</ce:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnCancel" OnClick="btnCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                            <span style="padding-left: 10px">
                                <ce:Label runat="server">Description:</ce:Label>
                            </span>
                            <span style="padding-left: 10px">
                                <ce:Label runat="server" ID="lblDescription"></ce:Label>
                            </span>
                        </div>
                    </div>

                    <div class="form-group">
                        <ce:RecipientsControl runat="server" ID="recipientsControl" OnView="recipientsControl_OnView" OnDelete="recipientsControl_OnDelete" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeRecipient" runat="server" Style="display: none" DefaultButton="btnRecipientOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server" Text="Recipient" />
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblRecipient" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <ce:RecipientControl runat="server" ID="recipientControl" />
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnRecipientOK" OnClick="btnRecipientOK_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnRecipientCancel" OnClick="btnRecipientCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>

