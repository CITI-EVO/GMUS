<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributesSchemasControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.AttributesSchemasControl" %>

<ce:ASPxTreeList ID="tlAttributes" runat="server" AutoGenerateColumns="False" Width="100%"
    KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID"
    ViewStateMode="Disabled">
    <Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Both" ShowTreeLines="True" />
    <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowSort="True" AllowFocusedNode="true" />
    <SettingsEditing Mode="EditFormAndDisplayNode" />
    <SettingsPager Position="Bottom" PageSize="25">
        <Summary Text="{0} გვერდი {1}-დან (სულ {2})"></Summary>
        <PageSizeItemSettings Items="25, 50, 100, 200" Visible="True" Caption="ჩანაწერების რაოდენობა" />
    </SettingsPager>
    <SettingsLoadingPanel Text="მიმდინარეობს მონაცემების დამუშავება&amp;hellip;" />
    <Styles>
        <Header ForeColor="#5D5D5D" Wrap="true" HorizontalAlign="Center">
            <border bordercolor="#F7F7F7" borderstyle="Solid"></border>
        </Header>
        <AlternatingNode Enabled="true" />
        <FocusedNode BackColor="#d7d7d7" ForeColor="#003399" />
        <Cell HorizontalAlign="Left" VerticalAlign="Middle" Border-BorderColor="#cfcfcf"
            Border-BorderWidth="1px">
            <border bordercolor="#CFCFCF" borderwidth="1px" />
        </Cell>
        <Header HorizontalAlign="Center" />
    </Styles>
    <Columns>
        <dx:TreeListDataColumn VisibleIndex="0" Width="3px">
            <DataCellTemplate>
                <asp:LinkButton ID="lnkEdit" runat="server" ToolTip="რედაქტირება"   CssClass="btn btn-primary fa fa-edit" CommandArgument='<% #Eval("Key")%>' Visible='<%# GetEditVisible(Eval("Type")) %>' OnCommand="btnEdit_OnCommand" />
                <asp:LinkButton  ID="lnkDelete" runat="server" ToolTip="წაშლა"  CssClass="btn btn-danger fa fa-trash" CommandArgument='<% #Eval("Key")%>' Visible='<%# GetDeleteVisible(Eval("Type")) %>' OnCommand="btnDelete_OnCommand" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                <asp:LinkButton  ID="lnkNew" runat="server" ToolTip="დამატება"  CssClass="btn btn-primary fa fa-plus" CommandArgument='<% #Eval("Key")%>' Visible='<%# GetNewVisible(Eval("Type")) %>' OnCommand="btnNew_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListDataColumn>
        <dx:TreeListDataColumn VisibleIndex="1">
            <HeaderCaptionTemplate>
                <ce:Label runat="server" Text="სახელი" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Name") %>' />
            </DataCellTemplate>
        </dx:TreeListDataColumn>
    </Columns>
</ce:ASPxTreeList>
