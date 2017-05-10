<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttributesSchemasList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.AttributesSchemasList" %>

<%@ Register TagPrefix="local" TagName="AttributeFieldControl" Src="~/Controls/AttributeFieldControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AttributeSchemaControl" Src="~/Controls/AttributeSchemaControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AttributesSchemasControl" Src="~/Controls/AttributesSchemasControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">

    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>ატრიბუტების სქემები</h5>
            </div>
            <div class="ibox-content">
                <div class="row">
                    <local:AttributesSchemasControl ID="attributesSchemasControl" runat="server" OnDelete="attributesSchemasControl_OnDelete" OnEdit="attributesSchemasControl_OnEdit" OnNew="attributesSchemasControl_OnNew" />
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlAttributeSchema" runat="server" Style="display: none;" CssClass="modalWindow" Width="333px" DefaultButton="btAttributeSchemaOK">
        <asp:Button ID="btAttributeSchemaPopup" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeAttributeSchema" TargetControlID="btAttributeSchemaPopup" PopupControlID="pnlAttributeSchema" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <p>
                            ატრიბუტის დამატება
                        </p>
                        <asp:Label ID="lblAttributeSchemaError" runat="server" ForeColor="Red"></asp:Label>
                        <div class="ibox-content form-horizontal">
                            <div class="col-sm-12">
                                <local:AttributeSchemaControl ID="attributeSchemaControl" runat="server" />
                            </div>
                            <div class="col-sm-12">
                                <asp:LinkButton CssClass="btn btn-success fa fa-floppy-o" ID="btAttributeSchemaOK" runat="server" ToolTip="შენახვა" OnClick="btAttributeSchemaOK_Click" />
                                <asp:LinkButton CssClass="btn btn-warning fa fa-times" ID="btAttributeSchemaCancel" runat="server" ToolTip="დახურვა" OnClick="btAttributeSchemaCancel_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>
    <asp:Panel ID="pnlAttributeSchemaNode" runat="server" Style="display: none;" CssClass="modalWindow" Width="333px" DefaultButton="btAttributeSchemaNodeOK">
        <asp:Button ID="btAttributeSchemaNodePopup" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeAttributeSchemaNode" TargetControlID="btAttributeSchemaNodePopup" PopupControlID="pnlAttributeSchemaNode" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <p>
                            ატრიბუტის დამატება
                        </p>
                        <asp:Label runat="server" ForeColor="Red"></asp:Label>
                        <div class="ibox-content form-horizontal">
                            <div class="col-sm-12">
                                <local:AttributeFieldControl ID="attributeFieldControl" runat="server" />

                            </div>
                            <div class="col-sm-12">
                                <asp:LinkButton CssClass="btn btn-success fa fa-floppy-o" ID="btAttributeSchemaNodeOK" runat="server" ToolTip="შენახვა" OnClick="btAttributeSchemaNodeOK_Click" />

                                <asp:LinkButton CssClass="btn btn-warning fa fa-close" ID="btAttributeSchemaNodeCancel" runat="server" ToolTip="დახურვა" OnClick="btAttributeSchemaNodeCancel_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
