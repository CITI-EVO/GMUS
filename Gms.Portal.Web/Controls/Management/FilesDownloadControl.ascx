<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilesDownloadControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FilesDownloadControl" %>
<div class="table-responsive">
    <div class="dataTables_wrapper form-inline dt-bootstrap">
        <ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" TableSectionFooter="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
            CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{FilesModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <ce:LinkButton runat="server" ID="btnDownload" ToolTip="Download" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDownload_OnCommand" CssClass="btn btn-danger btn-sm fa fa-download" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Name" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Description" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="FileName" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("FileName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </ce:GridView>
    </div>
</div>
