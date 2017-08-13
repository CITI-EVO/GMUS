<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordUsersGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.RecordUsersGridControl" %>
<ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{RecordUsersGridModel.DataView=DataSource, Mode=Assigne}">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='User Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("UserName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Status Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("StatusName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Date Of Assigne' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DateOfAssigne") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Date Of Status' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DateOfStatus") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Scores' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Scores") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
