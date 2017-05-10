<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CollectionsList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.CollectionsList" %>

<%@ Register Src="~/Controls/Management/CollectionsControl.ascx" TagPrefix="local" TagName="CollectionsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server">Lists</ce:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="form-group">
                        <local:CollectionsControl runat="server" ID="collectionsControl" OnEdit="collectionsControl_OnEdit" OnDelete="collectionsControl_OnDelete" OnView="collectionsControl_OnView" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

