<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecordMonitoring.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.RecordMonitoring" %>

<%@ Register TagPrefix="local" TagName="MonitoringStatusControl" Src="~/Controls/User/Monitoring/MonitoringStatusControl.ascx" %>

<%@ Register TagPrefix="local" TagName="MonitoringBudgetDataControl" Src="~/Controls/User/Monitoring/MonitoringBudgetDataControl.ascx" %>
<%@ Register TagPrefix="local" TagName="MonitoringBudgetItemControl" Src="~/Controls/User/Monitoring/MonitoringBudgetItemControl.ascx" %>
<%@ Register TagPrefix="local" TagName="MonitoringBudgetGridsControl" Src="~/Controls/User/Monitoring/MonitoringBudgetGridsControl.ascx" %>
<%@ Register TagPrefix="local" TagName="MonitoringBudgetHistoryControl" Src="~/Controls/User/Monitoring/MonitoringBudgetHistoryControl.ascx" %>

<%@ Register TagPrefix="local" TagName="SummaryBudgetDataControl" Src="~/Controls/User/Monitoring/SummaryBudgetDataControl.ascx" %>

<%@ Register Src="~/Controls/User/Monitoring/MonitoringProjectsGridsControl.ascx" TagPrefix="local" TagName="MonitoringProjectsGridsControl" %>
<%@ Register Src="~/Controls/User/Monitoring/MonitoringProjectsDataControl.ascx" TagPrefix="local" TagName="MonitoringProjectsDataControl" %>
<%@ Register Src="~/Controls/User/Monitoring/MonitoringProjectFilesControl.ascx" TagPrefix="local" TagName="MonitoringProjectFilesControl" %>
<%@ Register Src="~/Controls/User/Monitoring/MonitoringProjectStatusControl.ascx" TagPrefix="local" TagName="MonitoringProjectStatusControl" %>
<%@ Register Src="~/Controls/User/Monitoring/MonitoringProjectFlawControl.ascx" TagPrefix="local" TagName="MonitoringProjectFlawControl" %>

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
                    <div class="form-group">
                        <ce:Label runat="server" ID="lblGlobalError" ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <ce:Label runat="server" CssClass="col-md-2 control-label">Period</ce:Label>
                        <div class="col-lg-4">
                            <ce:DropDownList runat="server" ID="cbxPeriods" DataTextField="Name" DataValueField="Value" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="cbxPeriods_OnSelectedIndexChanged">
                                <Items>
                                    <asp:ListItem Text="1" Value="1" />
                                    <asp:ListItem Text="2" Value="2" />
                                    <asp:ListItem Text="3" Value="3" />
                                    <asp:ListItem Text="4" Value="4" />
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="6" Value="6" />
                                </Items>
                            </ce:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <ce:Label runat="server" CssClass="col-md-2 control-label">Date</ce:Label>
                        <div class="col-lg-2">
                            <div class="input-group date">
                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                <asp:TextBox runat="server" ID="tbxStartDate" placeholder="Start date" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="input-group date">
                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                <asp:TextBox runat="server" ID="tbxEndDate" placeholder="End date" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-2">
                        </div>
                        <div class="col-lg-4">
                            <ce:LinkButton runat="server" ID="btnSearch" OnClick="btnSearch_OnClick" ToolTip="Search" CssClass="btn btn-primary fa fa-search" />
                            <ce:LinkButton runat="server" ToolTip="Print" ID="btnPrint" OnClick="btnPrint_OnClick" CssClass="btn btn-primary fa fa-print" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="tabs-container">
                    <ul class="nav nav-tabs">
                        <li>
                            <a data-toggle="tab" href="#tab-base" aria-expanded="true">Base</a>
                        </li>
                        <li>
                            <a data-toggle="tab" href="#tab-programs" aria-expanded="false">Program</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div id="tab-base" class="tab-pane active">
                            <div class="panel-body">
                                <div class="form-group">
                                    <ce:LinkButton runat="server" ToolTip="Submit" ID="btnBudgetSubmit" OnClick="btnBudgetSubmit_OnClick" CssClass="btn btn-success fa fa-send" />
                                    <ce:LinkButton runat="server" ToolTip="Approve" ID="btnBudgetApprove" OnClick="btnBudgetApprove_OnClick" CssClass="btn btn-success fa fa-check" />
                                </div>
                                <div class="form-group">
                                    <ce:Label runat="server" CssClass="col-md-2 control-label">Financing Type</ce:Label>
                                    <div class="col-lg-4">
                                        <ce:DropDownList runat="server" ID="cbxFinancingType" CssClass="chosen-select" AutoPostBack="True">
                                            <Items>
                                                <asp:ListItem Text="Select an Option" Value="" />
                                                <asp:ListItem Text="From the fund" Value="Primary" />
                                                <asp:ListItem Text="CoPayment" Value="CoPayment" />
                                            </Items>
                                        </ce:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <ce:Label runat="server" CssClass="col-md-2 control-label">Organization Type</ce:Label>
                                    <div class="col-lg-4">
                                        <ce:DropDownList runat="server" ID="cbxOrganizationType" CssClass="chosen-select" AutoPostBack="True">
                                            <Items>
                                                <asp:ListItem Text="Select an Option" Value="" />
                                                <asp:ListItem Text="Leading Organization" Value="Leading" />
                                                <asp:ListItem Text="Custodian Organization" Value="Custodian" />
                                            </Items>
                                        </ce:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <ce:Label runat="server" CssClass="col-md-2 control-label">Organization</ce:Label>
                                    <div class="col-lg-4">
                                        <ce:DropDownList runat="server" ID="cbxOrganizations" DataTextField="Name" DataValueField="Key" CssClass="chosen-select">
                                        </ce:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <div>
                                        <ce:LinkButton runat="server" ToolTip="Export to Excel" ID="btnMonitoringBudgetDataExport" OnClick="btnMonitoringBudgetDataExport_OnClick" CssClass="btn btn-primary fa fa-file-excel-o" />
                                        <ce:LinkButton runat="server" ToolTip="Export to Pdf" Visible="False" ID="btnMonitoringBudgetDataPdf" OnClick="btnMonitoringBudgetDataExport_OnClick" CssClass="btn btn-danger fa fa-file-pdf-o" />
                                    </div>
                                    <asp:Panel runat="server" ID="pnlMonitoringBudgetDataControl">
                                        <local:MonitoringBudgetDataControl runat="server" ID="monitoringBudgetDataControl"
                                            ShowFooter="True"
                                            OnSave="monitoringBudgetDataControl_OnSave"
                                            OnCancel="monitoringBudgetDataControl_OnCancel"
                                            OnStatus="monitoringBudgetDataControl_OnStatus"
                                            OnView="monitoringBudgetDataControl_OnView"
                                            OnEdit="monitoringBudgetDataControl_OnEdit"
                                            OnDelete="monitoringBudgetDataControl_OnDelete"
                                            OnFiles="monitoringBudgetDataControl_OnFiles"
                                            OnHistory="monitoringBudgetDataControl_OnHistory" />
                                    </asp:Panel>
                                    <div>
                                        <ce:Label runat="server" ID="lblMonitoringBudgetDataError" ForeColor="Red" />
                                    </div>
                                </div>
                                <div>
                                    <ce:LinkButton runat="server" ToolTip="Export to Excel" ID="btnSummaryBudgetExport" OnClick="btnSummaryBudgetExport_OnClick" CssClass="btn btn-primary fa fa-file-excel-o" />
                                    <ce:LinkButton runat="server" ToolTip="Export to Pdf" Visible="False" ID="btnSummaryBudgetExportPdf" OnClick="btnSummaryBudgetExport_OnClick" CssClass="btn btn-danger fa fa-file-pdf-o" />
                                    <local:SummaryBudgetDataControl runat="server" ID="summaryBudgetDataControl" />
                                </div>
                            </div>
                        </div>
                        <div id="tab-programs" class="tab-pane">
                            <div class="panel-body">
                                <div class="form-group">
                                    <ce:LinkButton runat="server" ToolTip="Submit" ID="btnProjectsSubmit" OnClick="btnProjectsSubmit_OnClick" CssClass="btn btn-success fa fa-send" />
                                    <ce:LinkButton runat="server" ToolTip="Approve" ID="btnProjectsApprove" OnClick="btnProjectsApprove_OnClick" CssClass="btn btn-success fa fa-check" />
                                </div>
                                <div class="form-group">
                                    <local:MonitoringProjectsGridsControl runat="server" ID="monitoringProjectsGridsControl" />
                                </div>
                                <div class="form-group">
                                    <div>
                                        <ce:LinkButton runat="server" ToolTip="Sync" ID="btnMonitoringProjectItemNew" OnClick="btnMonitoringProjectItemNew_OnClick" CssClass="btn btn-success fa fa-refresh" />
                                        <ce:LinkButton runat="server" ToolTip="Export to Excel" ID="btnMonitoringProjectExcel" OnClick="btnMonitoringProjectExcel_OnClick" CssClass="btn btn-primary fa fa-file-excel-o" />
                                        <ce:LinkButton runat="server" ToolTip="Export to Pdf" Visible="False" ID="btnMonitoringProjectPdf" OnClick="btnMonitoringProjectExcel_OnClick" CssClass="btn btn-danger fa fa-file-pdf-o" />
                                    </div>
                                    <asp:Panel runat="server" ID="pnlMonitoringProjectsDataControl">
                                        <local:MonitoringProjectsDataControl runat="server" ID="monitoringProjectsDataControl"
                                            OnUpload="monitoringProjectsDataControl_OnUpload"
                                            OnDelete="monitoringProjectsDataControl_OnDelete"
                                            OnFlaw="monitoringProjectsDataControl_OnFlaw"
                                            OnStatus="monitoringProjectsDataControl_OnStatus" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeMonitoringStatus" runat="server" Style="display: none" DefaultButton="btnMonitoringStatusOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Status</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblMonitoringStatusErrors" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:MonitoringStatusControl runat="server" ID="monitoringStatusControl" OnDataChanged="monitoringStatusControl_OnDataChanged" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnMonitoringStatusOK" OnClick="btnMonitoringStatusOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnMonitoringStatusCancel" OnClick="btnMonitoringStatusCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeMonitoringProjectFiles" runat="server" Style="display: none">
            <div class="modal-dialog">
                <div class="modal-content" style="width: 900px">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Files</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblFileResult" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:MonitoringProjectFilesControl runat="server" ID="monitoringProjectFilesControl" OnDelete="monitoringProjectFilesControl_OnDelete" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnMonitoringProjectFilesOK" OnClick="btnMonitoringProjectFilesOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnMonitoringProjectFilesCancel" OnClick="btnMonitoringProjectFilesCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeMonitoringProjectItemStatus" runat="server" Style="display: none" DefaultButton="btnMonitoringProjectStatusOk">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server">Status</ce:Label>
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="Label3" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:MonitoringProjectStatusControl runat="server" ID="monitoringProjectStatusControl" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnMonitoringProjectStatusOk" OnClick="btnMonitoringProjectStatusOk_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnMonitoringProjectStatusCancel" OnClick="btnMonitoringProjectStatusCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>

    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeMonitoringProjectFlawStatus" runat="server" Style="display: none" DefaultButton="btnMonitoringProjectFlawOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server">Status</ce:Label>
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblFlawResult" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:MonitoringProjectFlawControl runat="server" ID="monitoringProjectFlawControl" OnDataChanged="monitoringProjectFlawControl_OnDataChanged" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnMonitoringProjectFlawOK" OnClick="btnMonitoringProjectFlawOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnMonitoringProjectFlawCancel" OnClick="btnMonitoringProjectFlawCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>

    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpePrint" runat="server" Style="display: none" DefaultButton="btnSubmitPrint">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server">Print</ce:Label>
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblPrintError" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>

                            <div class="form-group">
                                <ce:Label runat="server" CssClass="col-lg-2 control-label">Flaw</ce:Label>
                                <div class="col-lg-10">
                                    <ce:DropDownList runat="server" ID="cbxPrintTemplates" CssClass="chosen-select" DataTextField="Name" DataValueField="ID" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnSubmitPrint" OnClick="btnSubmitPrint_OnClick" CssClass="btn btn-success fa fa-print" />
                        <ce:LinkButton runat="server" ID="btnPrintCancel" OnClick="btnPrintCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeBudgetApprove" runat="server" Style="display: none" DefaultButton="btnBudgetApproveOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Status</h5>
                    </div>
                    <div class="modal-body">
                        <div style="padding: 25px; text-align: center;">
                            <b>
                                <ce:Label runat="server" ID="lblBudgetApproveText" ForeColor="Red" Text="Are you sure you want to approve?" />
                            </b>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnBudgetApproveOK" OnClick="btnBudgetApproveOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnBudgetApproveCancel" OnClick="btnBudgetApproveCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeProjectApprove" runat="server" Style="display: none" DefaultButton="btnProjectApproveOK">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>Status</h5>
                </div>
                <div class="modal-body">
                    <div style="padding: 25px; text-align: center;">
                        <b>
                            <ce:Label runat="server" ID="lblProjectApproveText" ForeColor="Red" Text="Are you sure you want to approve?" />
                        </b>
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnProjectApproveOK" OnClick="btnBudgetApproveOK_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnProjectApproveCancel" OnClick="btnBudgetApproveCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeBudgetHistory" runat="server" Style="display: none" DefaultButton="btnBudgetHistoryCancel">
            <div class="modal-dialog" style="width: 90%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>History</h5>
                    </div>
                    <div class="modal-body">
                        <div>
                            <local:MonitoringBudgetHistoryControl runat="server" ID="monitoringBudgetHistoryControl" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnBudgetHistoryCancel" OnClick="btnBudgetHistoryCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

