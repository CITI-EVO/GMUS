<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormDataControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormDataControl" %>

<asp:HiddenField runat="server" ID="hdMode" />
<asp:HiddenField runat="server" ID="hdFormID" />
<asp:HiddenField runat="server" ID="hdUserID" />
<asp:HiddenField runat="server" ID="hdOwnerID" />
<asp:HiddenField runat="server" ID="hdRecordID" />
<asp:HiddenField runat="server" ID="hdParentID" />
<asp:HiddenField runat="server" ID="hdStatusID" />
<asp:HiddenField runat="server" ID="hdContainerID" />
<asp:HiddenField runat="server" ID="hdEnabled" />
<asp:HiddenField runat="server" ID="hdDateCreated" />

<asp:Panel runat="server" ID="pnlMain" CssClass="row">
</asp:Panel>

<script type="text/javascript">
    function onTabClick(e) {
        var tabId = $(e).attr("tab-id");
        var activeStore = $(e).attr("active-store");
        $('#' + activeStore).val(tabId);
    }
</script>
