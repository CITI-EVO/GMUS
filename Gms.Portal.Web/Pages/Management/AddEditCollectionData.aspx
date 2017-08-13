<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditCollectionData.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditCollectionData" %>

<%@ Register Src="~/Controls/Management/CollectionDataControl.ascx" TagPrefix="local" TagName="CollectionDataControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ToolTip="Save" ID="btnSave" OnClick="btnSave_OnClick" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ToolTip="Cancel" ID="btnCancel" OnClick="btnCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                    <local:CollectionDataControl runat="server" ID="collectionDataControl" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

