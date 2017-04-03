<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormDataView.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataView" %>

<%@ Register TagPrefix="local" TagName="FormDataControl" Src="~/Controls/User/FormDataControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="form-group">
                    <div class="col-lg-12">
                        <ce:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" ToolTip="Save" CssClass="btn btn-success fa fa-save" OnClientClick="onSave()" />
                        <ce:LinkButton runat="server" ID="btnSubmit" OnClick="btnSubmit_OnClick" ToolTip="Submit" CssClass="btn btn-warning fa fa-share-square-o" />
                        <ce:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_OnClick" ToolTip="Close" CssClass="btn btn-warning fa fa-close" />
                        <asp:Label runat="server" ID="lblStatusDesc" ForeColor="Red" />
                    </div>
                </div>
                <asp:Panel runat="server" ID="pnlErrors" Style="padding: 5px;">
                    <asp:Repeater runat="server" ID="rptErrors">
                        <ItemTemplate>
                            <div>
                                <ce:Label runat="server" ForeColor="Red" Font-Size="10" Text='<%# Eval("Item") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <local:FormDataControl runat="server" ID="formDataControl" OnCommand="formDataControl_OnCommand" />
            </div>
        </div>
    </div>
</asp:Content>

