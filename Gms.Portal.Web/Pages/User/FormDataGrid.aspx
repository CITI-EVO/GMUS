<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FormDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataGrid" %>

<%@ Register TagPrefix="local" TagName="AssigneUsersControl" Src="~/Controls/User/AssigneUsersControl.ascx" %>
<%@ Register TagPrefix="local" TagName="FormDataGridControl" Src="~/Controls/User/FormDataGridControl.ascx" %>
<%@ Register TagPrefix="local" TagName="RecordStatusControl" Src="~/Controls/User/RecordStatusControl.ascx" %>
<%@ Register TagPrefix="local" TagName="FormGridFilterControl" Src="~/Controls/User/FormGridFilterControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <div class="ibox float-e-margins">
            <div class="ibox-title" style="background-color: #4d608a;">
                <h5 style="color: white;">
                    <ce:Label runat="server">Filters</ce:Label>
                </h5>
                <div class="ibox-tools">
                    <a class="collapse-link">
                        <i class="fa fa-chevron-up" style="color: white;"></i>
                    </a>
                </div>
            </div>
            <div class="ibox-content">
                <div>
                    <local:FormGridFilterControl runat="server" ID="formGridFilterControl" />
                    <div class="form-group">
                        <div class="col-lg-4">
                            <ce:LinkButton runat="server" ID="btnSearch" OnClick="btnSearch_OnClick" ToolTip="Search" CssClass="btn btn-primary fa fa-search" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <ce:Panel runat="server" id="pnlNew" PermissionKey="Submit">
                    <div>
                        <ce:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" ToolTip="New" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="hr-line-dashed"></div>
                </ce:Panel>
                <div class="form-group">
                    <local:FormDataGridControl runat="server" ID="formDataGridControl" OnView="formDataGridControl_OnView" OnEdit="formDataGridControl_OnEdit" OnDelete="formDataGridControl_OnDelete" OnStatus="formDataGridControl_OnStatus" OnReview="formDataGridControl_OnReview" OnInspect="formDataGridControl_OnInspect" OnAssigne="formDataGridControl_OnAssigne" OnPrint="formDataGridControl_OnPrint" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <asp:Panel ID="pnlAssigneUser" runat="server" Style="display: none" DefaultButton="btnAssigneUserOK">
            <asp:Button ID="btnAssigneUserFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeAssigneUser" runat="server" PopupControlID="pnlAssigneUser" BackgroundCssClass="modalBackground" TargetControlID="btnAssigneUserFake" />
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="ibox float-e-margins">
                                <div class="ibox-title">
                                    <h5>Status</h5>
                                </div>
                                <div class="ibox-content">
                                    <div class="form-group">
                                        <ce:Label ID="lblAssigneUser" runat="server" ForeColor="Red"></ce:Label>
                                    </div>
                                    <div>
                                        <local:AssigneUsersControl runat="server" ID="assigneUsersControl" OnDataChanged="assigneUsersControl_OnDataChanged" />
                                    </div>
                                    <div class="form-group">
                                        <ce:LinkButton runat="server" ID="btnAssigneUserOK" OnClick="btnAssigneUserOK_Click" CssClass="btn btn-success fa fa-save" />
                                        <ce:LinkButton runat="server" ID="btnAssigneUserCancel" OnClick="btnAssigneUserCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="pnlRecordStatus" runat="server" Style="display: none" DefaultButton="btnRecordStatusOK">
            <asp:Button ID="btnRecordStatusFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeRecordStatus" runat="server" PopupControlID="pnlRecordStatus" BackgroundCssClass="modalBackground" TargetControlID="btnRecordStatusFake" />
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="ibox float-e-margins">
                                <div class="ibox-title">
                                    <h5>Status</h5>
                                </div>
                                <div class="ibox-content">
                                    <div class="form-group">
                                        <ce:Label ID="lblRecordStatus" runat="server" ForeColor="Red"></ce:Label>
                                    </div>
                                    <div>
                                        <local:RecordStatusControl runat="server" ID="recordStatusControl" OnDataChanged="recordStatusControl_OnDataChanged" />
                                    </div>
                                    <div class="form-group">
                                        <ce:LinkButton runat="server" ID="btnRecordStatusOK" OnClick="btnRecordStatusOK_Click" CssClass="btn btn-success fa fa-save" />
                                        <ce:LinkButton runat="server" ID="btnRecordStatusCancel" OnClick="btnRecordStatusCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="pnlConfirmAccept" runat="server" Style="display: none" DefaultButton="btnConfirmAcceptOK">
            <asp:Button ID="btnConfirmAcceptFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeConfirmAccept" runat="server" PopupControlID="pnlConfirmAccept" BackgroundCssClass="modalBackground" TargetControlID="btnConfirmAcceptFake" />
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="ibox float-e-margins">
                                <div class="ibox-title">
                                    <h5>Status</h5>
                                </div>
                                <div class="ibox-content">
                                    <div style="padding: 25px; text-align: center;">
                                        <b>
                                            <ce:Label runat="server" ID="lblConfirmText" Text="Are you sure you want to approve data?" />
                                        </b>
                                    </div>
                                    <div class="form-group">
                                        <ce:LinkButton runat="server" ID="btnConfirmAcceptOK" OnClick="btnConfirmAcceptOK_Click" CssClass="btn btn-success fa fa-save" />
                                        <ce:LinkButton runat="server" ID="btnConfirmAcceptCancel" OnClick="btnConfirmAcceptCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
