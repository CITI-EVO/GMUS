<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Login" %>

<%@ Register TagPrefix="local" TagName="LoginControl" Src="~/Controls/LoginControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body class="gray-bg">
    <form id="form1" role="form" runat="server" style="height: 100%;">
        <asp:Panel runat="server" CssClass="middle-box text-center loginscreen animated fadeInDown" DefaultButton="btOK" Style="height: 100%;">
            <div style="vertical-align: middle;">
                <div>
                    <asp:Image runat="server" ImageUrl="~/App_Themes/default/images/rust_logo.png" />
                </div>

                <div style="padding: 20px 20px;">
                    <ce:HtmlLabel runat="server" Text="შოთა რუსთაველის ეროვნული სამეცნიერო ფონდი" />
                </div>
                <div style="padding: 20px 20px;">
                    <asp:HyperLink runat="server" ID="btGeoLang" OnClick="btnGeo_OnClick">GEO</asp:HyperLink>|<asp:HyperLink runat="server" ID="btEngLang" OnClick="btnEng_OnClick">ENG</asp:HyperLink>
                </div>
                <local:LoginControl runat="server" ID="loginControl" />


                <ce:LinkButton ID="btOK" CssClass="btn btn-success block full-width m-b" runat="server" OnClick="btOK_Click">
                        <ce:label runat="server" Text="ავტორიზაცია"/>
                </ce:LinkButton>


                <ce:LinkButton CssClass="btn btn-success block full-width m-b" runat="server" PostBackUrl="~/Pages/User/Register.aspx">
                        <ce:label runat="server" Text="რეგისტრაცია"/>
                </ce:LinkButton>

                <asp:Panel runat="server" ID="pnlRecovery" Visible="False">
                    <ce:LinkButton ID="btnRecovery" CssClass="btn btn-success block full-width m-b" runat="server" OnClick="btnRecovery_Click">
                        <ce:label runat="server" Text="პაროლის აღდგენა"/>
                    </ce:LinkButton>
                </asp:Panel>
                <ce:Label ID="lstErrorMessages" ForeColor="Red" runat="server" />
                <p class="m-t"><small>Consulting & IT Innovations  &copy; 2017</small> </p>
                          <%--<a href="http://evolution.ge" target="_blank">Evolution.ge</a>--%>
            </div>
        </asp:Panel>
    </form>
</body>
</html>

