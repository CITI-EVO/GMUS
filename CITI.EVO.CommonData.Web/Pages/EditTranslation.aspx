<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditTranslation.aspx.cs"
    Inherits="Pages_EditTranslation" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-family: Trebuchet MS, Verdana, Tahoma; font-size: 14px;">
    <form id="form1" runat="server">
    <table>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblMessage"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Module Name:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbModuleName" ReadOnly="True" Width="680px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Translation Key:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbKey" ReadOnly="True" Width="680px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Language Pair:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbLanguagePair" ReadOnly="True" Width="680px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Default Text:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbDefaultText" TextMode="MultiLine" Rows="50" Columns="150"
                    Height="183px" Width="680px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Translated Text:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbTranslatedText" TextMode="MultiLine" Rows="50"
                    Columns="150" Height="183px" Width="680px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button runat="server" ID="btSave" Text="Save" OnClick="btSave_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
