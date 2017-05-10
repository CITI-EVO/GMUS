<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FormsControl" %>
<div class="table-responsive">
    <div class="dataTables_wrapper form-inline dt-bootstrap">
        <ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" AutoGenerateColumns="False" EnableViewState="False"
            CssClass="tableStd table table-striped table-bordered table-hover"  data-page-size="8" data-filter="#filter" Property="{FormsModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField >
                    <ItemStyle Width="150" /> 
                    <ItemTemplate>
                        <ce:LinkButton runat="server" ID="btnPreview" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnPreview_OnCommand" CssClass="btn btn-info btn-sm fa fa-picture-o"/>
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm fa fa-search"/>
                        <ce:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnCopy" ToolTip="Copy" CommandArgument='<%# Eval("ID") %>' OnCommand="btnCopy_OnCommand" CssClass="btn btn-primary btn-sm fa fa-copy" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                        <ce:LinkButton runat="server" ID="btnFiles" ToolTip="Files" CommandArgument='<%# Eval("ID") %>' OnCommand="btnFiles_OnCommand" CssClass="btn btn-primary btn-sm fa fa-upload" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o"  OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
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
                        <asp:Label runat="server" Text='<%# Eval("Number") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Order Index"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("OrderIndex") %>' />
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
