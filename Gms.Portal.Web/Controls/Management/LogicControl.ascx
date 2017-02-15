<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LogicControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.LogicControl" %>

<%@ Register Src="~/Controls/Common/ExpressionsLogicControl.ascx" TagPrefix="local" TagName="ExpressionsLogicControl" %>

<table>
    <tr>
        <td style="text-align: left; padding: 10px;">
            <ce:Label runat="server" Text="Source Type" />
        </td>
        <td style="text-align: left; padding: 10px;">
            <asp:RadioButtonList runat="server" ID="lstSourceType" Property="LogicModel.SourceType" AutoPostBack="True" RepeatDirection="Horizontal">
                <Items>
                    <asp:ListItem Text="Table" Value="Table" Selected="True" />
                    <asp:ListItem Text="Logic" Value="Logic" />
                </Items>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; padding: 10px;">
            <ce:Label runat="server" Text="Source" />
        </td>
        <td style="text-align: left; padding: 10px;">
            <dx:ASPxComboBox runat="server" ID="cbxSource" TextField="Name" ValueField="ID" ValueType="System.Guid" Property="LogicModel.SourceID">
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; padding: 10px;">
            <ce:Label runat="server" Text="Logic Type" />
        </td>
        <td style="text-align: left; padding: 10px;">
            <asp:RadioButtonList runat="server" ID="lstType" Property="LogicModel.Type" AutoPostBack="True" RepeatDirection="Horizontal">
                <Items>
                    <asp:ListItem Text="Logic" Value="Logic" Selected="True" />
                    <asp:ListItem Text="Query" Value="Query" />
                </Items>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; padding: 10px;">
            <ce:Label runat="server" Text="Name" />
        </td>
        <td style="text-align: left; padding: 10px;">
            <asp:TextBox runat="server" Property="LogicModel.Name" Width="500px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel runat="server" ID="pnlQuery">
                <asp:TextBox runat="server" TextMode="MultiLine" Property="LogicModel.Query"></asp:TextBox>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlLogic">
                <local:ExpressionsLogicControl runat="server" ID="expressionsLogicControl" Property="LogicModel.ExpressionsLogic"></local:ExpressionsLogicControl>
            </asp:Panel>
        </td>
    </tr>
</table>
<div class="wrapper"></div>
