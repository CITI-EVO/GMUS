<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NamedExpressionsListControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.NamedExpressionsListControl" %>

<%@ Register Src="~/Controls/Common/NamedExpressionControl.ascx" TagPrefix="local" TagName="NamedExpressionControl" %>

<div>
    <div>
        <asp:LinkButton runat="server" ID="btnNew" OnClick="btnAdd_OnClick" CssClass="btn btn-primary">
            <asp:Label runat="server" CssClass="fa fa-plus"></asp:Label>
        </asp:LinkButton>
    </div>
    <div>
        <asp:GridView ID="gvExpressions" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnEdit" CommandArgument='<%# Eval("Key") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-xs">
                            <asp:Label runat="server" CssClass="fa fa-edit"/>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnDelete" CommandArgument='<%# Eval("Key") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-xs" OnClientClick="return confirm('Are you sure you want to delete?');">
                            <asp:Label runat="server" CssClass="fa fa-trash-o"/>
                        </asp:LinkButton>
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
        </asp:GridView>
    </div>
</div>
<div>
    <asp:Panel runat="server" ID="pnlExpression">
        <asp:Button runat="server" ID="btnExpressionFake" Style="display: none" />
        <act:ModalPopupExtender runat="server" ID="mpeExpression" TargetControlID="btnExpressionFake"
            Enabled="true" BackgroundCssClass="modalBackground" PopupControlID="pnlExpression"
            CancelControlID="btnCancel" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <div class="ibox float-e-margins">
                            <div class="ibox-title">
                                <h5>
                                    <ce:Label runat="server">Expression</ce:Label>
                                </h5>
                            </div>
                            <div class="ibox-content">
                                <local:NamedExpressionControl runat="server" ID="namedExpressionControl" />
                                <div class="form-group">
                                    <asp:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" CssClass="btn btn-success">
                                        <asp:Label runat="server" CssClass="fa fa-save"/>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-warning">
                                        <asp:Label runat="server" CssClass="fa fa-close"/>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
