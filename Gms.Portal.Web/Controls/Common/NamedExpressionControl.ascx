<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NamedExpressionControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.NamedExpressionControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>

<local:HiddenFieldValueControl runat="server" ID="hdKey" Property="{NamedExpressionModel.Key=Value}" />
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-3 control-label" Text="Name:" />
    <div class="col-sm-9">
        <asp:TextBox runat="server" ID="tbxName" CssClass="form-control" Property="{NamedExpressionModel.Name=Text}" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-3 control-label" Text="Expression:" />
    <div class="col-sm-9">
        <asp:TextBox runat="server" ID="tbxExpression" CssClass="form-control" Property="{NamedExpressionModel.Expression=Text}"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-3 control-label" Text="Type:" />
    <div class="col-sm-9">
        <asp:DropDownList runat="server" ID="ddlType" Property="{NamedExpressionModel.OutputType=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Unspecified" Value="Unspecified" />
                <asp:ListItem Text="Text" Value="Text" />
                <asp:ListItem Text="Number" Value="Number" />
                <asp:ListItem Text="DateTime" Value="DateTime" />
            </Items>
        </asp:DropDownList>
    </div>
</div>
