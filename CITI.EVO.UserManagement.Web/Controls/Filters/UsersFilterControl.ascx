<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsersFilterControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.Filters.UsersFilterControl" %>

<div>
    <ce:CheckBox ID="cbLoginName" runat="server" Text="მომხმარებელი" Property="{UsersFilterModel.LoginName=Checked}" />
    <ce:CheckBox ID="cbPassword" runat="server" Text="პაროლი" Property="{UsersFilterModel.Password=Checked}" />
    <ce:CheckBox ID="cbFirstName" runat="server" Text="სახელი" Property="{UsersFilterModel.FirstName=Checked}" />
    <ce:CheckBox ID="cbLastName" runat="server" Text="გვარი" Property="{UsersFilterModel.LastName=Checked}" />
    <ce:CheckBox ID="cbEmail" runat="server" Text="ელ-ფოსტა" Property="{UsersFilterModel.Email=Checked}" />
    <ce:CheckBox ID="cbAddress" runat="server" Text="მისამართი" Property="{UsersFilterModel.Address=Checked}" />
</div>
<div class="wrapper"></div>

<div class="left">
    <ce:ASPxComboBox ID="ddlUserCategories" runat="server" Width="100" TextField="Name" ValueField="ID" Property="{UsersFilterModel.CategoryID=Value}" />
</div>
<div class="left">
    <ce:ASPxComboBox ID="ddlStatues" Width="100" runat="server" Property="{UsersFilterModel.Status=Value}">
        <Items>
            <dx:ListEditItem Text="ყველა" Value="" Selected="true" />
            <dx:ListEditItem Text="აქტიური" Value="True" />
            <dx:ListEditItem Text="პასიური" Value="False" />
        </Items>
    </ce:ASPxComboBox>
</div>

<div>
    <asp:TextBox ID="txtKeyword" Width="100px" Height="27px" runat="server" Property="{UsersFilterModel.Keyword=Text}" />
</div>
