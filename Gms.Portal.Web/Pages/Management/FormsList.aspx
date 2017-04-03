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
                        <asp:Label runat="server"></asp:Label>
                    </h5>
                    <div class="btn-group">
                        <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                </div>
                <div class="ibox-content">
                    <local:FormsControl runat="server" ID="formsControl" OnEdit="formsControl_OnEdit" OnDelete="formsControl_OnDelete" OnView="formsControl_OnView" OnCopy="formsControl_OnCopy" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

