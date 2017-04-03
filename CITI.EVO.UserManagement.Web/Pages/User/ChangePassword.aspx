<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.ChangePassword" %>

<%@ Register TagPrefix="local" TagName="ChangePassword" Src="~/Controls/ChangePassword.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body class="gray-bg">
    <form id="form1" role="form" runat="server" style="height: 100%;">
        <asp:Panel runat="server" CssClass="middle-box text-center loginscreen animated fadeInDown" DefaultButton="btOK" Style="height: 100%;">
            <div>
                <asp:Image runat="server" ImageUrl="~/App_Themes/default/images/rust_logo.png" />
            </div>

            <div style="padding: 20px 20px;">
                <ce:HtmlLabel runat="server" Text="შოთა რუსთაველის ეროვნული სამეცნიერო ფონდი" />
            </div>
            <div style="padding: 20px 20px;">
                <asp:HyperLink runat="server" ID="btGeoLang" OnClick="btnGeo_OnClick">GEO</asp:HyperLink>|<asp:HyperLink runat="server" ID="btEngLang" OnClick="btnEng_OnClick">ENG</asp:HyperLink>
            </div>
            <local:ChangePassword runat="server" ID="changePassword" />

            <ce:LinkButton ID="btOK" CssClass="btn btn-success block full-width m-b" runat="server" OnClick="btOK_Click">
                <ce:label runat="server" Text="შეცვლა"/>
            </ce:LinkButton>

            <ce:LinkButton CssClass="btn btn-success block full-width m-b" runat="server" ID="btCancel" OnClick="btCancel_Click">
                <ce:label runat="server" Text="უკან დაბრუნება"/>
            </ce:LinkButton>

            <ce:Label ID="lstErrorMessages" ForeColor="Red" runat="server" />
        </asp:Panel>
    </form>
</body>
</html>
