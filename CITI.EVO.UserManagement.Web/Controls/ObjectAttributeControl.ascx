<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ObjectAttributeControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ObjectAttributeControl" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ObjectAttributeModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{ObjectAttributeModel.ParentID=Value}" />

<div class="form-group">
    <label class="col-sm-4 control-label">მოდული</label>
    <div class="col-sm-6">
        <ce:ASPxComboBox ID="cbxProject" CssClass="form-control" TextField="Name" ValueField="ID" runat="server" AutoPostBack="true" Property="{ObjectAttributeModel.ProjectID=Value}" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged" />

    </div>
</div>
<div class="form-group">
    <label class="col-sm-4 control-label">ატრიბუტის სქემა</label>
    <div class="col-sm-6">
        <ce:ASPxComboBox ID="cbxSchema" CssClass="form-control" TextField="Name" ValueField="ID" runat="server" AutoPostBack="true" Property="{ObjectAttributeModel.SchemaID=Value}" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged" />
    </div>
</div>
<div class="form-group">
    <label class="col-sm-4 control-label">ატრიბუტის სახელი</label>
    <div class="col-sm-6">
        <ce:ASPxComboBox ID="cbxField" CssClass="form-control" TextField="Name" ValueField="ID" runat="server" AutoPostBack="true" Property="{ObjectAttributeModel.FieldID=Value}" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged" />
    </div>
</div>
<div class="form-group">
    <label class="col-sm-4 control-label">მნიშვნელობა</label>
    <div class="col-sm-6">
        <asp:TextBox ID="tbxValue" CssClass="form-control" runat="server" TextMode="MultiLine" Property="{ObjectAttributeModel.Value=Text}"></asp:TextBox>

    </div>
</div>
