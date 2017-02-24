<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ObjectAttributesControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ObjectAttributesControl" %>

<ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
    CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{ObjectAttributeUnitsModel.List=DataSource, Mode=Assigne}">
    <Columns>
        <asp:BoundField HeaderText="სქემა" DataField="Schema"  />
        <asp:BoundField HeaderText="სახელი" DataField="Node" />
        <asp:BoundField HeaderText="მნიშვნელიბა" DataField="Value" />
    </Columns>
</ce:GridView>
