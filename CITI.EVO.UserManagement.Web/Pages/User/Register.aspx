<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Register" %>

<%@ Register TagPrefix="local" TagName="RegisterUserControl" Src="~/Controls/RegisterUserControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <section id="login_page">
            <div class="main_contentregister">
                <asp:Panel runat="server" ID="pnlCompleted" Visible="False">
                    <ce:Label runat="server" ID="lblMessage" ForeColor="white" Font-Size="17px"></ce:Label>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlUserData">
                    <div>
                        <asp:Label runat="server" ForeColor="Red" ID="lblError" Font-Size="17px" Font-Bold="True"></asp:Label>
                    </div>
                    <h1>რეგისტრაციის ფორმა</h1>
                    <local:RegisterUserControl runat="server" ID="registerUserControl" />
                    <div class="box_body" style="text-align: center; display: inline-block; padding-top: 20px;">
                        <ce:ImageLinkButton CssClass="login_button" runat="server" ID="btnOK" Text="რეგისტრაცია" OnClick="btnOK_OnClick" />
                    </div>
                </asp:Panel>
            </div>
        </section>
    </form>
</body>
</html>
