<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormElementControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FormElementControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{FormElementModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{FormElementModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentType" Property="{FormElementModel.ParentType=Value}" />

<table>
    <tr>
        <td>Name</td>
        <td>
            <asp:TextBox runat="server" ID="tbxName" Property="{FormElementModel.Name=Text}"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server">
        <td>Visible</td>
        <td>
            <asp:CheckBox runat="server" ID="tbxVisible" Property="{FormElementModel.Visible=Checked}"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="trNumber">
        <td>Number</td>
        <td>
            <asp:TextBox runat="server" ID="tbxNumber" Property="{FormElementModel.Number=Text}"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="trLanguage">
        <td>Language</td>
        <td>
            <dx:ASPxComboBox runat="server" ID="cbxLanguage" TextField="DisplayName" ValueField="Pair" ValueType="System.String" Property="{FormElementModel.Language=Value}">
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <td>ElementType</td>
        <td>
            <dx:ASPxComboBox runat="server" ID="cbxElementType" ValueType="System.String" Property="{FormElementModel.ElementType=Value}" AutoPostBack="True" OnSelectedIndexChanged="cbxElementType_OnSelectedIndexChanged">
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr runat="server" id="trOrderIndex">
        <td>OrderIndex</td>
        <td>
            <dx:ASPxSpinEdit runat="server" ID="seOrderIndex" Property="{FormElementModel.OrderIndex=Value}"></dx:ASPxSpinEdit>
        </td>
    </tr>
    <tr runat="server" id="trType">
        <td>Control Type</td>
        <td>
            <dx:ASPxComboBox runat="server" ID="tbxType" Property="{FormElementModel.ControlType=Value}">
                <Items>
                    <dx:ListEditItem Text="" Value=""/>                    
                    <dx:ListEditItem Text="TextBox" Value="TextBox"/>
                    <dx:ListEditItem Text="CheckBox" Value="CheckBox"/>
                    <dx:ListEditItem Text="RagioButton" Value="RagioButton"/>
                    <dx:ListEditItem Text="ComboBox" Value="ComboBox"/>
                    <dx:ListEditItem Text="FileUpload" Value="FileUpload"/>
                </Items>
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr runat="server" id="trMask">
        <td>Mask</td>
        <td>
            <asp:TextBox runat="server" ID="tbxMask" Property="{FormElementModel.Mask=Text}"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="trEnabled">
        <td>Enabled</td>
        <td>
            <asp:CheckBox runat="server" ID="chkEnabled" Property="{FormElementModel.Enabled=Checked}"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="trValidationExp">
        <td>ValidationExp</td>
        <td>
            <asp:TextBox runat="server" ID="tbxValidationExp" Property="{FormElementModel.ValidationExp=Text}"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="trTag">
        <td>Tag</td>
        <td>
            <asp:TextBox runat="server" ID="tbxTag" Property="{FormElementModel.Tag=Text}"></asp:TextBox>
        </td>
    </tr>
</table>
