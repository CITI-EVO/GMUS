<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormDataView.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataView" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="local" TagName="FormDataControl" Src="~/Controls/User/FormDataControl.ascx" %>
<%@ Register TagPrefix="local" TagName="CloneFormGridControl" Src="~/Controls/User/CloneFormGridControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ChooseTemplateControl" Src="~/Controls/User/ChooseTemplateControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:Label runat="server" ID="lblTimes"></asp:Label>
    <div class="row">
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="form-group">
                    <div class="col-lg-12">
                        <ce:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" OnClientClick="onSave()" />
                        <ce:LinkButton runat="server" ID="btnSubmit" OnClick="btnSubmit_OnClick" ToolTip="Submit" CssClass="btn btn-primary fa fa-share-square-o" />
                        <ce:LinkButton runat="server" ID="btnValidate" OnClick="btnValidate_OnClick" ToolTip="Validate" CssClass="btn btn-primary fa fa-recycle" />
                        <ce:LinkButton runat="server" ID="btnEdit" OnClick="btnEdit_OnClick" ToolTip="Edit" CssClass="btn btn-primary btn-sm fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnPrint" OnClick="btnPrint_OnClick" ToolTip="Pdf" CssClass="btn btn-primary fa fa-print" />
                        <ce:LinkButton runat="server" ID="btnNewHistory" OnClick="btnNewHistory_OnClick" ToolTip="Insert into History" CssClass="btn btn-primary fa fa-database" />
                        <ce:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                        <ce:LinkButton runat="server" ID="btnFiles" OnClick="btnFiles_OnClick" ToolTip="Files" CssClass="btn btn-danger fa fa-file" />
                        <asp:HyperLink runat="server" ID="lnkMonitoring" ToolTip="Monitoring"  CssClass="btn btn-success btn-sm fa fa-line-chart" />

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
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeCloneDataGrid" runat="server" Style="display: none" DefaultButton="btnCloneDataGridOK">
            <div class="modal-dialog" style="width: 900px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server" Text="Grids" />
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblCloneDataGrid" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <local:CloneFormGridControl runat="server" ID="cloneFormGridControl" />
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnCloneDataGridOK" OnClick="btnCloneDataGridOK_Click" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnCloneDataGridCancel" OnClick="btnCloneDataGridCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeRequiresSave" runat="server" Style="display: none" DefaultButton="btnRequiresSaveCancel">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server" Text="Warning" />
                        </h5>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField runat="server" ID="hdReturnUrl" />
                        <asp:HiddenField runat="server" ID="hdRedirectUrl" />
                        <ce:Label ID="lblRequiresSave" runat="server" ForeColor="Red" Text="Your data requires save, do you want to save data and continue?"></ce:Label>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnRequiresSaveOK" OnClick="btnRequiresSaveOK_Click" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnRequiresSaveCancel" OnClick="btnRequiresSaveCancel_Click" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeNotifyMessage" runat="server" Style="display: none" DefaultButton="btnNotifyMessageCancel">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server" Text="Warning" />
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblNotifyMessage" runat="server" ForeColor="Red" Text="Submit Required" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <%--<ce:LinkButton runat="server" ID="btnNotifyMessageOK" OnClick="btnNotifyMessageOK_Click" CssClass="btn btn-success fa fa-save" />--%>
                        <ce:LinkButton runat="server" ID="btnNotifyMessageCancel" OnClick="btnNotifyMessageCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" runat="server" ID="mpeChooseTemplate">
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
</asp:Content>

