<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FormDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataGrid" %>

<%@ Register TagPrefix="local" TagName="FormDataGridControl" Src="~/Controls/User/FormDataGridControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="wrapper wrapper-content animated fadeInRight">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <asp:Label runat="server"></asp:Label>
                    </h5>
                    <div class="btn-group">
                        <ce:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" ToolTip="New" CssClass="btn btn-primary fa fa-plus"/>
                    </div>
                </div>
                <div class="ibox-content">
                    <local:FormDataGridControl runat="server" ID="formDataGridControl" OnView="formDataGridControl_OnView" OnEdit="formDataGridControl_OnEdit" OnDelete="formDataGridControl_OnDelete" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
