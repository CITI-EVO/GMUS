<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormDataView.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataView" %>

<%@ Register TagPrefix="local" TagName="FormDataControl" Src="~/Controls/User/FormDataControl.ascx" %>
<%@ Register TagPrefix="local" TagName="CloneFormGridControl" Src="~/Controls/User/CloneFormGridControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="form-group">
                    <div class="col-lg-12">
                        <ce:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" OnClientClick="onSave()" />
                        <ce:LinkButton runat="server" ID="btnSubmit" OnClick="btnSubmit_OnClick" ToolTip="Submit" CssClass="btn btn-primary fa fa-share-square-o" />
                        <ce:LinkButton runat="server" ID="btnEdit" OnClick="btnEdit_OnClick" ToolTip="Edit" CssClass="btn btn-primary btn-sm fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                        <ce:LinkButton runat="server" ID="btnFiles" OnClick="btnFiles_OnClick" ToolTip="Files" CssClass="btn btn-danger fa fa-file" />

                        <asp:Label runat="server" ID="lblStatusDesc" ForeColor="Red" />
                    </div>
                </div>
                <asp:Panel runat="server" ID="pnlErrors" Style="padding: 5px;">
                    <asp:Repeater runat="server" ID="rptErrors">
                        <ItemTemplate>
                            <div>
                                <ce:Label runat="server" ForeColor="Red" Font-Size="10" Text='<%# Eval("Item") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <local:FormDataControl runat="server" ID="formDataControl" OnCommand="formDataControl_OnCommand" OnError="formDataControl_OnError" />
            </div>
        </div>
    </div>
    <div>
        <asp:Panel ID="pnlCloneDataGrid" runat="server" Style="display: none" DefaultButton="btnCloneDataGridOK">
            <asp:Button ID="btnCloneDataGridFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeCloneDataGrid" runat="server" PopupControlID="pnlCloneDataGrid" BackgroundCssClass="modalBackground" TargetControlID="btnCloneDataGridFake" />
            <div class="modal-dialog" style="width: 900px;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <h5>
                                <ce:Label runat="server" Text="Grids" />
                            </h5>
                        </div>
                        <div class="ibox-content">
                            <div class="form-group">
                                <ce:Label ID="lblCloneDataGrid" runat="server" ForeColor="Red"></ce:Label>
                            </div>
                            <local:CloneFormGridControl runat="server" ID="cloneFormGridControl" />
                            <div class="form-group">
                                <ce:LinkButton runat="server" ID="btnCloneDataGridOK" OnClick="btnCloneDataGridOK_Click" CssClass="btn btn-success fa fa-save" />
                                <ce:LinkButton runat="server" ID="btnCloneDataGridCancel" OnClick="btnCloneDataGridCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="pnlChangesWarning" runat="server" Style="display: none" DefaultButton="btnChangesWarningCancel">
            <asp:Button ID="btnChangesWarningFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeChangesWarning" runat="server" PopupControlID="pnlChangesWarning" BackgroundCssClass="modalBackground" TargetControlID="btnChangesWarningFake" />
            <div class="modal-dialog" style="width: 900px;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <h5>
                                <ce:Label runat="server" Text="Warning" />
                            </h5>
                        </div>
                        <div class="ibox-content">
                            <div class="form-group">
                                <asp:HiddenField runat="server" ID="hdReturnUrl" />
                                <asp:HiddenField runat="server" ID="hdRedirectUrl" />
                                <ce:Label ID="lblChangesWarning" runat="server" ForeColor="Red" Text="Do you want to save changes?"></ce:Label>
                            </div>
                            <div class="form-group">
                                <ce:LinkButton runat="server" ID="btnChangesWarningOK" OnClick="btnChangesWarningOK_Click" CssClass="btn btn-success fa fa-save" />
                                <ce:LinkButton runat="server" ID="btnChangesWarningCancel" OnClick="btnChangesWarningCancel_Click" CssClass="btn btn-warning fa fa-close" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="pnlNotifyMessage" runat="server" Style="display: none" DefaultButton="btnNotifyMessageCancel">
            <asp:Button ID="btnNotifyMessageFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeNotifyMessage" runat="server" PopupControlID="pnlNotifyMessage" BackgroundCssClass="modalBackground" TargetControlID="btnNotifyMessageFake" />
            <div class="modal-dialog" style="width: 900px;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <h5>
                                <ce:Label runat="server" Text="Warning" />
                            </h5>
                        </div>
                        <div class="ibox-content">
                            <div class="form-group">
                                <ce:Label ID="lblNotifyMessage" runat="server" ForeColor="Red" Text="Submit Required" />
                            </div>
                            <div class="form-group">
                                <%--<ce:LinkButton runat="server" ID="btnNotifyMessageOK" OnClick="btnNotifyMessageOK_Click" CssClass="btn btn-success fa fa-save" />--%>
                                <ce:LinkButton runat="server" ID="btnNotifyMessageCancel" OnClick="btnNotifyMessageCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

