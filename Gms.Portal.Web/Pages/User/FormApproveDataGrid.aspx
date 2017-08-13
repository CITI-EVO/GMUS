<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormApproveDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormApproveDataGrid" %>

<%@ Register TagPrefix="local" TagName="FormApproveDataGridFilterControl" Src="~/Controls/User/FormApproveDataGridFilterControl.ascx" %>
<%@ Register TagPrefix="local" TagName="FormApproveDataGridControl" Src="~/Controls/User/FormApproveDataGridControl.ascx" %>
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
                    <local:FormApproveDataGridFilterControl runat="server" ID="formApproveDataGridFilterControl" />
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
                    <local:FormApproveDataGridControl runat="server" ID="formApproveDataGridControl" OnStatus="formApproveDataGridControl_OnStatus" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeRecordStatus" runat="server" Style="display: none" DefaultButton="btnRecordStatusOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Status</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblRecordStatus" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:RecordStatusControl runat="server" ID="recordStatusControl" OnDataChanged="recordStatusControl_OnDataChanged" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnRecordStatusOK" OnClick="btnRecordStatusOK_Click" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnRecordStatusCancel" OnClick="btnRecordStatusCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeConfirmAccept" runat="server" Style="display: none" DefaultButton="btnConfirmAcceptOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Status</h5>
                    </div>
                    <div class="modal-body">
                        <div style="padding: 25px; text-align: center;">
                            <b>
                                <ce:Label runat="server" ID="lblConfirmText" Text="Are you sure you want to approve data?" />
                            </b>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnConfirmAcceptOK" OnClick="btnConfirmAcceptOK_Click" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnConfirmAcceptCancel" OnClick="btnConfirmAcceptCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

