<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringBudgetDataControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringBudgetDataControl" %>

<asp:Panel runat="server" ID="pnlMessage">
    <asp:Label runat="server" ForeColor="Red" ID="lblError">
    </asp:Label>
</asp:Panel>
<ce:GridView ID="gvData"
    runat="server"
    AutoGenerateColumns="False"
    UseAccessibleHeader="True"
    TableSectionHeader="True"
    ShowHeaderWhenEmpty="True"
    EnableViewState="False"
    CssClass="tableStd table table-striped table-bordered table-hover"
    data-page-size="8"
    data-filter="#filter"
    OnRowDataBound="gridView_OnRowDataBound"
    Property="{MonitoringBudgetDataModel.DataView=DataSource, Mode=Assigne}">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <div class="dropdown">
                    <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                    <div class="dropdown-content">
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" Visible='<%# GetViewVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm fa fa-eye" />
                        <ce:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" Visible='<%# GetEditVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm fa fa-frown-o" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" Visible='<%# GetDeleteVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                        <ce:LinkButton runat="server" ID="btnAccept" ToolTip="Change status" Visible='<%# GetAcceptVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnAccept_OnCommand" CssClass="btn btn-warning btn-sm fa fa-cog" />
                        <ce:LinkButton runat="server" ID="btnReturn" ToolTip="Assigne users" Visible='<%# GetReturnVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnReturn_OnCommand" CssClass="btn btn-info btn-sm fa fa-address-book-o" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Task">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Task' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetTaskName(Eval("TaskID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Goal">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Goal' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetGoalName(Eval("Goal")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DateOfTransfer">
            <HeaderTemplate>
                <ce:Label runat="server" Text='DateOfTransfer' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DateOfTransfer") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Remain">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Remain' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Remain") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Incoming">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Incoming' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Incoming") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Outgoing">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Outgoing' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Outgoing") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Returned">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Returned' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Returned") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Accepted">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Accepted' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Accepted") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Created By">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Created By' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# GetUserName(Eval("CreateUserID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Accepted By">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Accepted By' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# GetUserName(Eval("AcceptUserID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Of Creation">
            <HeaderTemplate>
                <ce:Label runat="server" Text='User Status' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("DateCreated") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
