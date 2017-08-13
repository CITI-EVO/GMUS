<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FullApproveDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FullApproveDataGrid" %>

<%@ Register TagPrefix="local" TagName="RecordStatusControl" Src="~/Controls/User/RecordStatusControl.ascx" %>
<%@ Register TagPrefix="local" TagName="FullApproveDataGridControl" Src="~/Controls/User/FullApproveDataGridControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <div class="ibox float-e-margins">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <local:FullApproveDataGridControl runat="server" ID="fullApproveDataGridControl" OnAccept="fullApproveDataGridControl_OnAccept" OnReject="fullApproveDataGridControl_OnReject" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

