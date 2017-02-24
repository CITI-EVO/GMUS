<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormDataGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormDataGridControl" %>
<div class="table-responsive">
    <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="FormDataGridModel.DataView=DataSource, Mode=Assigne">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div style="white-space: nowrap;">
                        <ce:LinkButton runat="server" ID="btnView" ToolTip="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm fa fa-search" />
                        <ce:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ce:GridView>
</div>
