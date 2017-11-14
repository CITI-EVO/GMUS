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
<asp:Panel CssClass="form-group" runat="server" ID="pnlBirthYear" Visible="False">
    <label>
        <ce:Label runat="server" ID="Label1">დაბადების წელი</ce:Label>
    </label>
    <div class="input-group">
        <asp:TextBox runat="server" ID="tbxBirthYear" Property="{RegisterUserModel.BirthYear=Text}" CssClass="form-control"></asp:TextBox>
        <span class="input-group-btn">
            <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="btnSync" OnClick="btnSync_OnClick">Go</asp:LinkButton>
        </span>
    </div>
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
<asp:Panel CssClass="form-group" runat="server" ID="pnlBirthDate">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblBirthDate">დაბადების თარიღი</ce:Label>
    </label>
    <div class="input-group date">
        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
        <asp:TextBox runat="server" ID="tbxBirthDate" Property="{RegisterUserModel.BirthDate=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlLoginName">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblLoginName">ელ.ფოსტა</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxLoginName" Property="{RegisterUserModel.LoginName=Text}{RegisterUserModel.Email=Text}" CssClass="form-control"></asp:TextBox>
</asp:Panel>
<asp:Panel CssClass="form-group" runat="server" ID="pnlPhoneNumber">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server" ID="lblPhoneNumber">ტელეფონი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxPhoneNumber" Property="{RegisterUserModel.Phone=Text}" CssClass="form-control"></asp:TextBox>
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
