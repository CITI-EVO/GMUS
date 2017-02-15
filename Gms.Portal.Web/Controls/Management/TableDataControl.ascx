<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TableDataControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.TableDataControl" %>
<div class="box" style="width: 800px;">
    <ul style="height: 40px;">
        <li style="width: 70px;">
            <ce:ImageLinkButton runat="server" CssClass="imagelinkbutton" Text="Add" ID="btnAdd" OnClick="btnAdd_OnClick" DefaultImageUrl="~/App_Themes/Default/images/add.png" />
        </li>
        <li style="width: 70px;">
            <ce:ImageLinkButton runat="server" Text="Clear" ID="btnClear" CssClass="imagelinkbutton" OnClick="btnClear_OnClick" OnClientClick="return confirm('Are you sure you want to delete all data?');" DefaultImageUrl="~/App_Themes/Default/images/clear.png" />
        </li>
        <li style="width: 100px;">
            <ce:ImageLinkButton runat="server" Text="Template" ID="btnTemplate" CssClass="imagelinkbutton" OnClick="btnTemplate_OnClick" DefaultImageUrl="~/App_Themes/Default/images/template.png" />
        </li>
    </ul>
    <ul>
        <li style="width: 100px;">
            <ce:ImageLinkButton runat="server" Text="Export" ID="btnExport" CssClass="imagelinkbutton" OnClick="btnExport_OnClick" DefaultImageUrl="~/App_Themes/Default/images/download.png" />
        </li>
        <li style="width: 100px;">
            <ce:ImageLinkButton runat="server" Text="Import" ID="btnImport" CssClass="imagelinkbutton" OnClick="btnImport_OnClick" DefaultImageUrl="~/App_Themes/Default/images/import.png" />
        </li>
        <li style="width: 250px;">
            <asp:FileUpload runat="server" ID="fuImport" />
        </li>
    </ul>
</div>

<div>
    <asp:SqlDataSource runat="server" ID="sqlDs"></asp:SqlDataSource>
    <dx:ASPxGridView runat="server" ID="gvData" DataSourceID="sqlDs"
        OnCellEditorInitialize="gvData_CellEditorInitialize"
        OnRowInserting="gvData_RowInserting"
        OnRowUpdating="gvData_RowUpdating"
        OnRowDeleting="gvData_OnRowDeleting">
        <SettingsEditing Mode="PopupEditForm" NewItemRowPosition="Top" />
    </dx:ASPxGridView>
</div>
