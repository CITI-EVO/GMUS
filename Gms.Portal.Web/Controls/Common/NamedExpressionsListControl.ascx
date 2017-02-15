<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NamedExpressionsListControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.NamedExpressionsListControl" %>

<%@ Register Src="~/Controls/Common/NamedExpressionControl.ascx" TagPrefix="local" TagName="NamedExpressionControl" %>

<div>
    <div>
        <ce:ImageLinkButton runat="server" ToolTip="New" DefaultImageUrl="~/App_Themes/Default/images/add.png" ID="btnAdd" OnCommand="btnAdd_OnClick" />
    </div>
    <div>
        <asp:GridView ID="gvExpressions" runat="server" AutoGenerateColumns="False"
            Width="100%">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <ce:ImageLinkButton runat="server" ToolTip="Edit" CommandArgument='<%# Eval("Key") %>' DefaultImageUrl="~/App_Themes/Default/images/edit.png" ID="btnEdit" OnCommand="btnEdit_OnCommand" />
                        <ce:ImageLinkButton runat="server" ToolTip="Delete" CommandArgument='<%# Eval("Key") %>' DefaultImageUrl="~/App_Themes/Default/images/delete.png" ID="btnDelete" OnCommand="btnDelete_OnCommand" OnClientClick="return confirm('Are you sure you want to delete?');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Name" DataField="Name" />
                <asp:BoundField HeaderText="Expression" DataField="Expression" />
                <asp:BoundField HeaderText="OutputType" DataField="OutputType" />
            </Columns>
        </asp:GridView>
    </div>
</div>
<div>
    <act:ModalPopupExtender runat="server" ID="mpeExpression" TargetControlID="btnExpressionFake"
        Enabled="true" BackgroundCssClass="modalBackground" PopupControlID="pnlExpression"
        CancelControlID="btnCancel" />
    <asp:Button runat="server" ID="btnExpressionFake" Style="display: none" />
    <asp:Panel runat="server" ID="pnlExpression">
        <div class="popup">
            <div class="popup_fieldset">
                <div class="popup-title"><ce:Label runat="server">Expression</ce:Label></div>
                <div class="title_separator"></div>
                <div class="box">
                    <local:NamedExpressionControl runat="server" ID="namedExpressionControl" />
                </div>
            </div>
            <div class="fieldsetforicons">
                <div class="left" style="padding-right: 10px">
                    <ce:ImageLinkButton runat="server" DefaultImageUrl="~/App_Themes/Default/Images/save.png" ID="btnSave" ToolTip="დამატება" OnClick="btnSave_OnClick" />
                </div>
                <div class="left">
                    <ce:ImageLinkButton runat="server" DefaultImageUrl="~/App_Themes/Default/Images/close.png" ID="btnCancel" ToolTip="დახურვა" />
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
