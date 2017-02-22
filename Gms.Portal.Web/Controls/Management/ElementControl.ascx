<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ElementControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.ElementControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ElementModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{ElementModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentType" Property="{ElementModel.ParentType=Value}" />

<asp:Panel runat="server" ID="pnlName" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" CssClass="form-control" Property="{ElementModel.Name=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlVisible" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Visible</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="tbxVisible" Property="{ElementModel.Visible=Checked}"></asp:CheckBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlElementType" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Element Type</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxElementType" AppendDataBoundItems="True" Property="{ElementModel.ElementType=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
        </asp:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlOrderIndex" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Order Index</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="seOrderIndex" Property="{ElementModel.OrderIndex=Text}" CssClass="intSpinEdit" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlMandatory" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Mandatory</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="chkMandatory" Property="{ElementModel.Mandatory=Checked}" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlPrivacy" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Privacy</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="chkPrivacy" Property="{ElementModel.Privacy=Checked}" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlDescription" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Description</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxDescription" Property="{ElementModel.Description=Text}" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlGroupAlign" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Group Size</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxGroupSize" Property="{ElementModel.GroupSize=Text}" CssClass="groupSizeSpin" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlType" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Control Type</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxType" AppendDataBoundItems="True" Property="{ElementModel.ControlType=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
            <Items>
                <asp:ListItem Text="Select an Option" Value="" />
                <asp:ListItem Text="TextBox" Value="TextBox" />
                <asp:ListItem Text="Date" Value="Date" />
                <asp:ListItem Text="Time" Value="Time" />
                <asp:ListItem Text="Number" Value="Number" />
                <asp:ListItem Text="CheckBox" Value="CheckBox" />
                <asp:ListItem Text="ComboBox" Value="ComboBox" />
                <asp:ListItem Text="RagioButton" Value="RagioButton" />
                <asp:ListItem Text="FileUpload" Value="FileUpload" />
            </Items>
        </asp:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlMask" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Mask</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxMask" CssClass="form-control" Property="{ElementModel.Mask=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlEnabled" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Enabled</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="chkEnabled" Property="{ElementModel.Enabled=Checked}"></asp:CheckBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlDisplayOnGrid" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Display On Grid</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="chkDisplayOnGrid" Property="{ElementModel.DisplayOnGrid=Checked}"></asp:CheckBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlValidationExp" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">ValidationExp</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxValidationExp" CssClass="form-control" Property="{ElementModel.ValidationExp=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlTag" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Tag</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxTag" CssClass="form-control" Property="{ElementModel.Tag=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlDataSource" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Data Source</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxDataSource" AppendDataBoundItems="True" DataTextField="Name" DataValueField="ID" Property="{ElementModel.DataSourceID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
        </asp:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlTextExpression" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Text Expression</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxTextExpression" CssClass="form-control" Property="{ElementModel.TextExpression=Text}"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlValueExpression" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Value Expression</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxValueExpression" CssClass="form-control" Property="{ElementModel.ValueExpression=Text}"></asp:TextBox>
    </div>
</asp:Panel>
