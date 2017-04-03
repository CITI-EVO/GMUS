<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegisterUserControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.RegisterUserControl" %>

<asp:Panel runat="server" ID="pnlGroup" CssClass="form-group">
    <label>
        <ce:Label runat="server">ჯგუფი</ce:Label>
    </label>
    <asp:DropDownList runat="server" ID="cbxGroups" DataValueField="ID" DataTextField="Name" Property="{RegisterUserModel.GroupID=SelectedValue}" CssClass="chosen-select">
    </asp:DropDownList>
</asp:Panel>
<div class="form-group">
    <label>
        <ce:Label runat="server">პირადი N</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxPersonalID" Property="{RegisterUserModel.PersonalID=Text}" CssClass="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server">სახელი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxFirstName" Property="{RegisterUserModel.FirstName=Text}" CssClass="form-control"></asp:TextBox>

</div>
<div class="form-group">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server">გვარი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxLastName" Property="{RegisterUserModel.LastName=Text}" CssClass="form-control"></asp:TextBox>

</div>
<%--<div class="form-group">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server">დაბადების თარიღი</ce:Label>
    </label>
    <div class="input-group date">
        <span class="input-group-addon">
            <span class="fa fa-calendar"></span>
        </span>
        <asp:TextBox runat="server" ID="tbxBirthDate" CssClass="form-control" Property="{RegisterUserModel.BirthDate=Text}"></asp:TextBox>
    </div>
</div>--%>
<div class="form-group">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server">ელ.ფოსტა</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxLoginName" Property="{RegisterUserModel.LoginName=Text}{RegisterUserModel.Email=Text}" CssClass="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server">პაროლი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxPassword" TextMode="Password" Property="{RegisterUserModel.Password=Text}" CssClass="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label>
        <span style="color: Red;">*</span>
        <ce:Label runat="server">დაადასტურეთ პაროლი</ce:Label>
    </label>
    <asp:TextBox runat="server" ID="tbxConfirmPassword" TextMode="Password" Property="{RegisterUserModel.ConfirmPassword=Text}" CssClass="form-control"></asp:TextBox>
</div>
