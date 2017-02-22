<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FormsControl" %>
<div class="table-responsive">
    <div class="dataTables_wrapper form-inline dt-bootstrap">
        <ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" AutoGenerateColumns="False" EnableViewState="False"
            CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{FormsModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm">
                            <asp:Label runat="server" CssClass="fa fa-file"/>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm">
                            <asp:Label runat="server" CssClass="fa fa-edit"/>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm">
                            <asp:Label runat="server" CssClass="fa fa-trash-o"/>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Name" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ce:Label runat="server" Text='<%# Eval("Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Number" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ce:Label runat="server" Text='<%# Eval("Number") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="OrderIndex"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ce:Label runat="server" Text='<%# Eval("OrderIndex") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Visible" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ce:Label runat="server" Text='<%# Eval("Visible") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </ce:GridView>
    </div>
</div>
