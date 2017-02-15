<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditLogic.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditLogic" %>

<%@ Register Src="~/Controls/Management/LogicControl.ascx" TagPrefix="lmis" TagName="LogicControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div style="margin-bottom: 60px;">
        <div class="left" style="padding-right: 10px;">
            <ce:ImageLinkButton runat="server" DefaultImageUrl="~/App_Themes/Default/Images/save.png" ID="btnSaveLogic" ToolTip="დამატება" OnClick="btnSaveLogic_OnClick" />
        </div>
        <div class="left">
            <ce:ImageLinkButton runat="server" DefaultImageUrl="~/App_Themes/Default/Images/close.png" ID="btnCancelLogic" ToolTip="დახურვა" OnClick="btnCancelLogic_OnClick" />
        </div>
        <div>
            <ce:ImageLinkButton runat="server" DefaultImageUrl="~/App_Themes/Default/Images/view.png" ID="btnPreview" ToolTip="გადახედვა" OnClick="btnPreview_OnClick" />
        </div>
    </div>
    <div>
        <lmis:LogicControl runat="server" ID="logicControl" />
    </div>
    <div>
        <act:ModalPopupExtender runat="server" ID="mpeAddEdit" TargetControlID="btnAddEditFake"
            Enabled="true" BackgroundCssClass="modalBackground" PopupControlID="pnlAddEdit"
            CancelControlID="btnCancel" />
        <asp:Button runat="server" ID="btnAddEditFake" Style="display: none" />
        <asp:Panel runat="server" ID="pnlAddEdit">
            <div class="popup">
                <div class="popup_fieldset">
                    <div class="popup-title">
                        <ce:Label runat="server">Data</ce:Label>
                    </div>
                    <div class="title_separator"></div>
                    <div class="box">
                        <div>
                            <ce:Label runat="server" ID="lblError" ForeColor="Red"></ce:Label>
                        </div>
                        <div>
                            <dx:ASPxGridView ID="gvData" runat="server" AutoGenerateColumns="True" ClientInstanceName="gvData" KeyFieldName="ID" Width="100%"
                                ClientIDMode="AutoID" EnableRowsCache="False" EnableViewState="False">
                            </dx:ASPxGridView>
                        </div>
                    </div>
                </div>
                <div class="fieldsetforicons">
                    <div class="left">
                        <ce:ImageLinkButton runat="server" DefaultImageUrl="~/App_Themes/Default/Images/close.png" ID="btnCancel" ToolTip="დახურვა" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

