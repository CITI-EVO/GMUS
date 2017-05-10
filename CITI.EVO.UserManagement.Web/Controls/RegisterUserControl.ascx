<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegisterUserControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.RegisterUserControl" %>

<asp:Panel runat="server" CssClass="form-group" ID="pnlGroup">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblGroups">ჯგუფი</ce:Label>
    </label>
    <asp:DropDownList runat="server" ID="cbxGroups" DataValueField="ID" DataTextField="Name" Property="{RegisterUserModel.GroupID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True">
    </asp:DropDownList>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlPersonalID" Visible="False">
    <label>
        <ce:Label runat="server" ID="lblPersonalID">პირადი N</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxPersonalID" Property="{RegisterUserModel.PersonalID=Text}" CssClass="form-control"></asp:TextBox>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlFirstName">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblFirstName">სახელი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxFirstName" Property="{RegisterUserModel.FirstName=Text}" CssClass="form-control"></asp:TextBox>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlLastName">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblLastName">გვარი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxLastName" Property="{RegisterUserModel.LastName=Text}" CssClass="form-control"></asp:TextBox>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlLoginName">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblLoginName">ელ.ფოსტა</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxLoginName" Property="{RegisterUserModel.LoginName=Text}{RegisterUserModel.Email=Text}" CssClass="form-control"></asp:TextBox>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlPassword">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblPassword">პაროლი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxPassword" TextMode="Password" Property="{RegisterUserModel.Password=Text}" CssClass="form-control"></asp:TextBox>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlConfirmPassword">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblConfirmPassword">დაადასტურეთ პაროლი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxConfirmPassword" TextMode="Password" Property="{RegisterUserModel.ConfirmPassword=Text}" CssClass="form-control"></asp:TextBox>
</asp:Panel>
