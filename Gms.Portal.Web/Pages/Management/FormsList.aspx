<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormsList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.FormsList" %>

<%@ Register Src="~/Controls/Management/FormsControl.ascx" TagPrefix="local" TagName="FormsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server">Forms</ce:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="form-group">
                        <local:FormsControl runat="server" ID="formsControl" OnPreview="formsControl_OnPreview" OnEdit="formsControl_OnEdit" OnDelete="formsControl_OnDelete" OnView="formsControl_OnView" OnCopy="formsControl_OnCopy" OnFiles="formsControl_OnFiles"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

