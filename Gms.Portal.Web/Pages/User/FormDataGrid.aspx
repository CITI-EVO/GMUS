<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FormDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataGrid" %>

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
                <div runat="server" id="dvNew">
                    <div>
                        <ce:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" ToolTip="New" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="hr-line-dashed"></div>
                </div>
                <div class="form-group">
                    <local:FormDataGridControl runat="server" ID="formDataGridControl" OnView="formDataGridControl_OnView" OnEdit="formDataGridControl_OnEdit" OnDelete="formDataGridControl_OnDelete" OnStatus="formDataGridControl_OnStatus" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <asp:Panel ID="pnlRecordStatus" runat="server" Style="display: none" DefaultButton="btRecordStatusOK">
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
                                        <ce:LinkButton runat="server" ID="btRecordStatusOK" OnClick="btnRecordStatusOK_Click" CssClass="btn btn-success fa fa-save" />
                                        <ce:LinkButton runat="server" ID="btRecordStatusCancel" CssClass="btn btn-warning fa fa-close" />
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
