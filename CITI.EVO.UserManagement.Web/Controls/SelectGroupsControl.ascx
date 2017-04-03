<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectGroupsControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.SelectGroupsControl" %>
<%@ Register TagPrefix="local" TagName="SearchGroupsControl" Src="~/Controls/SearchGroupsControl.ascx" %>

<asp:Panel runat="server" ID="pnlAccessLevel">
    <div class="form-group col-sm-12">
        <label class="col-sm-2">უფლებები</label>
        <div class="col-sm-12">
            <ce:DropDownList ID="ddlAccessLevels" runat="server" CssClass="chosen-select" Property="{SelectGroupsModel.AccessLevel=SelectedValue}">
                <Items>
                    <asp:ListItem Text="სტანდარტული" Value="0" Selected="true" />
                    <asp:ListItem Text="ადმინისტრატორი" Value="1" />
                </Items>
            </ce:DropDownList>
        </div>
    </div>
</asp:Panel>

<local:SearchGroupsControl runat="server" ID="searchGroupControl" Property="{SelectGroupsModel.Groups=Model}" />

