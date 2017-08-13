<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrintTemplateControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.PrintTemplateControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{PrintTemplateModel.ID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" Property="{PrintTemplateModel.Name=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Layout</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxLayout" Property="{PrintTemplateModel.Layout=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Portrait" Value="Portrait" />
                <asp:ListItem Text="Landscape" Value="Landscape" />
            </Items>
        </asp:DropDownList>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Type</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxType" Property="{PrintTemplateModel.Type=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="PDF" Value="PDF" />
                <asp:ListItem Text="HTML" Value="HTML" />
            </Items>
        </asp:DropDownList>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Role</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxRole" Property="{PrintTemplateModel.Role=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="ყველა" Value="" />
                <asp:ListItem Text="ფონდის თანამშრომელი" Value="Admin" />
                <asp:ListItem Text="ორგანიზაცია" Value="Org" />
                <asp:ListItem Text="მეცნიერი" Value="User" />
                <asp:ListItem Text="ექსპერტი" Value="Expert" />
            </Items>
        </asp:DropDownList>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Template</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxTemplate" Property="{PrintTemplateModel.Template=Text}" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
    </div>
</div>
