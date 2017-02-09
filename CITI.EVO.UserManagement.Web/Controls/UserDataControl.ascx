<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserDataControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.UserDataControl" %>

<div class="box">
    <h3>
        <asp:Label runat="server">მომხმარებლის სახელი</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbLoginName" runat="server" Property="{UserModel.LoginName=Text}" />
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">პაროლი</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbPassword" runat="server" Property="{UserModel.Password=Text}" />
    </div>
</div>
<div class="box">
    <asp:CheckBox ID="chkChangePassword" runat="server" AutoPostBack="true" Property="{UserModel.ChangePassword=Checked}" OnCheckedChanged="chkChangePassword_CheckChanged" />
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">სახელი</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbFirstName" runat="server" Property="{UserModel.FirstName=Text}" />
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">გვარი</asp:Label>
    </h3>
    <div class="box_body">

        <asp:TextBox ID="tbLastName" runat="server" Property="{UserModel.LastName=Text}" />
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">ელ-ფოსტა</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbEmail" runat="server" Property="{UserModel.Email=Text}"></asp:TextBox>
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">მისამართი</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbAddress" runat="server" Property="{UserModel.Address=Text}" />
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">აქტივაცია</asp:Label>
    </h3>
    <div class="box_body">
        <asp:CheckBox ID="chkActivate" runat="server" Property="{UserModel.IsActive=Checked}" />
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">ვალიდურობის თარიღი</asp:Label>
    </h3>
    <div class="box_body">
        <dx:ASPxDateEdit runat="server" ID="dePasswordExpire" Property="{UserModel.PasswordExpire=Value}"></dx:ASPxDateEdit>
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">კატეგორია</asp:Label>
    </h3>
    <div class="box_body">
        <dx:ASPxComboBox runat="server" ID="cbxUserCategory" ValueField="ID" TextField="Name" ValueType="System.Guid">
        </dx:ASPxComboBox>
    </div>
</div>
