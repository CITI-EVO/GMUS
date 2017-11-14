<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringProjectsDataControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringProjectsDataControl" %>

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
    OnRowDataBound="gridView_OnRowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemStyle Width="150px"></ItemStyle>
            <ItemTemplate>
                <div class="dropdown">
                    <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                    <div class="dropdown-content">
                        <ce:LinkButton runat="server" ID="btnFiles" ToolTip="ფაილები" Visible='<%# GetFilesVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnFiles_OnCommand" CssClass="btn btn-success btn-sm fa fa-folder" />
                        <ce:LinkButton runat="server" ID="btnFlaw" ToolTip="ხარვეზი" Visible='<%# GetFlawVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnFlaw_OnCommand" CssClass="btn btn-warning btn-sm fa fa-cogs" />
                        <ce:LinkButton runat="server" ID="bntStatus" ToolTip="სტატუსი" Visible='<%# GetStatusVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="bntStatus_OnCommand" CssClass="btn btn-warning btn-sm fa fa-check-square-o" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="წაშლა" Visible='<%# GetDeleteVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Task">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Task' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Name") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Flaw">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Flaw' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetFlewsNames(Eval("FlawsID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Flew scores">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Flaw' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetFlewsScores(Eval("FlawsID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Comment">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Comment' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Comment") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Status' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Status")  %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ExpireDate">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Expire Date' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("ExpireDate")  %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Status">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Done Status' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DoneStatus")  %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Description">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Done Description' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DoneDescription")  %>' />
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
        <%--  <asp:TemplateField HeaderText="Accepted By">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Accepted By' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# GetUserName(Eval("AcceptUserID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Date Of Creation">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Date Of Creation' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("DateCreated") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
