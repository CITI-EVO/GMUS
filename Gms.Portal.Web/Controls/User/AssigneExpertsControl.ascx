<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AssigneExpertsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.AssigneExpertsControl" %>
<ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{AssigneExpertsModel.DataView=DataSource, Mode=Assigne}" OnRowDataBound="gridView_OnRowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <div class="dropdown">
                    <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                    <div class="dropdown-content">
                        <asp:HyperLink runat="server" ID="btnView" ToolTip="Profile" Visible='<%# GetViewVisible(Container.DataItem) %>' NavigateUrl='<%# GetProfileUrl(Container.DataItem) %>' CssClass="btn btn-primary btn-sm fa fa-search" />
                        <ce:LinkButton runat="server" ID="btnAttach" ToolTip="Attach" Visible='<%# GetAttachVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnAttach_OnCommand" CssClass="btn btn-primary btn-sm fa fa-plus" />
                        <ce:LinkButton runat="server" ID="btnDetach" ToolTip="Detach" Visible='<%# GetDetachVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnDetach_OnCommand" CssClass="btn btn-danger btn-sm fa fa-minus" />
                        <ce:LinkButton runat="server" ID="btnStatus" ToolTip="Status" Visible='<%# GetStatusVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnStatus_OnCommand" CssClass="btn btn-primary btn-sm fa fa-cog" />
                        <ce:LinkButton runat="server" ID="btnEmail" ToolTip="Email" Visible='<%# GetEmailVisible(Container.DataItem) %>' CommandArgument='<%# GetCommandArg(Container.DataItem) %>' OnCommand="btnEmail_OnCommand" CssClass="btn btn-primary btn-sm fa fa-paper-plane-o" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Status' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("StatusName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='First Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("FirstName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Last Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("LastName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='User Status' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("UserStatus") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Email' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Email") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Email status' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("EmailStatus") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Directions' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Directions") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Projects' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Projects") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
