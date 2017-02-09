<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ObjectAttributesControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ObjectAttributesControl" %>

<ce:ASPxGridView ID="dwAttributeSchemaNodes" Width="340px" runat="server" EnableViewState="false" Property="{ObjectAttributeUnitsModel.List=DataSource, Mode=Assigne}">
    <SettingsBehavior AllowSort="false" AllowDragDrop="false"></SettingsBehavior>
    <Columns>
        <dx:GridViewDataColumn Caption="სქემა" FieldName="Schema" VisibleIndex="0">
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn Caption="სახელი" FieldName="Node" VisibleIndex="1">
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn Caption="მნიშვნელიბა" FieldName="Value" VisibleIndex="2">
        </dx:GridViewDataColumn>
    </Columns>
</ce:ASPxGridView>
