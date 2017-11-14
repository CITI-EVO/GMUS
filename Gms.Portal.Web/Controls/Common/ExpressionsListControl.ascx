<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExpressionsListControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.ExpressionsListControl" %>

<%@ Register Src="~/Controls/Common/ExpressionControl.ascx" TagPrefix="local" TagName="ExpressionControl" %>

<div>
    <div>
        <ce:LinkButton runat="server" ID="btnNew" OnClick="btnAdd_OnClick" ToolTip="New" CssClass="btn btn-primary fa fa-plus" />
    </div>
    <div>
        <ce:GridView ID="gvExpressions" runat="server" TableSectionHeader="True" TableSectionFooter="True" AutoGenerateColumns="False" Width="100%" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <ce:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" CommandArgument='<%# Eval("Key") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-xs fa fa-edit" />
                        <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("Key") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-xs fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Expression" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Expression") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="OutputType" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("OutputType") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </ce:GridView>
    </div>
</div>
<div>
    <ce:ModalPopup runat="server" ID="mpeExpression" CssClass="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server">Expression</ce:Label></h5>
                </div>
                <div class="modal-body">
                    <local:ExpressionControl runat="server" ID="expressionControl" />
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnSave" ToolTip="Save" OnClick="btnSave_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnCancel" ToolTip="Close" OnClick="btnCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>
