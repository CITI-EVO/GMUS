<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AssigneExperts.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.AssigneExperts" %>

<%@ Register TagPrefix="local" TagName="RecordStatusControl" Src="~/Controls/User/RecordStatusControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AssigneExpertsControl" Src="~/Controls/User/AssigneExpertsControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AssigneExpertsFilterControl" Src="~/Controls/User/AssigneExpertsFilterControl.ascx" %>

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
                        <asp:Label runat="server" ID="lblError" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="form-group">
                        <local:AssigneExpertsFilterControl runat="server" ID="assigneExpertsFilterControl" />
                    </div>
                    <div class="form-group">
                        <div class="col-lg-4">
                            <ce:LinkButton runat="server" ID="btnSearch" OnClick="btnSearch_OnClick" ToolTip="Search" CssClass="btn btn-primary fa fa-search" />
                            <ce:LinkButton runat="server" ID="btnRandom" OnClick="btnRandom_OnClick" ToolTip="Random" CssClass="btn btn-primary fa fa-ravelry" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="form-group">
                    <local:AssigneExpertsControl runat="server" ID="assigneExpertsControl" OnStatus="assigneExpertsControl_OnStatus" OnEmail="assigneExpertsControl_OnEmail" OnAttach="assigneExpertsControl_OnAttach" OnDetach="assigneExpertsControl_OnDetach" />
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
                            <local:RecordStatusControl runat="server" ID="recordStatusControl" />
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
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeRandomExperts" runat="server" Style="display: none" DefaultButton="btnRecordStatusOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Select Experts</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="col-lg-11">
                                <ce:Label runat="server" CssClass="col-lg-1 control-label">Count</ce:Label>
                                <div class="col-lg-10">
                                    <asp:TextBox runat="server" ID="seExpertsCount" CssClass="intSpinEdit" />
                                </div>
                            </div>
                            <div class="col-lg-1">
                                <ce:LinkButton runat="server" ID="btnRefreshExpertsOK" OnClick="btnRefreshExpertsOK_OnClick" CssClass="btn btn-success fa fa-refresh" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:HiddenField runat="server" ID="hdnSelectedExperts" />
                            <asp:ListBox runat="server" ID="lstSelectedEsperts" DataValueField="ID" DataTextField="Name" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnRandomExpertsOK" OnClick="btnRandomExpertsOK_OnClick" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnRandomExpertsCancel" OnClick="btnRandomExpertsCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

