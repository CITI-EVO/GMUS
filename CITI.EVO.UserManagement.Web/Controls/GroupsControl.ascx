<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupsControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.GroupsControl" %>

<ce:ASPxTreeList ID="tlGroups" runat="server" EnableViewState="False" AutoGenerateColumns="False"
    KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%"
    ViewStateMode="Disabled" Property="{GroupUnitsModel.List=DataSource, Mode=Assigne}">
    <settings showgroupfooter="false" showfooter="false" gridlines="Both" showtreelines="True" />
    <settingsbehavior expandcollapseaction="NodeDblClick" allowsort="True" allowfocusednode="true" />
    <settingsediting mode="EditFormAndDisplayNode" />
    <settingspager position="Bottom" pagesize="25">
        <Summary Text="{0} გვერდი {1}-დან (სულ {2})"></Summary>
        <PageSizeItemSettings Items="25, 50, 100, 200" Visible="True" Caption="ჩანაწერების რაოდენობა" />
    </settingspager>
    <settingsloadingpanel text="მიმდინარეობს მონაცემების დამუშავება&amp;hellip;" />
    <styles>
        <Header ForeColor="#5D5D5D" Wrap="true" HorizontalAlign="Center">
            <border bordercolor="#F7F7F7" borderstyle="Solid"></border>
        </Header>
        <AlternatingNode Enabled="true" />
        <FocusedNode BackColor="#d7d7d7" ForeColor="#003399" />
        <Cell HorizontalAlign="Left" VerticalAlign="Middle" Border-BorderColor="#cfcfcf" Border-BorderWidth="1px">
            <border bordercolor="#CFCFCF" borderwidth="1px" />
        </Cell>
        <Header HorizontalAlign="Center" />
    </styles>
    <columns>
        <dx:TreeListDataColumn VisibleIndex="0" Width="50px">
            <DataCellTemplate>
                <asp:LinkButton ID="btnEditGroup" runat="server" CssClass="btn btn-primary btn-sm fa fa-edit" ToolTip="რედაქტირება" CommandArgument='<% #Eval("ID")%>' CommandName="EditGroup" Visible='<%# GetEditVisible(Eval("Tag")) %>' OnCommand="btnEdit_OnCommand" />
                <asp:LinkButton ID="btnDeleteGroup" runat="server" CssClass="btn btn-danger btn-sm fa fa-trash" ToolTip="წაშლა" CommandArgument='<% #Eval("ID")%>' CommandName="DeleteGroup" Visible='<%# GetDeleteVisible(Eval("Tag")) %>' OnCommand="btnDelete_OnCommand" />
                <asp:LinkButton ID="btnAddUser" runat="server" CssClass="btn btn-primary btn-sm fa fa-user-plus" ToolTip="მომხმარებლის დამატება" CommandArgument='<% #Eval("ID")%>' CommandName="AddUser" Visible='<%# GetAddUserVisible(Eval("Tag")) %>' OnCommand="btnAddUser_OnCommand" />
                <asp:LinkButton ID="btnAddSubGroup" runat="server" CssClass="btn btn-primary btn-sm fa fa-plus fa-users" ToolTip="ქვე ჯგუფის დამატება" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetNewGroupVisible(Eval("Tag")) %>' CommandName="AddSubGroup" OnCommand="btnNew_OnCommand" />
                <asp:LinkButton ID="btnSetAttributes" runat="server" ToolTip="ატრიბუტების დამატება" CssClass="btn btn-primary btn-sm fa fa-plus" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetAttrSetVisible(Eval("Tag")) %>' OnCommand="btnSetAttribute_OnCommand" />
                <asp:LinkButton ID="btnViewAttributes" runat="server" CssClass="btn btn-primary btn-sm fa fa-search" ToolTip="ატრიბუტების ნახვა" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetAttrViewVisible(Eval("Tag")) %>' OnCommand="btnViewAttributes_OnCommand" />
                <asp:LinkButton ID="btnMessage" runat="server" CssClass="btn btn-primary btn-sm fa fa-bell" ToolTip="მესიჯის დამატება" CommandArgument='<% #Eval("ID")%>' Visible='<%# GetMessageVisible(Eval("Tag")) %>' OnCommand="btnMessage_OnCommand" />
            </DataCellTemplate>
        </dx:TreeListDataColumn>
        <dx:TreeListDataColumn VisibleIndex="1" Width="3px">
            <DataCellTemplate>
                <asp:Image runat="server" ImageUrl='<%# GetImageUrl(Eval("Tag")) %>' />
            </DataCellTemplate>
        </dx:TreeListDataColumn>
        <dx:TreeListDataColumn VisibleIndex="2">
            <HeaderCaptionTemplate>
                <ce:Label runat="server" Text="სახელი" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Name") %>' />
            </DataCellTemplate>
        </dx:TreeListDataColumn>
    </columns>
</ce:ASPxTreeList>
