<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormDataGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormDataGridControl" %>
<div class="table-responsive">
    <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="FormDataGridModel.DataView=DataSource, Mode=Assigne" OnRowDataBound="gridView_OnRowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="dropdown">
                        <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                        <div class="dropdown-content">
                            <asp:HyperLink runat="server" ID="lnkView" ToolTip="View" NavigateUrl='<%# GetViewUrl(Container.DataItem) %>'   CssClass="btn btn-success btn-xs fa fa-search" />
                            <asp:HyperLink runat="server" ID="btnInspect" ToolTip="Inspect" Visible='<%# GetInspectVisible(Container.DataItem) %>' NavigateUrl='<%# GetInspectUrl(Container.DataItem) %>' CssClass="btn btn-info btn-xs fa fa-eye" />
                            <asp:HyperLink runat="server" ID="btnReview" ToolTip="Review" Visible='<%# GetReviewVisible(Container.DataItem) %>'  NavigateUrl='<%# GetReviewUrl(Container.DataItem) %>' CssClass="btn btn-primary btn-xs fa fa-frown-o" />
                            <asp:HyperLink runat="server" ID="lnkEdit" ToolTip="Edit" Visible='<%# GetEditVisible(Container.DataItem) %>' NavigateUrl='<%# GetEditUrl(Container.DataItem) %>' CssClass="btn btn-primary btn-xs fa fa-edit" />
                            <ce:LinkButton runat="server" ID="btnPrint" ToolTip="Print" Visible='<%# GetPrintVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnPrint_OnCommand" CssClass="btn btn-primary btn-xs fa fa-print" />
                            <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" Visible='<%# GetDeleteVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-xs fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                            <ce:LinkButton runat="server" ID="btnStatus" ToolTip="Change status" Visible='<%# GetStatusVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnStatus_OnCommand" CssClass="btn btn-warning btn-xs fa fa-cog" />
                            <asp:HyperLink runat="server" ID="btnAssigne" ToolTip="Assigne users" Visible='<%# GetAssigneVisible(Container.DataItem) %>' NavigateUrl='<%# GetAssigneUrl(Container.DataItem) %>' CssClass="btn btn-info btn-xs fa fa-address-book-o" />
                            <asp:HyperLink runat="server" ID="lnkHistory" ToolTip="History" Visible='<%# GetHistoryVisible(Container.DataItem) %>' NavigateUrl='<%# GetHistoryUrl(Container.DataItem) %>' CssClass="btn btn-success btn-xs fa fa-database" />
                            <asp:HyperLink runat="server" ID="lnkMonitoring" ToolTip="Monitoring" Visible='<%# GetMonitoringVisible(Container.DataItem) %>' NavigateUrl='<%# GetMonitoringUrl(Container.DataItem) %>' CssClass="btn btn-success btn-xs fa fa-line-chart" />
                            <ce:LinkButton runat="server" ID="btnAcceptChanges" ToolTip="Accept Restore" Visible='<%# GetAcceptChangesVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnAcceptChanges_OnCommand" CssClass="btn btn-primary btn-xs fa fa-check-square-o" />
                            <ce:LinkButton runat="server" ID="btnRestoreChanges" ToolTip="Restore changes" Visible='<%# GetRestoreChangesVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnRestoreChanges_OnCommand" CssClass="btn btn-danger btn-xs fa fa-mail-reply-all" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID Number">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='ID Number' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("IDNumber") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status Name">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Status Name' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetStatusName(Eval("StatusID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status Change Date">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Status Change Date' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetStatusDate(Container.DataItem) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Days Left">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Days Left' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetDaysLeft(Container.DataItem) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Submit Date">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Submit Date' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetSubmitDate(Container.DataItem) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="User Name">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='User Name' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# GetUserName(Eval("UserID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="User Status">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='User Status' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# GetUserStatus(Eval("UserID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ce:GridView>
</div>
