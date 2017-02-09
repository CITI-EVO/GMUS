<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LangSwitcher.ascx.cs"
    Inherits="Controls_LangSwitcher" %>
<dx:ASPxMenu ID="mnuLanguages" Width="20" runat="server" AllowSelectItem="True" ShowPopOutImages="False"
    ImageUrlField="~/App_Themes/default/images/view.png" BorderBetweenItemAndSubMenu="HideAll"
    ItemImagePosition="Top" Orientation="Vertical" OnItemClick="mnuLanguages_OnItemClick">
    <Items>
        <dx:MenuItem Text="" Image-Url="~/App_Themes/default/images/language.png">
        </dx:MenuItem>
    </Items>
    <Border BorderStyle="None" />
</dx:ASPxMenu>
