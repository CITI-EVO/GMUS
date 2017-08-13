<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageRejectControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.MessageRejectControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdMessageID" Property="{MessageRejectModel.MessageID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Description</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxDescription" Property="{MessageRejectModel.Description=Text}"></asp:TextBox>
    </div>
</div>