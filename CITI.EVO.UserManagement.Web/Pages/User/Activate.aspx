<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Activate.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Activate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body class="gray-bg">
    <form id="form1" runat="server" style="height: 100%;">
        <div class="middle-box text-center loginscreen animated fadeInDown">
            <div class="row" style="text-align: center;">
                <div>
                    <asp:Image runat="server" ImageUrl="~/App_Themes/default/images/rust_logo.png" />
                </div>
                <div style="padding: 20px 20px;">
                    <ce:HtmlLabel runat="server" Text="შოთა რუსთაველის ეროვნული სამეცნიერო ფონდი" />
                </div>
                <label>
                    <asp:Panel runat="server" ID="pnlUser" Visible="False">
                        <ce:Label runat="server" ID="lblUser"></ce:Label>
                        <br />
                        <ce:Label runat="server" Text="მომხმარებელი აქტივირებულია" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlError" Visible="False">
                        <ce:Label runat="server" ID="lblError"></ce:Label>
                    </asp:Panel>
                </label>
                <div class="form-group">
                    <asp:HyperLink CssClass="btn btn-success block full-width m-b" runat="server" ID="lnkAuth" NavigateUrl="/Rnsf/Gms/Gms.Portal.Web/">
                        <ce:Label runat="server">გაგრძელება</ce:Label>
                    </asp:HyperLink>
                </div>
                <p class="m-t"><small>Consulting & IT Innovations  &copy; 2017</small> </p>
            </div>
        </div>
    </form>
</body>
</html>
