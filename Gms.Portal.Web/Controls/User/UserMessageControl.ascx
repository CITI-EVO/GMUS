<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserMessageControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.UserMessageControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdStatusUserID" Property="{UserMessageExModel.StatusUserID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdFromUserID" Property="{UserMessageExModel.FromUserID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdToUserID" Property="{UserMessageExModel.ToUserID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdRecordID" Property="{UserMessageExModel.RecordID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{UserMessageExModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdStatusID" Property="{UserMessageExModel.StatusID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdReaded" Property="{UserMessageExModel.Readed=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdFormID" Property="{UserMessageExModel.FormID=Value}" />


<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Subject</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxSubject" Property="{UserMessageExModel.Subject=Text}"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Text</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxSource" Property="{UserMessageExModel.Text=Text}" TextMode="MultiLine"></asp:TextBox>
    </div>
</div>
<asp:Panel CssClass="form-group" runat="server" ID="pnlAnswer">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Answer</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxReplay" Property="{UserMessageExModel.Replay=Text}" TextMode="MultiLine"></asp:TextBox>
    </div>
</asp:Panel>
