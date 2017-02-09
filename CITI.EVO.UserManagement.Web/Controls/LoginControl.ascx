<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.LoginControl" %>
<div class="box">
    <h1>მომხმარებელი</h1>
    <div class="box_body">
        <asp:TextBox runat="server" ID="tbLoginName" Property="{LoginModel.LoginName=Text}"></asp:TextBox>
    </div>
</div>
<div class="box">
    <h1>პაროლი
    </h1>
    <div class="box_body">
        <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" Property="{LoginModel.Password=Text}"></asp:TextBox>
    </div>
</div>
<div class="checkbox">
    <div class="box_body">
        <ce:CheckBox runat="server" ID="chkRememberMe" Property="{LoginModel.RememberMe=Checked}"></ce:CheckBox>
        <h2>დამახსოვრება
        </h2>
    </div>
</div>
