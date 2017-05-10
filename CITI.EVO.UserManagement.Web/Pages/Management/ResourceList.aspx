<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ResourceList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.ResourceList" %>

<%@ Register TagPrefix="local" TagName="ResourceControl" Src="~/Controls/ResourceControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ResourcesControl" Src="~/Controls/ResourcesControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ResourcesFilterControl" Src="~/Controls/Filters/ResourcesFilterControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <asp:LinkButton ID="btnSearch" runat="server" ToolTip="ძებნა" CssClass="btn btn-info fa fa-search" OnClick="btSearch_OnClick" />
                <asp:LinkButton ID="btNew" runat="server" ToolTip="რესურსის დამატება" CssClass="btn btn-primary fa fa-plus" OnClick="btnNew_OnClick" />
            </div>
            <div class="ibox-content">
                <div class="row">
                    <div class="col-md-12">
                        <local:ResourcesFilterControl runat="server" ID="resourcesFilterControl" />
                    </div>

                </div>
            </div>
            <div class="ibox-content">
                <div class="row">
                    <div class="col-md-12">
                        <local:ResourcesControl runat="server" ID="resourcesControl" OnEdit="resourcesControl_OnEdit" OnNew="resourcesControl_OnNew" OnDelete="resourcesControl_OnDelete" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:Panel ID="pnlResource" runat="server" Style="display: none;" CssClass="modalWindow" DefaultButton="btnOK">
        <asp:Button ID="btResourceFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeResource" runat="server" PopupControlID="pnlResource" BackgroundCssClass="modalBackground" TargetControlID="btResourceFake" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                            <div >
                                <h5>
                                    <asp:Label runat="server">რესურსი</asp:Label>
                                </h5>
                            </div>
                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>

                            <div class="ibox-content form-horizontal">
                                <div class="form-group">
                                    <local:ResourceControl runat="server" ID="resourceControl" />

                                </div>

                                <div class="form-group">
                                    <ce:LinkButton runat="server" ID="btnOK" OnClick="btnOK_Click" CssClass="btn btn-success fa fa-save" />
                                    <ce:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-warning fa fa-close" OnClick="btnCancel_OnClick" />
                                </div>
                            </div>
                    
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
