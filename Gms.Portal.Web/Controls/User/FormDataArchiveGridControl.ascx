<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormDataArchiveGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormDataArchiveGridControl" %>

<ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{FormDataArchiveGridModel.DataView=DataSource, Mode=Assigne}" OnRowDataBound="gridView_OnRowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <div class="dropdown">
                    <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                    <div class="dropdown-content">
                        <asp:HyperLink runat="server" ID="btnView" ToolTip="View" NavigateUrl='<%# GetViewUrl(Container.DataItem) %>' CssClass="btn btn-info btn-sm fa fa-search" Target="_blank" />
                        <asp:HyperLink runat="server" ID="btnPrint" ToolTip="Print" NavigateUrl='<%# GetPrintUrl(Container.DataItem) %>' Visible='<%# GetPrintVisible(Container.DataItem) %>' CssClass="btn btn-primary btn-sm fa fa-print" Target="_blank" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='ID Number' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("IDNumber") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Date Of Create' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DateCreated") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Date Of Submit' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DateOfSubmit") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Form Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# GetFormName(Container.DataItem) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
