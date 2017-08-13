<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsersFilterControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.Filters.UsersFilterControl" %>


<div class="form-group">
    <ce:CheckBox CssClass="checkbox-inline" ID="cbLoginName" runat="server" Text="მომხმარებელი" Property="{UsersFilterModel.LoginName=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbPassword" runat="server" Text="პაროლი" Property="{UsersFilterModel.Password=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbFirstName" runat="server" Text="სახელი" Property="{UsersFilterModel.FirstName=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbLastName" runat="server" Text="გვარი" Property="{UsersFilterModel.LastName=Checked}" />
    <ce:CheckBox CssClass="checkbox-inline" ID="cbAddress" runat="server" Text="მისამართი" Property="{UsersFilterModel.Address=Checked}" />
</div>
<div class="">
    <div class="col-sm-3 m-b-xs">
        <ce:DropDownList ID="ddlUserCategories" runat="server" CssClass="chosen-select" DataTextField="Name" DataValueField="ID" Property="{UsersFilterModel.CategoryID=SelectedValue}" />
    </div>
    <div class="col-sm-3 m-b-xs">
        <ce:DropDownList ID="ddlStatues" runat="server" CssClass="chosen-select" Property="{UsersFilterModel.Status=SelectedValue}">
            <Items>
                <asp:ListItem Text="ყველა" Value="" Selected="true" />
                <asp:ListItem Text="აქტიური" Value="True" />
                <asp:ListItem Text="პასიური" Value="False" />
            </Items>
        </ce:DropDownList>
    </div>
    <div class="col-sm-3 m-b-xs">
        <asp:TextBox ID="txtKeyword" Height="29px" runat="server" CssClass="form-control" Property="{UsersFilterModel.Keyword=Text}" />
    </div>
</div>


