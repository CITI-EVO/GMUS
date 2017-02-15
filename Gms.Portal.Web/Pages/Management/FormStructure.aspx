<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FormStructure.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.FormStructure" %>

<%@ Register TagPrefix="local" TagName="FormElementControl" Src="~/Controls/Management/FormElementControl.ascx" %>
<%@ Register TagPrefix="local" TagName="FormStructureControl" Src="~/Controls/Management/FormStructureControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <asp:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" Text="New"></asp:LinkButton>
    </div>
    <div>
        <local:FormStructureControl runat="server" ID="formStructureControl"
            OnNew="formStructureControl_OnNew"
            OnEdit="formStructureControl_OnEdit"
            OnDelete="formStructureControl_OnDelete" />
        
    </div>
    <div>
        <asp:Panel ID="pnlFormElement" runat="server" Style="display: none" DefaultButton="btFormElementOK">
            <asp:Button ID="btnFormElementFake" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeFormElement" runat="server" PopupControlID="pnlFormElement" BackgroundCssClass="modalBackground" TargetControlID="btnFormElementFake" />

            <div class="popup">
                <div class="popup_fieldset">
                    <h2>
                        <asp:Label ID="lblFormElement" runat="server" ForeColor="Red"></asp:Label>
                    </h2>
                    <div class="title_separator"></div>
                    <div class="box">
                        <local:FormElementControl runat="server" ID="formElementControl" OnDataChanged="formElementControl_OnDataChanged" />
                    </div>
                </div>
                <div class="fieldsetforicons">
                    <div class="left">
                        <asp:LinkButton ID="btFormElementOK" CssClass="icon" Text="შენახვა" ToolTip="შენახვა" runat="server" OnClick="btnFormElementOK_Click" />
                    </div>
                    <div class="right">
                        <asp:LinkButton ID="btFormElementCancel" CssClass="icon" Text="დახურვა" ToolTip="დახურვა" runat="server" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

