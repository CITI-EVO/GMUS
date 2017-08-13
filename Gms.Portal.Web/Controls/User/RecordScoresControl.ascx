<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordScoresControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.RecordScoresControl" %>

<asp:Panel runat="server" ID="pnlMain">
    <div class="row" style="background-color: darkcyan; padding: 5px; color: white; font-weight: bold;">
        <div class="col-lg-1"></div>
        <div class="col-lg-4">Name</div>
        <div class="col-lg-3">Score</div>
        <div class="col-lg-2">Comment</div>
    </div>
</asp:Panel>
<div>
    <div>Comment</div>
    <div>
        <asp:TextBox runat="server" ID="tbxComment" TextMode="MultiLine" Width="100%"></asp:TextBox>
    </div>
</div>
