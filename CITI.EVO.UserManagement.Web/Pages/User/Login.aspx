<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Login" %>

<%@ Register TagPrefix="local" TagName="LoginControl" Src="~/Controls/LoginControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" DefaultButton="btOK">
            <section id="login_page">
                <div class="main_content">
                    <div style="font-size: 18px; color: Red; font-weight: bold;">
                        სისტემა მუშაობს სატესტო რეჟიმში
                    </div>
                    <local:LoginControl runat="server" ID="loginControl" />

                    <div class="clear"></div>
                    <div class="wrapper">
                        <ce:ImageLinkButton ID="btOK" CssClass="login_button" Text="შესვლა" runat="server" OnClick="btOK_Click" />
                    </div>
                    <div class="wrapper">
                        <div class="login_button1">
                            <ce:ImageLinkButton CssClass="login_button" runat="server" Text="რეგისტრაცია" NavigateUrl="~/Pages/User/Register.aspx" />
                        </div>
                    </div>
                    <asp:Panel runat="server" CssClass="wrapper" ID="pnlRecovery" Visible="False">
                        <ce:ImageLinkButton ID="btnRecovery" CssClass="login_button" Text="პაროლის აღდგენა" runat="server" OnClick="btnRecovery_Click" />
                    </asp:Panel>
                    <ce:Label ID="lstErrorMessages" ForeColor="white" runat="server" />
                    <asp:Panel runat="server" ID="pnlWarning" Visible="False">
                        <ce:Label ID="lstWarningMessages" runat="server" ForeColor="Red" Style="font-weight: bold;" />
                        <asp:LinkButton runat="server" ID="btnGoToLicPage" OnClick="btnGoToLicPage_OnClick" Text="ლიცენზიის ატვირთვა"></asp:LinkButton>
                    </asp:Panel>
                </div>
            </section>
        </asp:Panel>
    </form>
</body>
</html>

