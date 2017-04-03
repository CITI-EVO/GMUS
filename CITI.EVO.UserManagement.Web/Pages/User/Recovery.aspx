<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Recovery.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Recovery" %>

<%@ Register TagPrefix="local" TagName="RecoveryControl" Src="~/Controls/RecoveryControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body class="gray-bg">
    <form id="form1" runat="server" style="height: 100%;">
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
            <asp:Panel runat="server" ID="pnlCompleted" Visible="False">
                <div class="form-group">
                    <ce:Label runat="server" ID="lblMessage" ForeColor="Green" Font-Size="15px">პაროლი წარმატებით შეიცვალა</ce:Label>
                </div>
                <div class="form-group">
                    <asp:HyperLink CssClass="btn btn-success block full-width m-b" runat="server" ID="lnkAuth" NavigateUrl="/Rnsf/Gms/Gms.Portal.Web/">
                        <ce:Label runat="server">გაგრძელება</ce:Label>
                    </asp:HyperLink>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlUserData" CssClass="row" Visible="True">
                <local:RecoveryControl runat="server" ID="recoveryControl" />
                <ce:LinkButton ID="btOK" CssClass="btn btn-success block full-width m-b" runat="server" OnClick="btnOK_OnClick">
                    <ce:label runat="server" Text="შეცვლა"/>
                </ce:LinkButton>
                <ce:Label runat="server" ForeColor="Red" ID="lblError"></ce:Label>
            </asp:Panel>
            <p class="m-t"><small>Consulting & IT Innovations  &copy; 2017</small> </p>
        </asp:Panel>
    </form>
</body>
</html>
