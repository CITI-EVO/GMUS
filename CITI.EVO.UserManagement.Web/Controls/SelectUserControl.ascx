<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectUserControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.SelectUserControl" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>
<%@ Register TagPrefix="local" TagName="SearchUsersControl" Src="~/Controls/SearchUsersControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{SelectUsersModel.ParentID=Value}" />

<div>
    <ce:ASPxComboBox ID="ddlAccessLevels" Width="320" runat="server" ValueType="System.Int32" Property="{SelectUsersModel.AccessLevel=Value}">
        <Items>
            <dx:ListEditItem Text="სტანდარტული" Value="0" />
            <dx:ListEditItem Text="ადმინისტრატორი" Value="1" />
        </Items>
    </ce:ASPxComboBox>
</div>
<div>
    <local:SearchUsersControl runat="server" ID="searchUsersControl" Property="{SelectUsersModel.User=Model}" OnDataChanged="searchUsersControl_OnDataChanged" />
</div>
