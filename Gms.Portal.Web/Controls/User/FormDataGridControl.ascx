<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormDataGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormDataGridControl" %>
<div class="table-responsive">
    <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="FormDataGridModel.DataView=DataSource, Mode=Assigne">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div style="white-space: nowrap;">
                        <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm">
                        <asp:Label runat="server" CssClass="fa fa-file"/>
                        <%--<ce:Label runat="server" CssClass="linkTitle" Text="View" />--%>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnEdit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm">
                        <asp:Label runat="server" CssClass="fa fa-edit"/>
                        <%--<ce:Label runat="server" CssClass="linkTitle" Text="Edit" />--%>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnDelete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm">
                        <asp:Label runat="server" CssClass="fa fa-trash-o"/>
                        <%--<ce:Label runat="server" CssClass="linkTitle" Text="Delete" />--%>
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ce:GridView>
</div>
