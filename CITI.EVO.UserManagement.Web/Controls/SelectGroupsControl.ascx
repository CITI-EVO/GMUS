<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectGroupsControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.SelectGroupsControl" %>
<%@ Register TagPrefix="local" TagName="SearchGroupsControl" Src="~/Controls/SearchGroupsControl.ascx" %>

<asp:Panel runat="server" ID="pnlAccessLevel">
    <div class="box">
        <h3>
            <asp:Label runat="server">უფლებები</asp:Label>
        </h3>
        <div class="box_body_short">
            <ce:ASPxComboBox ID="ddlAccessLevels" runat="server" Property="{SelectGroupsModel.AccessLevel=Value}">
                <Items>
                    <dx:ListEditItem Text="სტანდარტული" Value="0" />
                    <dx:ListEditItem Text="ადმინისტრატორი" Value="1" />
                </Items>
            </ce:ASPxComboBox>
        </div>
    </div>
</asp:Panel>
<div class="wrapper"></div>
<div class="popup_fieldset">
    <local:SearchGroupsControl runat="server" ID="searchGroupControl" Property="{SelectGroupsModel.Group=Model}" />
</div>