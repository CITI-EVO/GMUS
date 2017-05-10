<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecipientsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.RecipientsControl" %>



<div class="table-responsive">
    <div class="dataTables_wrapper form-inline dt-bootstrap">
        <ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" TableSectionFooter="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
            CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{RecipientsModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View Notifications" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-primary btn-sm fa fa-archive" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Login Name" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("UserName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="First Name" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("FirstName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Last Name" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("LastName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Email" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Email") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Phone" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Phone") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </ce:GridView>
    </div>
</div>
