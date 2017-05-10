<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FilesList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.FilesList" %>

<%@ Register Src="~/Controls/Management/FilesDownloadControl.ascx" TagPrefix="ce" TagName="FilesDownloadControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnClose" OnClick="btnClose_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                    <ce:FilesDownloadControl runat="server" ID="filesDownloadControl" OnDownload="filesDownloadControl_OnDownload" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

