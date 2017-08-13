<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FormDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataGrid" %>

<%@ Register TagPrefix="local" TagName="FormDataGridControl" Src="~/Controls/User/FormDataGridControl.ascx" %>
<%@ Register TagPrefix="local" TagName="RecordStatusControl" Src="~/Controls/User/RecordStatusControl.ascx" %>
<%@ Register TagPrefix="local" TagName="FormGridFilterControl" Src="~/Controls/User/FormGridFilterControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ChooseTemplateControl" Src="~/Controls/User/ChooseTemplateControl.ascx" %>

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
                <ce:Panel runat="server" ID="pnlNew" PermissionKey="Submit">
                    <div>
                        <ce:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" ToolTip="New" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="hr-line-dashed"></div>
                </ce:Panel>
                <div class="form-group">
                    <local:FormDataGridControl runat="server" ID="formDataGridControl" OnDelete="formDataGridControl_OnDelete" OnStatus="formDataGridControl_OnStatus" OnReview="formDataGridControl_OnReview" OnInspect="formDataGridControl_OnInspect" OnAssigne="formDataGridControl_OnAssigne" OnPrint="formDataGridControl_OnPrint" />
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
                        <ce:LinkButton runat="server" ID="btnRecordStatusOK" OnClick="btnRecordStatusOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnRecordStatusCancel" OnClick="btnRecordStatusCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeChooseTemplate" runat="server" Style="display: none" DefaultButton="btnTemplatesOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Template</h5>
                    </div>
                    <div class="modal-body">
                        <local:ChooseTemplateControl runat="server" ID="chooseTemplateControl" />
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnTemplatesOK" OnClick="btnChooseTemplateOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnTemplatesCancel" OnClick="btnChooseTemplateCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
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
                        <ce:LinkButton runat="server" ID="btnConfirmAcceptOK" OnClick="btnConfirmAcceptOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnConfirmAcceptCancel" OnClick="btnConfirmAcceptCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>
