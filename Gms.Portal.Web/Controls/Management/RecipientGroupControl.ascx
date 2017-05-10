<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecipientGroupControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.RecipientGroupControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{RecipientGroupModel.ID=Value}" />

<asp:Panel runat="server" ID="pnlType" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Type</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxType" AppendDataBoundItems="True" Property="{RecipientGroupModel.Type=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" >
            <Items>
                <asp:ListItem Text="Select an Option" Value="" />
                <asp:ListItem Text="Standard" Value="Standard" />
                <asp:ListItem Text="Expression" Value="Expression" />
            </Items>
        </ce:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlName" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" CssClass="form-control" Property="{RecipientGroupModel.Name=Text}" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlVisible" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Description</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxDescription" CssClass="form-control" Property="{RecipientGroupModel.Description=Text}" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlForm" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Form</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxForm" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" Property="{RecipientGroupModel.FormID=SelectedValue}" CssClass="chosen-select">
        </ce:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlExpression" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Expression</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxExpression" CssClass="form-control" Property="{RecipientGroupModel.Expression=Text}" />
    </div>
</asp:Panel>

