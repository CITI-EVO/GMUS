<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LogicsList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.LogicsList" %>

<%@ Register Src="~/Controls/Management/LogicsControl.ascx" TagPrefix="lmis" TagName="LogicsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div style="text-align: left;">
        <ce:ImageLinkButton runat="server" ToolTip="Add Logic" DefaultImageUrl="~/App_Themes/Default/images/add.png" ID="btnAddNew" OnClick="btnAddLogic_OnClick" />
    </div>
    <div>
        <lmis:LogicsControl runat="server" ID="logicsControl" OnEditItem="logicsControl_OnEditItem" OnDeleteItem="logicsControl_OnDeleteItem" OnViewItem="logicsControl_OnViewItem" />
    </div>
</asp:Content>

