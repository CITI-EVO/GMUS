<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecoveryControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.RecoveryControl" %>
<div class="form-group">
    <label>
        <ce:Label runat="server">ახალი პაროლი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxPassword" TextMode="Password" Property="{RecoveryModel.NewPassword=Text}" CssClass="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label>
        <ce:Label runat="server">დაადასტურეთ პაროლი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxConfirmPassword" TextMode="Password" Property="{RecoveryModel.ConfirmPassword=Text}" CssClass="form-control"></asp:TextBox>
</div>
