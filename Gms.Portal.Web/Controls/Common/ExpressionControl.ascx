<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExpressionControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.ExpressionControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>

<local:HiddenFieldValueControl runat="server" ID="hdKey" Property="{ExpressionModel.Key=Value}" />
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label" Text="Expression:" />
    <asp:TextBox runat="server" ID="txtExpression" Property="{ExpressionModel.Expression=Text}" Width="180" />
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label" Text="Type:" />
    <ce:DropDownList runat="server" Width="180" ID="ddlType" Property="{ExpressionModel.OutputType=SelectedValue}" CssClass="chosen-select">
        <Items>
            <asp:ListItem Text="Unspecified" Value="Unspecified" />
            <asp:ListItem Text="Text" Value="Text" />
            <asp:ListItem Text="Number" Value="Number" />
            <asp:ListItem Text="DateTime" Value="DateTime" />
        </Items>
    </ce:DropDownList>
</div>

