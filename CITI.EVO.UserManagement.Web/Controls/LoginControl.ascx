<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.LoginControl" %>


<div class="form-group">
    <asp:TextBox runat="server" class="form-control" ID="tbLoginName" Property="{LoginModel.LoginName=Text}"></asp:TextBox>
</div>
<div class="form-group">
    <asp:TextBox runat="server" ID="tbPassword" class="form-control" TextMode="Password" Property="{LoginModel.Password=Text}"></asp:TextBox>
</div>

<div class="form-group">
    <ce:CheckBox runat="server" ID="chkRememberMe" Property="{LoginModel.RememberMe=Checked}"></ce:CheckBox>
    დამახსოვრება
</div>
