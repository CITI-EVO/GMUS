<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecipientControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.RecipientControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{RecipientModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdGroupID" Property="{RecipientModel.GroupID=Value}" />

<asp:Panel runat="server" ID="pnlUsers" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-sm-6 font-normal">User</ce:Label>
    <div class="col-lg-6">
        <ce:DropDownList runat="server" ID="cbxUsers" DataValueField="ID" DataTextField="LoginName" AppendDataBoundItems="False" Property="{RecipientModel.UserID=SelectedValue}" CssClass="chosen-select">
        </ce:DropDownList>
    </div>
</asp:Panel>
