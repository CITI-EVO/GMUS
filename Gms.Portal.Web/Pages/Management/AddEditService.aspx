<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditService.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditService" %>

<%@ Register Src="~/Controls/Management/ServiceControl.ascx" TagPrefix="local" TagName="ServiceControl" %>

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
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnRefresh" OnClick="btnRefresh_OnClick" CssClass="btn btn-warning fa fa-close" />
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnCancel" OnClick="btnCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                    <local:ServiceControl runat="server" ID="serviceControl" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

