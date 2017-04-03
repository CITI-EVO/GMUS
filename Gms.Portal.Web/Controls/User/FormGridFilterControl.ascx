<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormGridFilterControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormGridFilterControl" %>
<div class="form-group">
    <label class="control-label col-lg-1">
        <ce:Label runat="server" Text="Status" />
    </label>
    <div class="col-lg-3">
        <ce:DropDownList runat="server" ID="cbxDataStatus" DataTextField="Name" DataValueField="ID" Property="{FormGridFilterModel.StatusID=SelectedValue}" CssClass="chosen-select">
        </ce:DropDownList>
    </div>
</div>
<div class="form-group">
    <label class="control-label col-lg-1">
        <ce:Label runat="server" Text="Date range" />
    </label>
    <div class="col-lg-3">
        <div class="input-daterange input-group">
            <asp:TextBox CssClass="input-sm form-control" name="start" runat="server" Property="{FormGridFilterModel.StartDate=Text}" />
            <span class="input-group-addon">-</span>
            <asp:TextBox CssClass="input-sm form-control" name="end" runat="server" Property="{FormGridFilterModel.EndDate=Text}" />
        </div>
    </div>
</div>
