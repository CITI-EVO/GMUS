<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ObjectAttributeControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ObjectAttributeControl" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ObjectAttributeModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{ObjectAttributeModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdTarget" Property="{ObjectAttributeModel.Target=Value}" />

<div class="box">
    <h3>
        <asp:Label runat="server">მოდული</asp:Label>
    </h3>
    <div class="box_body">
        <ce:ASPxComboBox ID="cbxProject" CssClass="content_left_dropdown" TextField="Name" ValueField="ID" Width="180px" runat="server" AutoPostBack="true" Property="{ObjectAttributeModel.ProjectID=Value}" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged" />
    </div>
</div>
<div class="box">
    <h3>
        <asp:Label runat="server">ატრიბუტის სქემა</asp:Label>
    </h3>
    <div class="box_body">
        <ce:ASPxComboBox ID="cbxSchema" CssClass="content_left_dropdown" TextField="Name" ValueField="ID" Width="180px" runat="server" AutoPostBack="true" Property="{ObjectAttributeModel.SchemaID=Value}" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged" />
    </div>
</div>

<div class="box">
    <h3>
        <asp:Label runat="server">ატრიბუტის სახელი</asp:Label>
    </h3>
    <div class="box_body">
        <ce:ASPxComboBox ID="cbxField" CssClass="content_left_dropdown" TextField="Name" ValueField="ID" Width="180px" runat="server" AutoPostBack="true" Property="{ObjectAttributeModel.FieldID=Value}" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged"  />
    </div>
</div>

<div class="box">
    <h3>
        <asp:Label runat="server">მნიშვნელობა</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbxValue" Width="178px" runat="server" TextMode="MultiLine" Property="{ObjectAttributeModel.Value=Text}"></asp:TextBox>
    </div>
</div>
