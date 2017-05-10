<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormDataGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormDataGridControl" %>
<div class="table-responsive">
    <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="FormDataGridModel.DataView=DataSource, Mode=Assigne" OnRowDataBound="gridView_OnRowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div style="white-space: nowrap;">
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm fa fa-search" />
                        <ce:LinkButton runat="server" ID="btnInspect" ToolTip="Inspect" Visible='<%# GetInspectVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnInspect_OnCommand" CssClass="btn btn-info btn-sm fa fa-eye" />
                        <ce:LinkButton runat="server" ID="btnReview" ToolTip="Review"  Visible='<%# GetReviewVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnReview_OnCommand" CssClass="btn btn-primary btn-sm fa fa-frown-o" />
                        <ce:LinkButton runat="server" ID="btnEdit" ToolTip="Edit"  Visible='<%# GetEditVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnPrint" ToolTip="Print"  Visible='<%# GetPrintVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>'  OnCommand="btnPrint_OnCommand" CssClass="btn btn-primary btn-sm fa fa-print" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete"  Visible='<%# GetDeleteVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')"  />
                        <ce:LinkButton runat="server" ID="btnStatus" ToolTip="Change status" Visible='<%# GetStatusVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnStatus_OnCommand" CssClass="btn btn-warning btn-sm fa fa-cog" />
                        <ce:LinkButton runat="server" ID="btnAssigne" ToolTip="Assigne users" Visible='<%# GetAssigneVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnAssigne_OnCommand" CssClass="btn btn-info btn-sm fa fa-address-book-o" />
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
