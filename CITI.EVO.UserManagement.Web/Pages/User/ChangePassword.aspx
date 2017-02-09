<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.ChangePassword" %>

<%@ Register TagPrefix="local" TagName="ChangePassword" Src="~/Controls/ChangePassword.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" DefaultButton="btOK">
            <section id="login_page">
                <div class="logo">
                    <asp:Image runat="server" ImageUrl="~/App_Themes/default/images/l2.png" />
                </div>
                <div class="main_content">
                    <ce:Label ID="lstWarningMessages" runat="server" ForeColor="Red" Style="font-weight: bold;">
                    </ce:Label>
                    <ce:Label ID="lstErrorMessages" runat="server">
                    </ce:Label>
                    <local:ChangePassword runat="server" ID="changePassword" />
                    <div class="clear"></div>
                    <div class="wrapper"></div>
                    <ce:ImageLinkButton ID="btOK" CssClass="login_button"
                        Text="შეცვლა" runat="server" OnClick="btOK_Click" />
                    <div class="wrapper"></div>
                    <ce:ImageLinkButton ID="btCancel" CssClass="login_button"
                        Text="უკან დაბრუნება" runat="server" OnClick="btCancel_Click" />

                </div>
            </section>
        </asp:Panel>
    </form>
</body>
</html>
