<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecipientGroupsList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.RecipientGroupList" %>

<%@ Register Src="~/Controls/Management/RecipientGroupsControl.ascx" TagPrefix="local" TagName="RecipientGroupsControl" %>
<%@ Register Src="~/Controls/Management/RecipientGroupControl.ascx" TagPrefix="local" TagName="RecipientGroupControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server">Recipient Groups</ce:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="form-group">
                        <local:RecipientGroupsControl runat="server" ID="recipientGroupsControl" OnEdit="recipientGroupsControl_OnEdit" OnDelete="recipientGroupsControl_OnDelete" OnView="recipientGroupsControl_OnView" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeRecipientGroup" runat="server" Style="display: none" DefaultButton="btnRecipientGroupOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server" Text="Recipient Group" />
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblRecipientGroup" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <local:RecipientGroupControl runat="server" ID="recipientGroupControl" />
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnRecipientGroupOK" OnClick="btnRecipientGroupOK_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnRecipientGroupCancel" OnClick="btnRecipientGroupCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>

