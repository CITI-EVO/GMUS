<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ChangePassword" %>
<div class="form-group">
    <label>
        <span style="color: red;">*</span>
        <ce:Label runat="server" Text="ძველი პაროლი" />
    </label>
    <asp:TextBox runat="server" class="form-control" ID="tbOldPassword" TextMode="Password" Property="{ChangePasswordModel.OldPassword=Text}"></asp:TextBox>
</div>
<div class="form-group">
    <label>
        <span style="color: red;">*</span>
        <ce:Label runat="server" Text="პაროლი" />
    </label>
    <asp:TextBox runat="server" ID="tbNewPassword" class="form-control" TextMode="Password" Property="{ChangePasswordModel.NewPassword=Text}"></asp:TextBox>
</div>
<div class="form-group">
    <label>
        <span style="color: red;">*</span>
        <ce:Label runat="server" Text="დაადასტურეთ პაროლი" />
    </label>
    <asp:TextBox runat="server" ID="tbxConfirmPassword" class="form-control" TextMode="Password" Property="{ChangePasswordModel.ConfirmPassword=Text}"></asp:TextBox>
</div>
<div class="form-group">
    <label>
        <ce:Label runat="server" Text="ელ-ფოსტა" />
        <ce:label runat="server" style="color: red;">(მიუთითეთ თუ გსურთ ელ.ფოსტის ცვლილება)</ce:label>
    </label>
    <asp:TextBox runat="server" ID="tbxEmail" class="form-control" TextMode="Email" Property="{ChangePasswordModel.Email=Text}"></asp:TextBox>
</div>
<asp:Panel CssClass="form-group" runat="server" Visible="False">
    <label>
        <ce:Label runat="server" Text="ტელეფონი" />
        <ce:label runat="server" style="color: red;">(მიუთითეთ თუ გსურთ ელ.ფოსტის ცვლილება)</ce:label>
    </label>
    <asp:TextBox runat="server" ID="tbxPhone" class="form-control" Property="{ChangePasswordModel.Phone=Text}"></asp:TextBox>
</asp:Panel>
