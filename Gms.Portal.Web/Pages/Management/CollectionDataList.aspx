<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CollectionDataList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.CollectionDataList" %>

<%@ Register Src="~/Controls/Management/CollectionDatasControl.ascx" TagPrefix="local" TagName="CollectionDatasControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <asp:Label runat="server" ID="lblTitle"></asp:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                        <ce:LinkButton runat="server" ID="btnClear" ToolTip="Clear" OnClick="btnClear_OnClick" CssClass="btn btn-danger fa fa-trash-o" />
                        <ce:LinkButton runat="server" ID="btnImport" ToolTip="Import" OnClick="btnImport_OnClick" CssClass="btn btn-info fa fa-upload" />
                        <ce:LinkButton runat="server" ID="btnClose" ToolTip="Cancel" OnClick="btnClose_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                    <div class="form-group">
                        <local:CollectionDatasControl runat="server" ID="collectionDatasControl" OnEdit="collectionDatasControl_OnEdit" OnDelete="collectionDatasControl_OnDelete" OnView="collectionDatasControl_OnView" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeImport" runat="server" Style="display: none" DefaultButton="btnImportOK">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>Import</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <asp:Label ID="lblImport" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <div>
                            <asp:FileUpload runat="server" ID="fuFile" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnImportOK" ToolTip="Save" OnClick="btnImportOK_Click" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnImportCancel" ToolTip="Close" OnClick="btnImportCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>


