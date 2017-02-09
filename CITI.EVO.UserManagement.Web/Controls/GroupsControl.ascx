<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupsControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.GroupsControl" %>


<ce:ASPxTreeList ID="tlGroups" runat="server" EnableViewState="False" AutoGenerateColumns="False"
    KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%"
    ViewStateMode="Disabled" Property="{GroupUnitsModel.List=DataSource, Mode=Assigne}">
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
        <dx:TreeListHyperLinkColumn VisibleIndex="2">
            <DataCellTemplate>
                <asp:LinkButton ID="btnEditGroup" runat="server" DefaultImageUrl="~/App_Themes/default/images/edit.png"
                    ToolTip="რედაქტირება" CommandArgument='<% #Eval("ID")%>' CommandName="EditGroup" Visible='<%# GetEditVisible(Eval("Tag")) %>'
                    OnCommand="btnEdit_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListHyperLinkColumn VisibleIndex="3">
            <DataCellTemplate>
                <ce:ImageLinkButton ID="btnDeleteGroup" runat="server" DefaultImageUrl="~/App_Themes/default/images/delete.png"
                    ToolTip="წაშლა" CommandArgument='<% #Eval("ID")%>' CommandName="DeleteGroup" Visible='<%# GetDeleteVisible(Eval("Tag")) %>'
                    OnCommand="btnDelete_OnCommand"  />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListHyperLinkColumn VisibleIndex="4">
            <DataCellTemplate>
                <ce:ImageLinkButton ID="btnAddUser" runat="server" DefaultImageUrl="~/App_Themes/default/images/add_group.png" 
                    ToolTip="მომხმარებლის დამატება" CommandArgument='<% #Eval("ID")%>' CommandName="AddUser" Visible='<%# GetAddUserVisible(Eval("Tag")) %>' 
                    OnCommand="btnAddUser_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListHyperLinkColumn VisibleIndex="5">
            <DataCellTemplate>
                <ce:ImageLinkButton ID="btnAddSubGroup" runat="server" DefaultImageUrl="~/App_Themes/default/images/add.png" 
                    ToolTip="ქვე ჯგუფის დამატება" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetNewGroupVisible(Eval("Tag")) %>'
                    CommandName="AddSubGroup" OnCommand="btnNew_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListHyperLinkColumn VisibleIndex="6">
            <DataCellTemplate>
                <ce:ImageLinkButton CommandName="SetAttributes" ID="btnSetAttributes" runat="server"
                    ToolTip="ატრიბუტების დამატება" DefaultImageUrl="~/App_Themes/default/images/add_atributes.png"
                    CommandArgument='<% #Eval("ID")%>' Visible='<%# GetAttrSetVisible(Eval("Tag")) %>' 
                    OnCommand="btnSetAttribute_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListHyperLinkColumn VisibleIndex="7" Width="3px">
            <DataCellTemplate>
                <ce:ImageLinkButton CommandName="ViewAttributes" ID="btnViewAttributes" runat="server"
                    DefaultImageUrl="~/App_Themes/default/images/view.png" ToolTip="ატრიბუტების ნახვა"
                    CommandArgument='<% #Eval("ID")%>' Visible='<%# GetAttrViewVisible(Eval("Tag")) %>' 
                    OnCommand="btnViewAttributes_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListHyperLinkColumn VisibleIndex="8" Width="3px">
            <DataCellTemplate>
                <ce:ImageLinkButton CommandName="Message" ID="btnMessage" runat="server"
                    DefaultImageUrl="~/App_Themes/default/images/info.png" ToolTip="მესიჯის დამატება"
                    CommandArgument='<% #Eval("ID")%>' Visible='<%# GetMessageVisible(Eval("Tag")) %>' 
                    OnCommand="btnMessage_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListHyperLinkColumn VisibleIndex="0" Width="3px">
            <DataCellTemplate>
                <asp:Image runat="server" ImageUrl='<%# GetImageUrl(Eval("Tag")) %>' />
            </DataCellTemplate>
        </dx:TreeListHyperLinkColumn>
        <dx:TreeListTextColumn>
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="სახელი" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Name") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
    </Columns>
</ce:ASPxTreeList>
