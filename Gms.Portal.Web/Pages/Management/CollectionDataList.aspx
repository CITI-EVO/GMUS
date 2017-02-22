<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CollectionDataList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.CollectionDataList" %>

<%@ Register Src="~/Controls/Management/CollectionDatasControl.ascx" TagPrefix="lmis" TagName="CollectionDatasControl" %>

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
                    <div class="ibox-tools">
                        <asp:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary">
                            <asp:Label runat="server" CssClass="fa fa-plus"/>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnClear" ToolTip="Clear" OnClick="btnClear_OnClick" CssClass="btn btn-danger">
                            <asp:Label runat="server" CssClass="fa fa-trash-o"/>
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="ibox-content">
                    <lmis:CollectionDatasControl runat="server" ID="collectionDatasControl" OnEdit="collectionDatasControl_OnEdit" OnDelete="collectionDatasControl_OnDelete" OnView="collectionDatasControl_OnView" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>


