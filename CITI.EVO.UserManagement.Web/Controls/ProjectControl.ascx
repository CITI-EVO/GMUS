<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ProjectControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ProjectModel.ID=Value}" />

<div class="form-group">
    <label class="col-sm-4 control-label">მოდულის სახელი</label>
    <div class="col-sm-6">
        <asp:TextBox ID="tbxName" runat="server" class="form-control" Property="{ProjectModel.Name=Text}"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label class="col-sm-4 control-label">მოდულის სტატუსი</label>
    <div class="col-sm-6">
        <ce:CheckBox ID="CheckBox1" runat="server" Property="{ProjectModel.IsActive=Checked}" />
    </div>
</div>


