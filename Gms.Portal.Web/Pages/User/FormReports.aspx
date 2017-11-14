<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormReports.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
        <div>
        <div class="ibox float-e-margins">
            <div class="ibox-title" style="background-color: #4d608a;">
                <h5 style="color: white;">
                    <ce:Label runat="server">Reports</ce:Label>
                </h5>
            </div>
            <div class="ibox-content">
                <div>
                    <div class="form-group">
                        <ce:Label runat="server" CssClass="col-lg-2 control-label">Logic</ce:Label>
                        <div class="col-lg-10">
                            <asp:DropDownList runat="server" ID="cbxLogics" CssClass="chosen-select" DataTextField="Name" DataValueField="ID">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-4">
                            <ce:LinkButton runat="server" ID="btnSearch" OnClick="btnSearch_OnClick" ToolTip="Search" CssClass="btn btn-primary fa fa-search" />
                            <ce:LinkButton runat="server" ToolTip="Export to Excel" ID="btnExport" OnClick="btnExport_OnClick" CssClass="btn btn-primary fa fa-file-excel-o" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="ibox float-e-margins">
            <div class="ibox-content">
                <div class="form-group">
                    <div class="table-responsive">
                        <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter">
                        </ce:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

