<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhoneNotificationsHistoryControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.PhoneNotificationsHistoryControl" %>
<div class="table-responsive">
    <div class="dataTables_wrapper form-inline dt-bootstrap">
        <ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" TableSectionFooter="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
            CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{NotificationsHistoryModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-danger btn-sm fa fa-search" />
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
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Message" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Body") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Send Date" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("DateCreated") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </ce:GridView>
    </div>
</div>
