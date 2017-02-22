﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ElementsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.ElementsControl" %>

<ce:ASPxTreeList ID="tlData" runat="server" EnableViewState="False" AutoGenerateColumns="False"
    KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%"
    ViewStateMode="Disabled" Property="{ElementNodesModel.List=DataSource, Mode=Assigne}">
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
                <asp:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" Visible='<%# GetEditVisible(Eval("ElementType")) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-info btn-xs">
                    <asp:Label runat="server" CssClass="fa fa-edit"/>
                </asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" Visible='<%# GetDeleteVisible(Eval("ElementType")) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-xs">
                    <asp:Label runat="server" CssClass="fa fa-trash-o"/>
                </asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnNew" ToolTip="New" Visible='<%# GetNewVisible(Eval("ElementType")) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnNew_OnCommand" CssClass="btn btn-info btn-xs">
                    <asp:Label runat="server" CssClass="fa fa-plus"/>
                </asp:LinkButton>
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="3">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="Name" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <div>
                    <span runat="server" class='<%# GetImageClass(Eval("ElementType")) %>'></span>
                    <span style="padding: 2px;"><%#Eval("Name") %></span>
                </div>
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="4">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="ElementType" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("ElementType") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="5">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="ControlType" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("ControlType") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="6">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="Order Index" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("OrderIndex") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn VisibleIndex="7">
            <HeaderCaptionTemplate>
                <asp:Label runat="server" Text="Visible" />
            </HeaderCaptionTemplate>
            <DataCellTemplate>
                <asp:Label runat="server" Text='<%#Eval("Visible") %>' />
            </DataCellTemplate>
        </dx:TreeListTextColumn>
    </Columns>
</ce:ASPxTreeList>
