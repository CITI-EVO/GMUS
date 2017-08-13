<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserExpertDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.UserExpertDataGrid" %>

<%@ Register TagPrefix="local" TagName="UserMessageControl" Src="~/Controls/User/UserMessageControl.ascx" %>
<%@ Register TagPrefix="local" TagName="UserExpertDataGridControl" Src="~/Controls/User/UserExpertDataGridControl.ascx" %>

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
                        <ce:DropDownList runat="server" ID="cbxForms" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" CssClass="chosen-select" AutoPostBack="True">
                        </ce:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-4">
                            <ce:LinkButton runat="server" ID="btnSearch" OnClick="btnSearch_OnClick" ToolTip="Search" CssClass="btn btn-primary fa fa-search" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="ibox float-e-margins">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <local:UserExpertDataGridControl runat="server" ID="userExpertDataGridControl" OnMessage="userExpertDataGridControl_OnMessage" OnAccept="userExpertDataGridControl_OnAccept" OnReject="userExpertDataGridControl_OnReject" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeUserMessage" runat="server" Style="display: none" DefaultButton="btnUserMessageOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Status</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblUserMessageErrors" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <local:UserMessageControl runat="server" ID="userMessageControl" OnDataChanged="userMessage_OnDataChanged" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnUserMessageOK" OnClick="btnUserMessageOK_Click" CssClass="btn btn-success fa fa-paper-plane" />
                        <ce:LinkButton runat="server" ID="btnUserMessageCancel" OnClick="btnUserMessageCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

