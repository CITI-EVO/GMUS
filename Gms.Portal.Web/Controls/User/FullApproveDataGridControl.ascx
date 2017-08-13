<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FullApproveDataGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FullApproveDataGridControl" %>
<ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{FullApproveDataGridModel.DataView=DataSource, Mode=Assigne}" OnRowDataBound="gridView_OnRowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <div class="dropdown">
                    <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                    <div class="dropdown-content">
                        <asp:HyperLink runat="server" ID="btnView" ToolTip="View" NavigateUrl='<%# GetViewUrl(Container.DataItem) %>' CssClass="btn btn-info btn-sm fa fa-search" Target="_blank" />
                        <asp:HyperLink runat="server" ID="btnPrint" ToolTip="Print" NavigateUrl='<%# GetPrintUrl(Container.DataItem) %>' Visible='<%# GetPrintVisible(Container.DataItem) %>' CssClass="btn btn-primary btn-sm fa fa-print" Target="_blank" />
                        <ce:LinkButton runat="server" ID="btnAccept" ToolTip="Accept" Visible='<%# GetAcceptVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnAccept_OnCommand" CssClass="btn btn-primary btn-sm fa fa-check" />
                        <ce:LinkButton runat="server" ID="btnReject" ToolTip="Reject" Visible='<%# GetRejectVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnReject_OnCommand" CssClass="btn btn-danger btn-sm fa fa-close" />
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
                <ce:Label runat="server" Text='Status' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetStatusName(Container.DataItem) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Status Date' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetStatusDate(Container.DataItem) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Days left' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetDaysLeft(Container.DataItem) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='User Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# GetUserName(Container.DataItem) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Form Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("FormName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Field Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("FieldName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Organization Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("OrganizationName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
