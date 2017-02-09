<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectsControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ProjectsControl" %>

<ce:ASPxGridView ID="gvData" runat="server" KeyFieldName="ID" Property="{ProjectsModel.List=DataSource, Mode=Assigne}" Width="900" EnableRowsCache="False">
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
        <dx:GridViewDataColumn VisibleIndex="4" Width="3px" Name="Edit">
            <DataItemTemplate>
                <ce:ImageLinkButton ID="btnEdit" runat="server" CommandArgument='<% #Eval("ID")%>'
                    DefaultImageUrl="~/App_Themes/default/images/edit.png" ToolTip="რედაქტირება"
                    OnCommand="btnEdit_OnCommand" />
                <ce:ImageLinkButton ID="btnDelete" runat="server" CommandArgument='<% #Eval("ID")%>'
                    DefaultImageUrl="~/App_Themes/default/images/delete.png" ToolTip="წაშლა" OnCommand="btnDelete_OnCommand"
                    OnClientClick="return confirm('გსურთ მოდულის წაშლა?');" />
            </DataItemTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn Caption="ID" FieldName="ID" VisibleIndex="1" Visible="true" />
        <dx:GridViewDataColumn Caption="მოდული" FieldName="Name" VisibleIndex="2">
            <HeaderCaptionTemplate>
                <asp:Label runat="server">მოდული</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
        <dx:GridViewDataColumn Caption="სტატუსი" FieldName="IsActive" VisibleIndex="3">
            <HeaderCaptionTemplate>
                <asp:Label ID="Label2" runat="server">სტატუსი</asp:Label>
            </HeaderCaptionTemplate>
        </dx:GridViewDataColumn>
    </Columns>
</ce:ASPxGridView>
