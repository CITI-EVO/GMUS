<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditForm.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditForm" %>

<%@ Register Src="~/Controls/Management/FormControl.ascx" TagPrefix="local" TagName="FormControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ID="btnSave" ToolTip="Save" OnClick="btnSave_OnClick" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ID="btnPreview" ToolTip="Preview" OnClick="btnPreview_OnClick" CssClass="btn btn-info fa fa-picture-o" />
                            <ce:LinkButton runat="server" ID="btnCancel" ToolTip="Close" OnClick="btnCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                    <local:FormControl runat="server" ID="formControl" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

