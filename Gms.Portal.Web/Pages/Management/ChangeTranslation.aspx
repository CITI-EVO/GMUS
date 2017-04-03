<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChangeTranslation.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.ChangeTranslation" %>

<%@ Register Src="~/Controls/Management/TranslationControl.ascx" TagPrefix="local" TagName="TranslationControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ID="btnSaveForm" ToolTip="Save" OnClick="btSave_Click" CssClass="btn btn-success fa fa-save" />
                        </div>
                    </div>
                    <div>
                        <asp:Label runat="server" ID="lblMessage"></asp:Label>
                    </div>
                    <local:TranslationControl runat="server" ID="translationControl" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

