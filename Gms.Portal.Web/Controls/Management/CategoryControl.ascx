<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CategoryControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.CategoryControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{CategoryModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{CategoryModel.ParentID=Value}" />

<asp:Panel runat="server" ID="pnlName" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" CssClass="form-control" Property="{CategoryModel.Name=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlVisible" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Visible</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="tbxVisible" Property="{CategoryModel.Visible=Checked}"></asp:CheckBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlOrderIndex" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Order Index</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="seOrderIndex" Property="{CategoryModel.OrderIndex=Text}" CssClass="intSpinEdit" />
    </div>
</asp:Panel>