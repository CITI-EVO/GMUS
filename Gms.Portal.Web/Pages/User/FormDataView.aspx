<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormDataView.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataView" %>

<%@ Register TagPrefix="local" TagName="FormDataControl" Src="~/Controls/User/FormDataControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <asp:LinkButton runat="server" Text="Save" ID="btnSave" OnClick="btnSave_OnClick"/>
        <asp:LinkButton runat="server" Text="Cancel" ID="btnCancel" OnClick="btnCancel_OnClick"/>
    </div>
    <div>
        <local:FormDataControl runat="server" ID="formDataControl" OnCommand="formDataControl_OnCommand" />
    </div>
</asp:Content>

