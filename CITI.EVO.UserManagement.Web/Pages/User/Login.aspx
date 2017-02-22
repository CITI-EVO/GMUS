<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Login" %>

<%@ Register TagPrefix="local" TagName="LoginControl" Src="~/Controls/LoginControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body class="gray-bg">
    <form id="form1" class="m-t" role="form" runat="server">




        <asp:Panel runat="server" DefaultButton="btOK">
            <div class="middle-box text-center loginscreen animated fadeInDown">
                <div>
                    <div>

                        <h1 class="logo-name">CITI</h1>

                    </div>

                    <p>
                       Consulting and IT Innovations
                    </p>
                    <p>სისტემა მუშაობს სატესტო რეჟიმში</p>

                    <local:LoginControl runat="server" ID="loginControl" />


                    <ce:ImageLinkButton ID="btOK" class="btn btn-primary block full-width m-b" Text="შესვლა" runat="server" OnClick="btOK_Click" />


                    <p class="text-muted text-center"><small>Do not have an account?</small></p>
                    <div class="btn btn-sm btn-white btn-block">
                        <ce:ImageLinkButton class="btn btn-sm btn-white btn-block" runat="server" Text="რეგისტრაცია" NavigateUrl="~/Pages/User/Register.aspx" />
                    </div>

                    <a href="#"><small>დაგავიწყდათ პაროლი?</small></a>

                    <asp:Panel runat="server" CssClass="wrapper" ID="pnlRecovery" Visible="False">
                        <ce:ImageLinkButton ID="btnRecovery" class="btn btn-primary block full-width m-b" Text="პაროლის აღდგენა" runat="server" OnClick="btnRecovery_Click" />
                    </asp:Panel>
                    <ce:Label ID="lstErrorMessages" ForeColor="white" runat="server" />
                    <asp:Panel runat="server" ID="pnlWarning" Visible="False">
                        <ce:Label ID="lstWarningMessages" runat="server" ForeColor="Red" Style="font-weight: bold;" />
                        <asp:LinkButton runat="server" ID="btnGoToLicPage" OnClick="btnGoToLicPage_OnClick" Text="ლიცენზიის ატვირთვა"></asp:LinkButton>
                    </asp:Panel>

                    <p class="m-t"><small>Consulting & IT Innovations  &copy; 2017</small> </p>
                </div>
            </div>
        </asp:Panel>
    </form>
</body>
</html>

