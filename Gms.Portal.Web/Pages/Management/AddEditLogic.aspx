<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditLogic.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditLogic" %>

<%@ Register Src="~/Controls/Management/LogicControl.ascx" TagPrefix="local" TagName="LogicControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:LinkButton runat="server" ID="btnSaveLogic" ToolTip="Save" OnClick="btnSaveLogic_OnClick" CssClass="btn btn-success fa fa-save"/>
                        <ce:LinkButton runat="server" ID="btnCancelLogic" ToolTip="Close" OnClick="btnCancelLogic_OnClick" CssClass="btn btn-warning fa fa-close"/>
                    </div>
                    <local:LogicControl runat="server" ID="logicControl" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlPreview">
            <asp:Button runat="server" ID="btnPreviewFake" Style="display: none" />
            <act:ModalPopupExtender runat="server" ID="mpePreview" TargetControlID="btnPreviewFake"
                Enabled="true" BackgroundCssClass="modalBackground" PopupControlID="pnlPreview"
                CancelControlID="btnCancel" />
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="ibox float-e-margins">
                                <div class="ibox-title">
                                    <h5>
                                        <ce:Label runat="server" Text="Data" />
                                    </h5>
                                </div>
                                <div class="ibox-content">
                                    <div class="form-group">
                                        <asp:Label ID="lblFormElement" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="True" Width="100%" EnableViewState="False">
                                        </asp:GridView>
                                    </div>
                                    <div class="form-group">
                                        <ce:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-warning fa fa-close" />
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

