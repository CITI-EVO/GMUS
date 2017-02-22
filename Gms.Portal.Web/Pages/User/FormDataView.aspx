<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormDataView.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataView" %>

<%@ Register TagPrefix="local" TagName="FormDataControl" Src="~/Controls/User/FormDataControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="form-group">
                    <asp:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" CssClass="btn btn-success">
                        <asp:Label runat="server" CssClass="fa fa-save"></asp:Label>
                    </asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_OnClick" CssClass="btn btn-warning">
                        <asp:Label runat="server" CssClass="fa fa-close"></asp:Label>
                    </asp:LinkButton>
                </div>
                <local:FormDataControl runat="server" ID="formDataControl" OnCommand="formDataControl_OnCommand" />
            </div>
        </div>
    </div>
</asp:Content>

