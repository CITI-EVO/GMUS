<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NamedExpressionControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.NamedExpressionControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>

<local:HiddenFieldValueControl runat="server" ID="hdKey" Property="{NamedExpressionModel.Key=Value}" />
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label" Text="Name:" />
    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" Property="{NamedExpressionModel.Name=Text}" Width="180" />
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label" Text="Expression:" />
    <asp:TextBox runat="server" ID="txtExpression" CssClass="form-control" Property="{NamedExpressionModel.Expression=Text}" Width="180" />
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label" Text="Type:" />
    <asp:DropDownList runat="server" Width="180" ID="ddlType" Property="{NamedExpressionModel.OutputType=SelectedValue}" CssClass="chosen-select">
        <Items>
            <asp:ListItem Text="Unspecified" Value="Unspecified" />
            <asp:ListItem Text="Text" Value="Text" />
            <asp:ListItem Text="Number" Value="Number" />
            <asp:ListItem Text="DateTime" Value="DateTime" />
        </Items>
    </asp:DropDownList>
</div>
