<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FullExpertDataGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FullExpertDataGridControl" %>
<ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{FullExpertDataGridModel.DataView=DataSource, Mode=Assigne}" OnRowDataBound="gridView_OnRowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <div class="dropdown">
                    <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                    <div class="dropdown-content">
                        <asp:HyperLink runat="server" ID="btnView" ToolTip="View" NavigateUrl='<%# GetViewUrl(Container.DataItem) %>' Visible='<%# GetViewVisible(Container.DataItem) %>' CssClass="btn btn-info btn-sm fa fa-search" Target="_blank" />
                        <asp:HyperLink runat="server" ID="btnPrint" ToolTip="Print" NavigateUrl='<%# GetPrintUrl(Container.DataItem) %>' Visible='<%# GetPrintVisible(Container.DataItem) %>' CssClass="btn btn-primary btn-sm fa fa-print" Target="_blank" />
                        <asp:HyperLink runat="server" ID="btnDetails" ToolTip="Details" NavigateUrl='<%# GetDetailsUrl(Container.DataItem) %>' Visible='<%# GetDetailsVisible(Container.DataItem) %>' CssClass="btn btn-primary btn-sm fa fa-list-alt" Target="_blank" />
                        <asp:HyperLink runat="server" ID="btnScores" ToolTip="Change scores" NavigateUrl='<%# GetChangeScoresUrl(Container.DataItem) %>' Visible='<%# GetScoresVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnScores_OnCommand" CssClass="btn btn-warning btn-sm fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnMessage" ToolTip="Send message" Visible='<%# GetMessageVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnMessage_OnCommand" CssClass="btn btn-warning btn-sm fa fa-comment" />
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
                <asp:Label runat="server" Text='<%# GetFormName(Container.DataItem) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Experts' />
            </HeaderTemplate>
            <ItemTemplate>
                <div><%# GetFormExperts(Container.DataItem) %></div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Scores' />
            </HeaderTemplate>
            <ItemTemplate>
                <div><%# GetSummaryScores(Container.DataItem) %></div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
