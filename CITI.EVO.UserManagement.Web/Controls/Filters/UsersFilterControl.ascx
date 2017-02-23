<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsersFilterControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.Filters.UsersFilterControl" %>




<div class="form-group">
    <ce:CheckBox CssClass="checkbox-inline" ID="cbLoginName" runat="server" Text="მომხმარებელი" Property="{UsersFilterModel.LoginName=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbPassword" runat="server" Text="პაროლი" Property="{UsersFilterModel.Password=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbFirstName" runat="server" Text="სახელი" Property="{UsersFilterModel.FirstName=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbLastName" runat="server" Text="გვარი" Property="{UsersFilterModel.LastName=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbEmail" runat="server" Text="ელ-ფოსტა" Property="{UsersFilterModel.Email=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbAddress" runat="server" Text="მისამართი" Property="{UsersFilterModel.Address=Checked}" />
</div>
<div class="form-group">

    <div class="col-md-2">
        <ce:ASPxComboBox ID="ddlUserCategories" runat="server" Width="100" TextField="Name" ValueField="ID" Property="{UsersFilterModel.CategoryID=Value}" />
    </div>

    <div class="col-md-2">
        <ce:ASPxComboBox ID="ddlStatues" Width="100" runat="server" Property="{UsersFilterModel.Status=Value}">
            <Items>
                <dx:ListEditItem Text="ყველა" Value="" Selected="true" />
                <dx:ListEditItem Text="აქტიური" Value="True" />
                <dx:ListEditItem Text="პასიური" Value="False" />
            </Items>
        </ce:ASPxComboBox>
    </div>
    <div class="col-md-2">
        <asp:TextBox ID="txtKeyword" Width="100px" Height="27px" runat="server" Property="{UsersFilterModel.Keyword=Text}" />
    </div>
</div>

