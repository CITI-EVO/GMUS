<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogicData.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.LogicData" MasterPageFile="~/Site.master" %>

<%@ Register Src="~/Controls/Management/TableDataControl.ascx" TagPrefix="lmis" TagName="TableDataControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
	<lmis:TableDataControl runat="server" ID="tableDataControl" />
</asp:Content>
