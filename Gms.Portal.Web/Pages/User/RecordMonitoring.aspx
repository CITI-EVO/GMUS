<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecordMonitoring.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.RecordMonitoring" %>

<%@ Register TagPrefix="local" TagName="MonitoringItemControl" Src="~/Controls/User/Monitoring/MonitoringItemControl.ascx" %>
<%@ Register TagPrefix="local" TagName="MonitoringDataGridsControl" Src="~/Controls/User/Monitoring/MonitoringDataGridsControl.ascx" %>
<%@ Register TagPrefix="local" TagName="MonitoringBudgetDataControl" Src="~/Controls/User/Monitoring/MonitoringBudgetDataControl.ascx" %>
<%@ Register TagPrefix="local" TagName="SummaryBudgetDataControl" Src="~/Controls/User/Monitoring/SummaryBudgetDataControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <div class="ibox float-e-margins">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <local:MonitoringDataGridsControl runat="server" ID="monitoringDataGridsControl" />
                    </div>
                    <div class="form-group">
                        <div>
                            <ce:LinkButton runat="server" ID="btnMonitoringItemNew" OnClick="btnMonitoringItemNew_OnClick" CssClass="btn btn-success fa fa-plus" />
                        </div>
                        <div>
                            <local:MonitoringBudgetDataControl runat="server" ID="monitoringBudgetDataControl"
                                OnAccept="monitoringBudgetDataControl_OnAccept"
                                OnReturn="monitoringBudgetDataControl_OnReturn"
                                OnView="monitoringBudgetDataControl_OnView"
                                OnEdit="monitoringBudgetDataControl_OnEdit"
                                OnDelete="monitoringBudgetDataControl_OnDelete" />
                        </div>
                    </div>
                    <div>
                        <local:SummaryBudgetDataControl runat="server" ID="summaryBudgetDataControl" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeMonitoringItem" runat="server" Style="display: none" DefaultButton="btnMonitoringItemOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Monitoring</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblUserMessageErrors" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:MonitoringItemControl runat="server" ID="monitoringItemControl" OnDataChanged="monitoringItemControl_OnDataChanged" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnMonitoringItemOK" OnClick="btnMonitoringItemOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnMonitoringItemCancel" OnClick="btnMonitoringItemCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

