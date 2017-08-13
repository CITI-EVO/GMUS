<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormApproveDataGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormApproveDataGridControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdFormID" Property="{FormApproveDataGridModel.FormID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdFieldID" Property="{FormApproveDataGridModel.SourceField=Value}" />

<div class="table-responsive">
    <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{FormApproveDataGridModel.DataView=DataSource, Mode=Assigne}" OnRowDataBound="gridView_OnRowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="dropdown">
                        <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                        <div class="dropdown-content">
                            <asp:HyperLink runat="server" ID="btnView" ToolTip="View" NavigateUrl='<%# GetViewUrl(Container.DataItem) %>' CssClass="btn btn-info btn-sm fa fa-search" Target="_blank" />
                            <asp:HyperLink runat="server" ID="btnPrint" ToolTip="Print" NavigateUrl='<%# GetPrintUrl(Container.DataItem) %>' Visible='<%# GetPrintVisible(Container.DataItem) %>' CssClass="btn btn-primary btn-sm fa fa-print" Target="_blank"  />
                            <ce:LinkButton runat="server" ID="btnStatus" ToolTip="Change status" Visible='<%# GetStatusVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnStatus_OnCommand" CssClass="btn btn-warning btn-sm fa fa-cog" />
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
