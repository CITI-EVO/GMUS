<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProjectsList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.ProjectsList" %>

<%@ Register TagPrefix="local" TagName="ProjectControl" Src="~/Controls/ProjectControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ProjectsControl" Src="~/Controls/ProjectsControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <ce:ImageLinkButton runat="server" CssClass="btn btn-primary  fa fa-plus" ID="btNewProject" ToolTip="მოდულის დამატება" OnClick="btNewProject_Click" />
            </div>
            <div class="ibox-content">
                <div class="row">
                    <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
                        <ContentTemplate>
                            <local:ProjectsControl runat="server" ID="projectsControl" OnEdit="projectsControl_OnEdit" OnDelete="projectsControl_OnDelete" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlProject" runat="server" Style="display: none;" CssClass="modalWindow" DefaultButton="btnOK">
        <asp:Button ID="btnProjectFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeProject" TargetControlID="btnProjectFake" PopupControlID="pnlProject" CancelControlID="btnCancel" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />

        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <ce:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></ce:Label>
                        <p>
                            <ce:Label runat="server">მოდულის დამატება</ce:Label>
                        </p>
                        <div class="ibox-content form-horizontal">
                            <div class="col-sm-12">
                                <local:ProjectControl runat="server" ID="projectControl" />
                            </div>
                            <div class="col-sm-12">
                                <ce:ImageLinkButton ID="btnOK" runat="server" CssClass="btn btn-success fa fa-floppy-o" ToolTip="შენახვა" OnClick="btnOK_OnClick" />

                                <ce:ImageLinkButton ID="btnCancel" CssClass="btn btn-warning  fa fa-times" runat="server" ToolTip="დახურვა" OnClick="btnCancel_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
