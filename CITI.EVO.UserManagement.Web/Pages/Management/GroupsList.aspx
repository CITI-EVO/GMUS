<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GroupsList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.GroupsList" %>

<%@ Register TagPrefix="local" TagName="GroupControl" Src="~/Controls/GroupControl.ascx" %>
<%@ Register TagPrefix="local" TagName="GroupsControl" Src="~/Controls/GroupsControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ObjectAttributeControl" Src="~/Controls/ObjectAttributeControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ObjectAttributesControl" Src="~/Controls/ObjectAttributesControl.ascx" %>
<%@ Register TagPrefix="local" TagName="SelectUserControl" Src="~/Controls/SelectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always" RenderMode="Block">
        <ContentTemplate>

            <div class="col-lg-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <h5>ჯგუფები </h5>
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
                            <local:GroupsControl ID="groupsControl" runat="server" OnDelete="groupsControl_OnDelete" OnEdit="groupsControl_OnEdit" OnNew="groupsControl_OnNew" OnAddUser="groupsControl_OnAddUser" OnViewAttributes="groupsControl_OnViewAttributes" OnSetAttribute="groupsControl_OnSetAttribute" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlGroup" runat="server" Style="display: none;" CssClass="modalWindow" Width="333px" DefaultButton="btGroupOK">
        <asp:UpdatePanel ID="upnlGroup" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="btGroupFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeGroup" TargetControlID="btGroupFake" PopupControlID="pnlGroup" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
                <div>
                    <asp:Label runat="server" ID="lblGroupError"></asp:Label>
                </div>
                <div>
                    <local:GroupControl ID="groupControl" runat="server" />
                </div>
                <div class="fieldsetforicons">
                    <div class="left">
                        <asp:LinkButton CssClass="icon" ID="btGroupOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btGroupOK_Click" />
                    </div>
                    <div class="right">
                        <asp:LinkButton CssClass="icon" ID="btGroupCancel" runat="server" ToolTip="დახურვა" Text="დახურვა" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlGroupAttributes" runat="server" Style="display: none;" CssClass="modalWindow" Width="333px" DefaultButton="btAttributeOK">
        <asp:UpdatePanel ID="upnlGroupAttributes" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="btGroupAttributesFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeGroupAttributes" TargetControlID="btGroupAttributesFake" PopupControlID="pnlGroupAttributes" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
                <div>
                    <local:ObjectAttributeControl ID="objectAttributeControl" runat="server" />
                </div>
                <div class="fieldsetforicons">
                    <div class="left">
                        <asp:LinkButton CssClass="icon" ID="btAttributeOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btAttributeOK_Click" />
                    </div>
                    <div class="right">
                        <asp:LinkButton CssClass="icon" ID="btAttributeCancel" runat="server" ToolTip="დახურვა" Text="დახურვა" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlViewGroupAttributes" runat="server" Style="display: none;" CssClass="modalWindow" Width="333px" DefaultButton="btViewAttributeOK">
        <asp:UpdatePanel ID="upnlViewGroupAttributes" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="btViewGroupAttributesFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeViewGroupAttributes" TargetControlID="btViewGroupAttributesFake" PopupControlID="pnlViewGroupAttributes" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
                <div>
                    <local:ObjectAttributesControl ID="objectAttributesControl" runat="server" />
                </div>
                <div class="fieldsetforicons">
                    <div class="left">
                        <asp:LinkButton CssClass="icon" ID="btViewAttributeOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btViewAttributeOK_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlUsers" runat="server" Style="display: none;" CssClass="modalWindow" Width="333px" DefaultButton="btUserOK">
        <asp:UpdatePanel ID="upnlUsers" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="btUsersFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeUsers" TargetControlID="btUsersFake" PopupControlID="pnlUsers" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
                <div>
                </div>
                <div>
                    <local:SelectUserControl ID="selectUserControl" runat="server" OnDataChanged="selectUserControl_OnDataChanged" />
                </div>
                <div class="fieldsetforicons">
                    <div class="left">
                        <asp:LinkButton CssClass="icon" ID="btUserOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btUserOK_Click" />
                    </div>
                    <div class="right">
                        <asp:LinkButton CssClass="icon" ID="btUserCancel" runat="server" ToolTip="დახურვა" Text="დახურვა" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <ce:MessageControl ID="ucMessage" runat="server" />
</asp:Content>
