<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormDataArchive.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataArchive" %>

<%@ Register TagPrefix="local" TagName="FormDataArchiveGridControl" Src="~/Controls/User/FormDataArchiveGridControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <div class="ibox float-e-margins">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <local:FormDataArchiveGridControl runat="server" id="formDataArchiveGridControl" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

