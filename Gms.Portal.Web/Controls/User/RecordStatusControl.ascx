<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordStatusControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.RecordStatusControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdRecordID" Property="{RecordStatusModel.RecordID=Value}" />

<asp:Panel runat="server" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Status</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxDataStatus" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" Property="{RecordStatusModel.StatusID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
        </ce:DropDownList>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlDescription" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 control-label">Description</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxDescription" Property="{RecordStatusModel.Description=Text}" CssClass="form-control" />
    </div>
</asp:Panel>
