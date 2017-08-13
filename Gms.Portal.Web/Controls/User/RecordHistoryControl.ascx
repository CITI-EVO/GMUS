<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordHistoryControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.RecordHistoryControl" %>
<ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{RecordHistoryModel.DataView=DataSource, Mode=Assigne}">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="lnkDownload" NavigateUrl='<%# GetDownloadUrl(Container.DataItem) %>' CssClass="btn btn-primary btn-sm fa fa-download"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='User Name' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# GetUserName(Container.DataItem) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <ce:Label runat="server" Text='Date Of Creation' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("DateCreated") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>
