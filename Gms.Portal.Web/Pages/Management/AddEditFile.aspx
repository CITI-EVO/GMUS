﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditFile.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditFile" %>

<%@ Register Src="~/Controls/Management/FilesControl.ascx" TagPrefix="ce" TagName="FilesControl" %>
<%@ Register Src="~/Controls/Management/FileControl.ascx" TagPrefix="ce" TagName="FileControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ToolTip="New" ID="btnNew" OnClick="btnNew_OnClick" CssClass="btn btn-success fa fa-plus" />
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnClose" OnClick="btnClose_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                    <ce:FilesControl runat="server" ID="filesControl" OnEdit="filesControl_OnEdit" OnDelete="filesControl_OnDelete" OnDownload="filesControl_OnDownload" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeFile" runat="server" Style="display: none" DefaultButton="btnFileOK">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server" Text="File" />
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <ce:Label ID="lblFile" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <ce:FileControl runat="server" ID="fileControl" />
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnFileOK" OnClick="btnFileOK_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnFileCancel" OnClick="btnFileCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>

</asp:Content>

