<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailContractControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.MailContractControl" %>

<%@ Register Src="~/Controls/Management/DataChangeLinkGenControl.ascx" TagPrefix="local" TagName="DataChangeLinkGenControl" %>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-1 control-label">Recipients Groups</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList AppendDataBoundItems="True" runat="server" ID="cbxRecipientGroups" AutoPostBack="True" DataTextField="Name" DataValueField="ID" Property="{MailContactModel.RecipientGroupID=SelectedValue}" CssClass="chosen-select">
            <asp:ListItem Text="Select an Option" Value="" />
        </asp:DropDownList>
    </div>
</div>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-1 control-label">Recipients</ce:Label>
    <div class="col-lg-10">
        <div style="overflow-y: auto; height: 100px; border: 1px solid #aaaaaa">
            <div class="form-control">
                <ce:CheckBox runat="server" ID="cbCheckAll" Text="Select all" AutoPostBack="True" OnCheckedChanged="cbCheckAll_OnCheckedChanged" />
                <ce:CheckBoxList runat="server" ID="cblRecipients" DataTextField="Text" DataValueField="Value">
                </ce:CheckBoxList>
            </div>
        </div>
    </div>
</div>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-1 control-label">Notification Type</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxContactType" Property="{MailContactModel.ContactType=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="All" Value="All" />
                <asp:ListItem Text="Email" Value="Email" />
                <asp:ListItem Text="SMS" Value="Sms" />
            </Items>
        </ce:DropDownList>
    </div>
</div>

<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-1 control-label">Template:</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList AppendDataBoundItems="True" runat="server" ID="cbxMessageTemplate" AutoPostBack="True" DataTextField="Name" DataValueField="ID" Property="{MailContactModel.TemplateID=SelectedValue}" CssClass="chosen-select">
            <asp:ListItem Text="Select an Option" Value="" />
        </asp:DropDownList>
    </div>
</div>
<asp:Panel runat="server" ID="pnlTemplateName" class="form-group" Visible="False">
    <ce:Label runat="server" CssClass="col-lg-1 control-label">Name:</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxTemplateName" Property="{MailContactModel.TemplateName=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlSaveTemplate" class="form-group" Visible="False">
    <ce:Label runat="server" CssClass="col-lg-1 control-label"></ce:Label>
    <div class="col-lg-10">
        <ce:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" />
    </div>
</asp:Panel>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-1 control-label">Subject:</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxSubject" Property="{MailContactModel.Subject=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<asp:Panel runat="server" ID="pnlForm" CssClass="form-group">
    <ce:Label runat="server" CssClass="col-lg-1 font-normal">Form:</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxForm" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" Property="{MailContactModel.FormID=SelectedValue}" CssClass="chosen-select">
        </ce:DropDownList>
    </div>
</asp:Panel>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-1 control-label">Body:</ce:Label>
    <div class="col-lg-10">
        <ce:LinkButton runat="server" ID="btnGenLink" OnClick="btnGenLink_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-external-link-square" />
        <br />
        <br />
        <asp:TextBox runat="server" ID="tbxBody" ValidateRequestMode="Disabled" Property="{MailContactModel.Body=Text}" Rows="5" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
    </div>
</div>

<div>
    <asp:Panel ID="pnlDataChangeLinkGen" runat="server" Style="display: none" DefaultButton="btnDataChangeLinkGenCancel">
        <asp:Button ID="btnDataChangeLinkGenFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeDataChangeLinkGen" runat="server" PopupControlID="pnlDataChangeLinkGen" BackgroundCssClass="modalBackground" TargetControlID="btnDataChangeLinkGenFake" CancelControlID="btnDataChangeLinkGenCancel" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <h5>
                            <ce:Label runat="server" Text="Link Generator" />
                        </h5>
                        <div class="ibox-content">
                            <div class="form-group">
                                <ce:Label ID="lblDataChangeLinkGen" runat="server" ForeColor="Red"></ce:Label>
                            </div>
                            <local:DataChangeLinkGenControl runat="server" ID="dataChangeLinkGenControl" />
                            <div class="form-group">
                                <ce:LinkButton runat="server" ID="btnDataChangeLinkGenCancel" OnClick="btnDataChangeLinkGenCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
