<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditCollection.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditCollection" %>

<%@ Register Src="~/Controls/Management/CollectionControl.ascx" TagPrefix="local" TagName="CollectionControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ToolTip="Save" ID="btnSaveCollection" OnClick="btnSaveCollection_OnClick" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ToolTip="Close" ID="btnCancelCollection" OnClick="btnCancelCollection_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                    <local:CollectionControl runat="server" ID="collectionControl" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

