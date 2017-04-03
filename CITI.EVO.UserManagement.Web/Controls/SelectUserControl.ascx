<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectUserControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.SelectUserControl" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>
<%@ Register TagPrefix="local" TagName="SearchUsersControl" Src="~/Controls/SearchUsersControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{SelectUserModel.ParentID=Value}" />

<div class="form-group">
    <label class="col-sm-2  font-normal">უფლებები</label>
    <div class="col-sm-12">
        <ce:DropDownList ID="ddlAccessLevels" runat="server" CssClass="chosen-select" Property="{SelectUserModel.AccessLevel=SelectedValue}">
            <Items>
                <asp:ListItem Text="სტანდარტული" Value="0" Selected="true" />
                <asp:ListItem Text="ადმინისტრატორი" Value="1" />
            </Items>
        </ce:DropDownList>
    </div>
</div>
<local:SearchUsersControl runat="server" ID="searchUsersControl" Property="{SelectUserModel.User=Model}" OnDataChanged="searchUsersControl_OnDataChanged" />

