<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecordScores.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.RecordScores" %>

<%@ Register TagPrefix="local" TagName="RecordScoresControl" Src="~/Controls/User/RecordScoresControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ToolTip="Save" ID="btnSave" OnClick="btnSave_OnClick" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ToolTip="Submit" ID="btnSubmit" OnClick="btnSubmit_OnClick" CssClass="btn btn-primary fa fa-check" />
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnCancel" OnClick="btnCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                            <ce:Label runat="server" ID="lblResult" Text="" ForeColor="Red" />
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="pnlRecordScores">
                        <local:RecordScoresControl runat="server" ID="recordScoresControl" />
                    </asp:Panel>
                </div>
            </div>
        </div>
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
                                <ce:Label runat="server" ID="lblConfirmText" ForeColor="Red" />
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
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

