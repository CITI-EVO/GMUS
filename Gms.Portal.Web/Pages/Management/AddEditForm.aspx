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
                        <asp:LinkButton runat="server" ID="btnSaveForm" ToolTip="Save" OnClick="btnSaveForm_OnClick" CssClass="btn btn-success">
                            <asp:Label runat="server" CssClass="fa fa-save"/>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnCancelForm" ToolTip="Close" OnClick="btnCancelForm_OnClick" CssClass="btn btn-warning">
                            <asp:Label runat="server" CssClass="fa fa-close"/>
                        </asp:LinkButton>
                    </div>
                    <local:FormControl runat="server" ID="formControl" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

