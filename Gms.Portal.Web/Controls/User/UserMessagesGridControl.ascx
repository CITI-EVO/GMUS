<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserMessagesGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.UserMessagesGridControl" %>

<ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{UserMessagesModel.List=DataSource, Mode=Assigne}" OnRowDataBound="gridView_OnRowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <div class="dropdown">
                    <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                    <div class="dropdown-content">
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-warning btn-sm fa fa-comments-o" />
                        <ce:LinkButton runat="server" ID="btnApprove" ToolTip="Approve" CommandArgument='<%# Eval("ID") %>' OnCommand="btnApprove_OnCommand" CssClass="btn btn-primary btn-sm fa fa-check-circle" Visible='<%# GetApproveVisible() %>' />
                        <ce:LinkButton runat="server" ID="btnReject" ToolTip="Reject" CommandArgument='<%# Eval("ID") %>' OnCommand="btnReject_OnCommand" CssClass="btn btn-danger btn-sm fa fa-exclamation-circle" Visible='<%# GetRejectVisible() %>' />
                        <ce:LinkButton runat="server" ID="btnMarkAsRead" ToolTip="Mark as read" CommandArgument='<%# Eval("ID") %>' OnCommand="btnMarkAsRead_OnCommand" CssClass="btn btn-primary btn-sm fa fa-envelope-open-o" Visible='<%# GetMarkAsReadVisible() %>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Subject' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Subject") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Description' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Description") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Form Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# GetFormName(Eval("FormID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Message Date' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("DateCreated") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='From User' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# GetUserName(Eval("FromUserID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='To User' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetUserName(Eval("ToUserID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Status User' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetUserName(Eval("StatusUserID")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Readed' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Readed") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
