<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Register" %>

<%@ Register TagPrefix="local" TagName="RegisterUserControl" Src="~/Controls/RegisterUserControl.ascx" %>

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
            <asp:Panel runat="server" ID="pnlCompleted" Visible="False">
                <ce:Label runat="server" ID="lblMessage" ForeColor="Green" Font-Size="14px"></ce:Label>
                <asp:Label runat="server" ID="lblAdminEmail"></asp:Label>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlUserData" CssClass="row">
                <div>
                    <ce:Label runat="server" ForeColor="Red" ID="lblError" Font-Size="14px" Font-Bold="True"></ce:Label>
                </div>
                <local:RegisterUserControl runat="server" ID="registerUserControl" />
                <div>
                    <ce:LinkButton ID="btOK" CssClass="btn btn-success block full-width m-b" runat="server" OnClick="btOK_Click">
                        <ce:label runat="server" Text="გაგზავნა"/>
                    </ce:LinkButton>
                    <ce:LinkButton CssClass="btn btn-success block full-width m-b" runat="server" ID="btCancel" OnClick="btCancel_Click">
                        <ce:label runat="server" Text="უკან დაბრუნება"/>
                    </ce:LinkButton>
                </div>
                <p class="m-t"><small>Consulting & IT Innovations  &copy; 2017</small> </p>
            </asp:Panel>
        </asp:Panel>

        <script src='<%=ResolveUrl("~/Scripts/jquery-3.1.1.min.js")%>' type='text/javascript'></script>
        <script src='<%=ResolveUrl("~/Scripts/bootstrap.min.js")%>' type='text/javascript'></script>
        <script src='<%=ResolveUrl("~/Scripts/plugins/datapicker/bootstrap-datepicker.js")%>' type='text/javascript'></script>
        <script src='<%=ResolveUrl("~/Scripts/plugins/chosen/chosen.jquery.js")%>' type='text/javascript'></script>
        <script src='<%=ResolveUrl("~/Scripts/inspinia.js")%>' type='text/javascript'></script>

        <script type="text/javascript">
            $(document).ready(function () {
                $('.chosen-select').chosen();

                $('.input-group.date').datepicker({
                    todayBtn: "linked",
                    keyboardNavigation: false,
                    forceParse: false,
                    calendarWeeks: true,
                    autoclose: true
                });
            });
        </script>
    </form>
</body>
</html>
