<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsersControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.UsersControl" %>

<ce:ASPxGridView ID="gvUsers" ClientInstanceName="gvUsers" runat="server" KeyFieldName="ID" Width="100%" EnableRowsCache="False"  Property="{UsersModel.List=DataSource, Mode=Assigne}">
    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="რაოდენობა = {0:d}" SummaryType="Count" />
    </GroupSummary>
    <SettingsBehavior AllowGroup="true" AllowSort="true" />
    <Settings ShowGroupPanel="true" ShowFilterBar="Hidden" />
    <SettingsText HeaderFilterShowAll="ყველა" HeaderFilterShowBlanks="მნიშვნელობის გარეშე"
        GroupContinuedOnNextPage="(გაგრძელება იხ. შემდეგ გვერდზე)" HeaderFilterShowNonBlanks="შევსებული"
        GroupPanel="გადმოიტანეთ ის სვეტი რომლის მიხედვითაც გინდათ ცხრილის დაჯგუფება"
        EmptyDataRow="მონაცემები არ მოიძებნა" />
    <SettingsPager Position="Bottom" PageSize="25">
        <Summary Text="{0} გვერდი {1}-დან (სულ {2})"></Summary>
        <PageSizeItemSettings Items="25, 50, 100, 200" Visible="True" Caption="ჩანაწერების რაოდენობა" />
    </SettingsPager>
    <SettingsLoadingPanel Text="მიმდინარეობს მონაცემების დამუშავება&amp;hellip;" />
    <Styles>
        <Header ForeColor="#5D5D5D" Wrap="True" HorizontalAlign="Center">
            <border bordercolor="#F7F7F7" borderstyle="Solid" />
        </Header>
        <AlternatingRow Enabled="True">
        </AlternatingRow>
        <FocusedRow BackColor="#d7d7d7">
        </FocusedRow>
        <GroupPanel BackColor="#003399" ForeColor="White">
            <border bordercolor="White" />
            <BorderTop BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
            <BorderBottom BorderColor="White" BorderStyle="Solid" />
        </GroupPanel>
    </Styles>
    <Columns>
        <dx:GridViewDataColumn VisibleIndex="0" Width="3px" Name="Edit">
            <DataItemTemplate>
                <ce:ImageLinkButton ID="lnkEdit" runat="server" DefaultImageUrl="~/App_Themes/default/images/edit.png"
                    ToolTip="რედაქტირება" CommandArgument='<% #Eval("ID")%>' OnCommand="btnEdit_OnCommand" />
            </DataItemTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn VisibleIndex="1" Width="3px" Name="View">
            <DataItemTemplate>
                <ce:ImageLinkButton ID="lnkView" runat="server" DefaultImageUrl="~/App_Themes/default/images/view.png"
                    ToolTip="ნახვა" CommandArgument='<% #Eval("ID")%>' OnCommand="btnView_OnCommand" />
            </DataItemTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn VisibleIndex="2" Width="3px" Name="AddMessage">
            <DataItemTemplate>
                <ce:ImageLinkButton ID="btnNewMessage" runat="server"
                    DefaultImageUrl="~/App_Themes/default/images/add_message.png" ToolTip="მესიჯის დამატება"
                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnNewMessage_OnCommand" />
            </DataItemTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn VisibleIndex="3" Width="3px" Name="Delete">
            <DataItemTemplate>
                <ce:ImageLinkButton ID="lnkDelete" runat="server" DefaultImageUrl="~/App_Themes/default/images/delete.png"
                    ToolTip="წაშლა" OnClientClick="return confirm('გსურთ მომხმარებილის წაშლა?');"
                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnDelete_OnCommand" />
            </DataItemTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn VisibleIndex="4" Width="3px" Name="AddAttributes">
            <DataItemTemplate>
                <ce:ImageLinkButton ID="btnSetAttribute" runat="server"
                    DefaultImageUrl="~/App_Themes/default/images/add.png" ToolTip="ატრიბუტების დამატება"
                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnSetAttribute_OnCommand" />
            </DataItemTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn VisibleIndex="5" Width="3px" Name="ShowAttributes">
            <DataItemTemplate>
                <ce:ImageLinkButton ID="btnViewAttributes" runat="server"
                    DefaultImageUrl="~/App_Themes/default/images/view.png" ToolTip="ატრიბუტების ნახვა"
                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnViewAttributes_OnCommand" />
            </DataItemTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn Caption="ID" FieldName="ID" VisibleIndex="6" Visible="false" />
        <dx:GridViewDataColumn FieldName="LoginName" VisibleIndex="7">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">მომხმარებელი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="Password" VisibleIndex="8">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">პაროლი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn Caption="სახელი" FieldName="FirstName" VisibleIndex="9">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">სახელი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="LastName" VisibleIndex="10">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">გვარი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="IsActive" VisibleIndex="11">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">აქტივაცია</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="Email" VisibleIndex="12">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">მეილი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="Address" VisibleIndex="13">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">მისამართი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="PasswordExpire" VisibleIndex="14">
            <HeaderCaptionTemplate>
                <asp:Label ID="Label9" runat="server">პაროლის ვალიდურობის თარიღი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
    </Columns>
</ce:ASPxGridView>
