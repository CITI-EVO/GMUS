<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CategoriesList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.CategoriesList" %>

<%@ Register Src="~/Controls/Management/CategoryControl.ascx" TagPrefix="local" TagName="CategoryControl" %>
<%@ Register Src="~/Controls/Management/CategoriesControl.ascx" TagPrefix="local" TagName="CategoriesControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server" ID="lblTitle">Categories</ce:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="form-group">
                        <local:CategoriesControl runat="server" ID="categoriesControl" OnEdit="categoriesControl_OnEdit" OnDelete="categoriesControl_OnDelete" OnView="categoriesControl_OnView" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div>
        <asp:Panel ID="pnlCategory" runat="server" Style="display: none" DefaultButton="btCategoryOK">
            <asp:Button ID="btnCategoryFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeCategory" runat="server" PopupControlID="pnlCategory" BackgroundCssClass="modalBackground" TargetControlID="btnCategoryFake" />
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <h5>
                                <ce:Label runat="server" Text="Category" />
                            </h5>
                            <div class="ibox-content">
                                <div class="form-group">
                                    <ce:Label ID="lblFormCategory" runat="server" ForeColor="Red"></ce:Label>
                                </div>
                                <local:CategoryControl runat="server" ID="categoryControl" OnDataChanged="categoryControl_OnDataChanged" />
                                <div class="form-group">
                                    <ce:LinkButton runat="server" ID="btCategoryOK" OnClick="btnCategoryOK_Click" ToolTip="Save" CssClass="btn btn-success fa fa-save" />
                                    <ce:LinkButton runat="server" ID="btCategoryCancel" OnClick="btCategoryCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
