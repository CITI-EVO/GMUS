<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttributesSchemasList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.AttributesSchemasList" %>

<%@ Register TagPrefix="local" TagName="AttributeFieldControl" Src="~/Controls/AttributeFieldControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AttributeSchemaControl" Src="~/Controls/AttributeSchemaControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AttributesSchemasControl" Src="~/Controls/AttributesSchemasControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always" RenderMode="Block">
        <ContentTemplate>
            
            
                 <div class="col-lg-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <h5>ატრიბუტების სქემები</h5>
                        <div class="ibox-tools">
                            <a class="collapse-link">
                                <i class="fa fa-chevron-up"></i>
                            </a>
                            <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                                <i class="fa fa-wrench"></i>
                            </a>
                            <ul class="dropdown-menu dropdown-user">
                                <li><a href="#">Config option 1</a>
                                </li>
                                <li><a href="#">Config option 2</a>
                                </li>
                            </ul>
                            <a class="close-link">
                                <i class="fa fa-times"></i>
                            </a>
                        </div>
                    </div>
                    <div class="ibox-content">
                        <div class="row">
                                    <local:AttributesSchemasControl ID="attributesSchemasControl" runat="server" OnDelete="attributesSchemasControl_OnDelete" OnEdit="attributesSchemasControl_OnEdit" OnNew="attributesSchemasControl_OnNew" />

                        </div>
                    </div>
                </div>
            </div>
          

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlAttributeSchema" runat="server" Style="display: none;" CssClass="modalWindow"
        Width="333px" DefaultButton="btAttributeSchemaOK">
        <asp:UpdatePanel ID="upnlAttributeSchema" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btAttributeSchemaPopup" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeAttributeSchema" TargetControlID="btAttributeSchemaPopup" PopupControlID="pnlAttributeSchema" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />

                <div class="popup">
                    <div class="popup_fieldset">
                        <h2></h2>
                        <asp:Label ID="lblAttributeSchemaError" runat="server" ForeColor="Red"></asp:Label>
                        <div class="title_separator"></div>
                        <local:AttributeSchemaControl ID="attributeSchemaControl" runat="server" />

                        <div class="box">
                            <h3></h3>
                            <div class="box_body">
                            </div>
                        </div>
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btAttributeSchemaOK_Click" />
                        </div>
                        <div class="right">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaCancel" runat="server" ToolTip="დახურვა" Text="დახურვა" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlAttributeSchemaNode" runat="server" Style="display: none;" CssClass="modalWindow"
        Width="333px" DefaultButton="btAttributeSchemaNodeOK">
        <asp:UpdatePanel ID="upnlAttributeSchemaNode" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btAttributeSchemaNodePopup" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeAttributeSchemaNode" TargetControlID="btAttributeSchemaNodePopup" PopupControlID="pnlAttributeSchemaNode" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />

                <div class="popup">
                    <div class="popup_fieldset">
                        <h2></h2>
                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                        <div class="title_separator"></div>
                        <local:AttributeFieldControl ID="attributeFieldControl" runat="server" />

                        <div class="box">
                            <h3></h3>
                            <div class="box_body">
                            </div>
                        </div>
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaNodeOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btAttributeSchemaNodeOK_Click" />
                        </div>
                        <div class="right">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaNodeCancel" runat="server" ToolTip="დახურვა" Text="დახურვა" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
