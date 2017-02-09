<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecoveryControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.RecoveryControl" %>
<div class="box">
    <h1>
        <ce:Label runat="server">ახალი პაროლი</ce:Label>
    </h1>
    <div class="box_body">
        <asp:TextBox runat="server" ID="tbxPassword" TextMode="Password" Property="{RecoveryModel.NewPassword}"></asp:TextBox>
    </div>
</div>
<div class="box">
    <h1>
        <ce:Label runat="server">დაადასტურეთ პაროლი</ce:Label>
    </h1>
    <div class="box_body">
        <asp:TextBox runat="server" ID="tbxConfirmPassword" TextMode="Password" Property="{RecoveryModel.ConfirmPassword}"></asp:TextBox>
    </div>
</div>
