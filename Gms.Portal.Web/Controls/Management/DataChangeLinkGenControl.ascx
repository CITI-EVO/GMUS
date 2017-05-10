<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataChangeLinkGenControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.DataChangeLinkGenControl" %>
<asp:Panel runat="server" ID="pnlForm" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Form</ce:Label>
    <div class="col-lg-6">
        <ce:DropDownList runat="server" ID="cbxForm" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" Property="{DataChangeLinkGenModel.FormID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="cbxForm_OnSelectedIndexChanged">
        </ce:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlField" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Field</ce:Label>
    <div class="col-lg-6">
        <ce:DropDownList runat="server" ID="cbxField" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" Property="{DataChangeLinkGenModel.FieldKey=SelectedValue}" CssClass="chosen-select">
        </ce:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlValue" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Value</ce:Label>
    <div class="col-lg-6">
        <asp:TextBox runat="server" ID="tbxValue" CssClass="form-control" Property="{DataChangeLinkGenModel.Value=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlText" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Text</ce:Label>
    <div class="col-lg-6">
        <asp:TextBox runat="server" ID="tbxText" CssClass="form-control" Property="{DataChangeLinkGenModel.Text=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlGenButton" CssClass="form-group">
</asp:Panel>
<asp:Panel runat="server" ID="pnlLink" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Text</ce:Label>
    <div class="col-lg-1">
        <ce:LinkButton runat="server" ID="btnGenerate" OnClick="btnGenerate_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-plus-square-o" />
    </div>
    <div class="col-lg-5">
        <asp:TextBox runat="server" ID="tbxLink" ValidateRequestMode="Disabled" CssClass="form-control" Property="{DataChangeLinkGenModel.Link=Text}" ReadOnly="True"></asp:TextBox>
    </div>
</asp:Panel>


