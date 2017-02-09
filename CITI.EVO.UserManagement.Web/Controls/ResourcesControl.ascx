<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResourcesControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ResourcesControl" %>


<dx:ASPxTreeList ID="tlResources" runat="server" AutoGenerateColumns="False"
    KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%"
    ViewStateMode="Disabled"  Property="{ResourcesModel.List=DataSource, Mode=Assigne}">
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
        <dx:TreeListHyperLinkColumn VisibleIndex="0">
            <DataCellTemplate>
                <ce:ImageLinkButton ID="lnkEdit" runat="server" Style="float: right;" ToolTip="რედაქტირება"
                    DefaultImageUrl="~/App_Themes/default/images/edit.png" CommandArgument='<% #Eval("ID")%>'
                    Visible='<%# EditEnabled(Eval("ID")) %>' OnCommand="btnEdit_OnCommand" />
                <ce:ImageLinkButton ID="lnkDelete" runat="server" Style="float: right;" ToolTip="წაშლა"
                    DefaultImageUrl="~/App_Themes/default/images/delete.png" OnClientClick="return confirm('გსურთ რესურსის წაშლა?');"
                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnDelete_OnCommand" Visible='<%# DeleteEnabled(Eval("ID")) %>' />
                <ce:ImageLinkButton ID="lnkNew" runat="server" ToolTip="დამატება" Style="float: right;"
                    DefaultImageUrl="~/App_Themes/default/images/add.png" CommandArgument='<% #Eval("ID")%>'
                    Visible='<%# NewEnabled(Eval("ID")) %>' OnCommand="btnNew_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListTextColumn VisibleIndex="1">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="სახელი" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Name") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="2">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="მნიშვნელობა" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Value") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="3">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="აღწერა" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Description") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="4">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="ტიპი" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%# GetTypeName(Eval("Type")) %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
    </Columns>
</dx:ASPxTreeList>
