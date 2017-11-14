<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExpressionControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.ExpressionControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>

<local:HiddenFieldValueControl runat="server" ID="hdKey" Property="{ExpressionModel.Key=Value}" />
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-3 control-label" Text="Expression:" />
    <div class="col-sm-9">
        <asp:TextBox runat="server" ID="txtExpression" CssClass="form-control" Property="{ExpressionModel.Expression=Text}" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-3 control-label" Text="Type:" />
    <div class="col-sm-9">
        <ce:DropDownList runat="server" ID="ddlType" Property="{ExpressionModel.OutputType=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Unspecified" Value="Unspecified" />
                <asp:ListItem Text="Text" Value="Text" />
                <asp:ListItem Text="Number" Value="Number" />
                <asp:ListItem Text="DateTime" Value="DateTime" />
            </Items>
        </ce:DropDownList>
    </div>
</div>

