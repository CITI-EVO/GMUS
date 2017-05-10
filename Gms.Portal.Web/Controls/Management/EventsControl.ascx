<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EventsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.EventsControl" %>
<div class="table-responsive">
    <div class="dataTables_wrapper form-inline dt-bootstrap">
        <ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" TableSectionFooter="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
                     CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{EventsModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-primary btn-sm fa fa-search" />
                        <ce:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
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
            </Columns>
        </ce:GridView>
    </div>
</div>
