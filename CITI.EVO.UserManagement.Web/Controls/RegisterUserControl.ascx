<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegisterUserControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.RegisterUserControl" %>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">იდენტიფიკატორი</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:DropDownList runat="server" ID="cbxGroups" DataValueField="ID" DataTextField="Name" Property="{RegisterUserModel.GroupID=SelectedValue}">
        </asp:DropDownList>
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">იდენტიფიკატორი</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:TextBox runat="server" ID="tbxLoginName" Property="{RegisterUserModel.LoginName=Text}"></asp:TextBox>
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">ელ-ფოსტა</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:TextBox runat="server" ID="tbxEmail" Property="{RegisterUserModel.Email=Text}"></asp:TextBox>
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">პირადი N</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:TextBox runat="server" ID="tbxPersonalID" Property="{RegisterUserModel.PersonalID=Text}"></asp:TextBox>
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">სახელი</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:TextBox runat="server" ID="tbxFirstName" Property="{RegisterUserModel.FirstName=Text}"></asp:TextBox>
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">გვარი</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:TextBox runat="server" ID="tbxLastName" Property="{RegisterUserModel.LastName=Text}"></asp:TextBox>
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">დაბადების თარიღი</ce:Label>
    </div>
    <div class="box_bodyregister_small">
        <dx:ASPxDateEdit runat="server" ID="deBirthDate" Property="{RegisterUserModel.BirthDate=Value}" />
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">პაროლი</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:TextBox runat="server" ID="tbxPassword" TextMode="Password" Property="{RegisterUserModel.Password=Text}"></asp:TextBox>
    </div>
</div>
<div class="registerbox">
    <div class="title">
        <ce:Label runat="server">დაადასტურეთ პაროლი</ce:Label>
    </div>
    <div class="box_bodyregister">
        <asp:TextBox runat="server" ID="tbxConfirmPassword" TextMode="Password" Property="{RegisterUserModel.ConfirmPassword=Text}"></asp:TextBox>
    </div>
</div>
