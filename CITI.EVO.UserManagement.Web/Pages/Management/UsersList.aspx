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
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <asp:LinkButton ID="btnBindData" runat="server" CssClass="btn btn-info fa fa-search" ToolTip="ძებნა" OnClick="btnBindData_Click" />
                <asp:LinkButton ID="btnNewUser" CssClass="btn btn-primary  fa fa-plus" ToolTip="მომხმარებლის დამატება" runat="server" OnClick="btnNewUser_Click" />
            </div>
            <div class="ibox-content">
                <div class="row">
                    <div class="col-md-12">
                        <asp:Panel ID="pnlFilters" runat="server">
                            <local:UsersFilterControl runat="server" ID="usersFilterControl" />

                            <asp:Label runat="server" Style="padding: 4px 0 0 7px;" ID="lblError" />
                        </asp:Panel>

                    </div>
                </div>
            </div>
            <div class="ibox-content">
                <div class="row">
                    <div class="col-md-12">
                        <local:UsersControl runat="server" ID="usersControl"
                            OnEdit="usersControl_OnEdit"
                            OnView="usersControl_OnView"
                            OnDelete="usersControl_OnDelete"
                            OnNewMessage="usersControl_OnNewMessage"
                            OnSetAttribute="usersControl_OnSetAttribute"
                            OnViewAttributes="usersControl_OnViewAttributes" />
                    </div>
                </div>
            </div>

        </div>
    </div>
    <asp:Panel ID="pnlUser" runat="server" Style="display: none" DefaultButton="btUserOK">
        <asp:Button ID="btUserFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeUserForm" runat="server" PopupControlID="pnlUser" TargetControlID="btUserFake" />

        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-body">
                    <div>მომხმარებლის დამატება</div>
                    <div>
                        <asp:Label ID="lblUserError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="row">

                        <div style="padding-bottom: 15px;">
                            <asp:Panel runat="server" ID="pnlCreateUser">
                                <local:CreateUserControl runat="server" ID="createUserControl" OnDataChanged="createUserControl_OnDataChanged" />
                            </asp:Panel>
                        </div>
                        <div class="col-sm-12">
                            <asp:LinkButton ID="btUserOK" CssClass="btn btn-success fa fa-floppy-o" ToolTip="შენახვა" runat="server" OnClick="btnUserOK_Click" />

                            <asp:LinkButton ID="btUserCancel" class="btn btn-warning fa fa-close" ToolTip="დახურვა" runat="server" OnClick="btUserCancel_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSetAttributes" runat="server" Style="display: none;" DefaultButton="btSetAttributesOK">
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
                    <asp:LinkButton CssClass="icon" ID="btSetAttributesCancel" class="btn btn-warning fa fa-close" ToolTip="დახურვა" runat="server" OnClick="btSetAttributesCancel_OnClick" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlViewAttributes" runat="server" Style="display: none;" CssClass="modalWindow" DefaultButton="btViewAttributesCancel">
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
                    <asp:LinkButton ID="btViewAttributesCancel" class="btn btn-warning fa fa-close" ToolTip="დახურვა" runat="server" OnClick="btViewAttributesCancel_OnClick"/>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
