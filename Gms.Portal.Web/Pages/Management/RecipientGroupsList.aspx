<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecipientGroupsList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.RecipientGroupList" %>

<%@ Register Src="~/Controls/Management/RecipientGroupsControl.ascx" TagPrefix="local" TagName="RecipientGroupsControl" %>
<%@ Register Src="~/Controls/Management/RecipientGroupControl.ascx" TagPrefix="local" TagName="RecipientGroupControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
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
        <asp:Panel ID="pnlRecipientGroup" runat="server" Style="display: none" DefaultButton="btnRecipientGroupOK">
            <asp:Button ID="btnRecipientGroupFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeRecipientGroup" runat="server" PopupControlID="pnlRecipientGroup" BackgroundCssClass="modalBackground" TargetControlID="btnRecipientGroupFake" CancelControlID="btnRecipientGroupCancel" />
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <h5>
                                <ce:Label runat="server" Text="Recipient Group" />
                            </h5>
                            <div class="ibox-content">
                                <div class="form-group">
                                    <ce:Label ID="lblRecipientGroup" runat="server" ForeColor="Red"></ce:Label>
                                </div>
                                <local:RecipientGroupControl runat="server" id="recipientGroupControl" />
                                <div class="form-group">
                                    <ce:LinkButton runat="server" ID="btnRecipientGroupOK" OnClick="btnRecipientGroupOK_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" />
                                    <ce:LinkButton runat="server" ID="btnRecipientGroupCancel" OnClick="btnRecipientGroupCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

</asp:Content>

