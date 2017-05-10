<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataApproveGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.DataApproveGridControl" %>
<div class="table-responsive">
    <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="FormDataGridModel.DataView=DataSource, Mode=Assigne" OnRowDataBound="gridView_OnRowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div style="white-space: nowrap;">
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm fa fa-search" />
                        <ce:LinkButton runat="server" ID="btnPrint" ToolTip="Print"  Visible='<%# GetPrintVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>'  OnCommand="btnPrint_OnCommand" CssClass="btn btn-primary btn-sm fa fa-print" />
                        <ce:LinkButton runat="server" ID="btnStatus" ToolTip="Change status" Visible='<%# GetStatusVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnStatus_OnCommand" CssClass="btn btn-warning btn-sm fa fa-cog" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Status' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetStatusName(Eval("StatusID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Status Change Date' />
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
                    <ce:Label runat="server" Text='User' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# GetUserName(Eval("UserID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ce:GridView>
</div>
