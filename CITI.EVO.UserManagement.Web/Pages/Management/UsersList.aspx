<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="UsersList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.UsersList" %>

<%@ Register TagPrefix="local" TagName="UsersControl" Src="~/Controls/UsersControl.ascx" %>
<%@ Register TagPrefix="local" TagName="CreateUserControl" Src="~/Controls/CreateUserControl.ascx" %>
<%@ Register TagPrefix="local" TagName="UsersFilterControl" Src="~/Controls/Filters/UsersFilterControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ObjectAttributeControl" Src="~/Controls/ObjectAttributeControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ObjectAttributesControl" Src="~/Controls/ObjectAttributesControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">

    <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
        <ContentTemplate>
            <div class="col-lg-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <h5>ფილტრები </h5>
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

                            <asp:Panel ID="pnlFilters" runat="server">
                                <local:UsersFilterControl runat="server" ID="usersFilterControl" />

                                <asp:LinkButton ID="btnBindData" runat="server" CssClass="btn btn-primary fa fa-search" Text="ნახვა" OnClick="btnBindData_Click" />

                                <asp:Label runat="server" Style="padding: 4px 0 0 7px;" ID="lblError" />

                                <asp:LinkButton ID="btnNewUser" CssClass="btn btn-primary fa fa-plus-circle" Text="მომხმარებლის დამატება" runat="server" OnClick="btnNewUser_Click" />
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>




            <div>
                <local:UsersControl runat="server" ID="usersControl"
                    OnEdit="usersControl_OnEdit"
                    OnView="usersControl_OnView"
                    OnDelete="usersControl_OnDelete"
                    OnNewMessage="usersControl_OnNewMessage"
                    OnSetAttribute="usersControl_OnSetAttribute"
                    OnViewAttributes="usersControl_OnViewAttributes" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlUser" runat="server" Style="display: none" DefaultButton="btUserOK">
        <asp:UpdatePanel ID="upnlUser" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btUserFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeUserForm" runat="server" PopupControlID="pnlUser" TargetControlID="btUserFake" />

                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-6 b-r">
                                    <h3 class="m-t-none m-b">Sign in</h3>

                                    <p>
                                        <asp:Label ID="lblUserError" runat="server" ForeColor="Red"></asp:Label>
                                    </p>


                                </div>
                                <div class="col-sm-6">
                                    <p>
                                        <asp:Panel runat="server" ID="pnlCreateUser">
                                            <local:CreateUserControl runat="server" ID="createUserControl" />
                                        </asp:Panel>
                                    </p>

                                </div>
                                <div class="col-sm-6">
                                    <asp:LinkButton ID="btUserOK" CssClass="icon" Text="შენახვა" ToolTip="შენახვა" runat="server" OnClick="btnUserOK_Click" />

                                    <asp:LinkButton ID="btUserCancel" CssClass="icon" Text="დახურვა" ToolTip="დახურვა" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlSetAttributes" runat="server" Style="display: none;" DefaultButton="btSetAttributesOK">
        <asp:UpdatePanel ID="upnlSetAttributes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btSetAttributesFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeSetAttributes" TargetControlID="btSetAttributesFake" PopupControlID="pnlSetAttributes" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />

                <div class="popup">
                    <div class="popup_fieldset">
                        <h2>
                            <asp:Label runat="server">ატრიბუტის დამატება</asp:Label>
                        </h2>
                        <asp:Label ID="lblSetAttributesError" runat="server" ForeColor="Red"></asp:Label>
                        <div class="title_separator"></div>
                        <div class="box">
                            <local:ObjectAttributeControl runat="server" ID="objectAttributeControl" />
                        </div>
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <asp:LinkButton CssClass="icon" runat="server" ID="btSetAttributesOK" Text="შენახვა" ToolTip="შენახვა" OnClick="btSetAttributesOK_Click" />
                        </div>
                        <div class="right">
                            <asp:LinkButton CssClass="icon" ID="btSetAttributesCancel" Text="დახურვა" ToolTip="დახურვა" runat="server" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlViewAttributes" runat="server" Style="display: none;" CssClass="modalWindow" DefaultButton="btViewAttributesCancel">
        <asp:UpdatePanel ID="upnlViewAttributes" runat="server">
            <ContentTemplate>
                <asp:Button ID="btViewAttributesFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeViewAttributes" TargetControlID="btViewAttributesFake" PopupControlID="pnlViewAttributes" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
                <div class="popup">
                    <div class="popup_fieldset">
                        <h2>
                            <asp:Label runat="server">ატრიბუტების ნახვა</asp:Label>
                        </h2>
                        <div class="title_separator"></div>
                        <div class="box">
                            <local:ObjectAttributesControl runat="server" ID="objectAttributesControl" />
                        </div>
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <asp:LinkButton ID="btViewAttributesCancel" CssClass="icon" Text="დახურვა" ToolTip="დახურვა" runat="server" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
