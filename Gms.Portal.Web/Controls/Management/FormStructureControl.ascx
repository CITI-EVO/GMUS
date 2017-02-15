<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormStructureControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FormStructureControl" %>

<ce:ASPxTreeList ID="tlData" runat="server" EnableViewState="False" AutoGenerateColumns="False"
    KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%"
    ViewStateMode="Disabled" Property="{FormUnitsModel.List=DataSource, Mode=Assigne}">
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
        <Cell HorizontalAlign="Left" VerticalAlign="Middle" Border-BorderColor="#cfcfcf" Border-BorderWidth="1px">
            <border bordercolor="#CFCFCF" borderwidth="1px" />
        </Cell>
        <Header HorizontalAlign="Center" />
    </Styles>
    <Columns>
        <dx:TreeListTextColumn VisibleIndex="1">
            <DataCellTemplate>
                <ce:ImageLinkButton ID="btnEdit" runat="server" DefaultImageUrl="~/App_Themes/default/images/edit.png"
                    ToolTip="Edit" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetEditVisible(Eval("Type")) %>'
                    OnCommand="btnEdit_OnCommand" />
                <ce:ImageLinkButton ID="btnDelete" runat="server" DefaultImageUrl="~/App_Themes/default/images/delete.png"
                    ToolTip="Delete" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetDeleteVisible(Eval("Type")) %>'
                    OnCommand="btnDelete_OnCommand" />
                <ce:ImageLinkButton ID="btnNew" runat="server" DefaultImageUrl="~/App_Themes/default/images/add_group.png"
                    ToolTip="New" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetNewVisible(Eval("Type")) %>'
                    OnCommand="btnNew_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="3">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="Name" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Image runat="server" ImageUrl='<%# GetImageUrl(Eval("Type")) %>' />
                <asp:Label runat="server" Text='<%#Eval("Name") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="4">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="Type" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Type") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="5">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="Number" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Number") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
    </Columns>
</ce:ASPxTreeList>
