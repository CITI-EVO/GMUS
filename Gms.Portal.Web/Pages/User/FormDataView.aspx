<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormDataView.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataView" %>

<%@ Register TagPrefix="local" TagName="FormDataControl" Src="~/Controls/User/FormDataControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="form-group">
                    <ce:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save"/>
                    <ce:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close"/>
                </div>
                <div class="form-group">
                    <ce:Label runat="server" ID="lblErrorMessage" />
                </div>
                <local:FormDataControl runat="server" ID="formDataControl" OnCommand="formDataControl_OnCommand" />
            </div>
        </div>
    </div>
</asp:Content>

