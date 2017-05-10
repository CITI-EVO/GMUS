<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GroupsList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.GroupsList" %>

<%@ Register TagPrefix="local" TagName="GroupControl" Src="~/Controls/GroupControl.ascx" %>
<%@ Register TagPrefix="local" TagName="GroupsControl" Src="~/Controls/GroupsControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ObjectAttributeControl" Src="~/Controls/ObjectAttributeControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ObjectAttributesControl" Src="~/Controls/ObjectAttributesControl.ascx" %>
<%@ Register TagPrefix="local" TagName="SelectUserControl" Src="~/Controls/SelectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>ჯგუფები </h5>
            </div>
            <div class="ibox-content">
                <div class="row">
                    <local:GroupsControl ID="groupsControl" runat="server" OnDelete="groupsControl_OnDelete" OnEdit="groupsControl_OnEdit" OnNew="groupsControl_OnNew" OnAddUser="groupsControl_OnAddUser" OnViewAttributes="groupsControl_OnViewAttributes" OnSetAttribute="groupsControl_OnSetAttribute" />
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlGroup" runat="server" Style="display: none;" DefaultButton="btGroupOK">
        <asp:Button ID="btGroupFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeGroup" TargetControlID="btGroupFake" PopupControlID="pnlGroup" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />


        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <p>
                            ჯგუფები
                        </p>
                        <asp:Label runat="server" ID="lblGroupError"></asp:Label>

                        <div class="ibox-content form-horizontal">
                            <div class="col-sm-12">
                                <local:GroupControl ID="groupControl" runat="server" />

                            </div>
                            <div class="col-sm-12">
                                <asp:LinkButton CssClass="btn btn-success fa fa-floppy-o" ID="btGroupOK" runat="server" ToolTip="შენახვა" OnClick="btGroupOK_Click" />
                                <asp:LinkButton CssClass="btn btn-warning fa fa-close" ID="btGroupCancel" runat="server" ToolTip="დახურვა" OnClick="btGroupCancel_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlGroupAttributes" runat="server" Style="display: none;" DefaultButton="btAttributeOK">
        <asp:Button ID="btGroupAttributesFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeGroupAttributes" TargetControlID="btGroupAttributesFake" PopupControlID="pnlGroupAttributes" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <p>
                            ატრიბუტი
                        </p>
                        <div class="ibox-content form-horizontal">
                            <div class="col-sm-12">
                                <local:ObjectAttributeControl ID="objectAttributeControl" runat="server" OnDataChanged="objectAttributeControl_OnDataChanged" />

                            </div>
                            <div class="col-sm-12">
                                <asp:LinkButton CssClass="btn btn-success fa fa-floppy-o" ID="btAttributeOK" runat="server" ToolTip="შენახვა" OnClick="btAttributeOK_Click" />
                                <asp:LinkButton CssClass="btn btn-warning fa fa-close" ID="btAttributeCancel" runat="server" ToolTip="დახურვა" OnClick="btAttributeCancel_OnClick" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlViewGroupAttributes" runat="server" Style="display: none;" DefaultButton="btViewAttributeOK">
        <asp:Button ID="btViewGroupAttributesFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeViewGroupAttributes" TargetControlID="btViewGroupAttributesFake" PopupControlID="pnlViewGroupAttributes" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <p>
                            ატრიბუტის ნახვა
                        </p>
                        <div class="ibox-content form-horizontal">
                            <div class="col-sm-12">
                                <local:ObjectAttributesControl ID="objectAttributesControl" runat="server" />

                            </div>
                            <div class="col-sm-12">
                                <asp:LinkButton CssClass="btn btn-success fa fa-floppy-o" ID="btViewAttributeOK" runat="server" ToolTip="შენახვა" OnClick="btViewAttributeOK_Click" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlUsers" runat="server" Style="display: none;" CssClass="modalWindow" DefaultButton="btUserOK">
        <asp:Button ID="btUsersFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeUsers" TargetControlID="btUsersFake" PopupControlID="pnlUsers" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <p>
                            მომხმარებლის დამატება
                        </p>
                        <div class="ibox-content">
                            <div class="col-sm-12 form-horizontal">
                                <local:SelectUserControl ID="selectUserControl" runat="server" OnDataChanged="selectUserControl_OnDataChanged" />

                            </div>
                            <div class="col-sm-12">
                                <asp:LinkButton CssClass="btn btn-success fa fa-floppy-o" ID="btUserOK" runat="server" ToolTip="შენახვა" OnClick="btUserOK_Click" />

                                <asp:LinkButton CssClass="btn btn-warning fa fa-close" ID="btUserCancel" runat="server" ToolTip="დახურვა" OnClick="btUserCancel_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <ce:MessageControl ID="ucMessage" runat="server" />
</asp:Content>
