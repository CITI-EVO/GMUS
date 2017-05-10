<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DataApproveGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.DataApproveGrid" %>

<%@ Register TagPrefix="local" TagName="DataApproveGridFilterControl" Src="~/Controls/User/DataApproveGridFilterControl.ascx" %>
<%@ Register TagPrefix="local" TagName="DataApproveGridControl" Src="~/Controls/User/DataApproveGridControl.ascx" %>
<%@ Register TagPrefix="local" TagName="RecordStatusControl" Src="~/Controls/User/RecordStatusControl.ascx" %>

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
                    <local:DataApproveGridFilterControl runat="server" id="dataApproveGridFilterControl" />
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
                <div class="form-group">
                    <local:DataApproveGridControl runat="server" id="dataApproveGridControl" onview="dataApproveGridControl_OnView" onstatus="dataApproveGridControl_OnStatus" OnPrint="dataApproveGridControl_OnPrint" />
                </div>
            </div>
        </div>
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
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

