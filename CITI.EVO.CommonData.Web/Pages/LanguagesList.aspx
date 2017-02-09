<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LanguagesList.aspx.cs" Inherits="Pages_LanguagesList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1" />
        <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
            <ContentTemplate>
                <div>
                    <asp:Button runat="server" ID="btAddLanguage" Text="Add Language" OnClick="btAddLanguage_OnClick" />
                </div>
                <div>
                    <dx:ASPxGridView ID="gvLanguages" ClientInstanceName="gvLanguages" runat="server"
                        KeyFieldName="ID" Width="100%">
                        <Settings ShowVerticalScrollBar="False" ShowHorizontalScrollBar="False" ShowGroupPanel="True"
                            ShowHeaderFilterButton="False" ShowFilterRowMenu="False" ShowFilterBar="Visible" />
                        <SettingsPager PageSize="35" AlwaysShowPager="True">
                        </SettingsPager>
                        <SettingsBehavior AllowFocusedRow="True" />
                        <SettingsCookies Enabled="True" CookiesID="gvTrn" StoreColumnsVisiblePosition="True"
                            StoreColumnsWidth="True" StoreFiltering="True" StoreGroupingAndSorting="True"
                            StorePaging="True" />
                        <Styles>
                            <AlternatingRow Enabled="True" ForeColor="#5D5D5D" />
                            <Cell HorizontalAlign="Left" VerticalAlign="Middle" />
                            <Header HorizontalAlign="Center" Wrap="True" ForeColor="#5D5D5D">
                                <Border BorderStyle="None" />
                            </Header>
                            <GroupRow ForeColor="#5D5D5D">
                            </GroupRow>
                            <Row ForeColor="#5D5D5D">
                            </Row>
                            <FocusedRow BackColor="#D7D7D7">
                            </FocusedRow>
                            <GroupPanel>
                                <BorderTop BorderColor="#F7F7F7" BorderStyle="Solid" BorderWidth="1px" />
                                <BorderBottom BorderColor="#F7F7F7" BorderStyle="Solid" BorderWidth="1px" />
                            </GroupPanel>
                            <PagerBottomPanel ForeColor="#5d5d5d">
                            </PagerBottomPanel>
                        </Styles>
                        <StylesPopup>
                            <FilterBuilder>
                                <Header CssClass="filter_header" />
                            </FilterBuilder>
                        </StylesPopup>
                        <StylesPager>
                            <PageNumber ForeColor="#5d5d5d">
                            </PageNumber>
                        </StylesPager>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Display Name" FieldName="DisplayName" Name="DisplayName"
                                VisibleIndex="0" FixedStyle="Left">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="English Name" FieldName="EngName" Name="EngName"
                                VisibleIndex="1" FixedStyle="Left">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Native Name" FieldName="NativeName" Name="NativeName"
                                VisibleIndex="2" FixedStyle="Left">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Pair" FieldName="Pair" Name="Pair" VisibleIndex="0"
                                FixedStyle="Left">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="3" FixedStyle="Left">
                                <DataItemTemplate>
                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="lnkEditLang_Click">[Edit]</asp:LinkButton>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="4" FixedStyle="Left">
                                <DataItemTemplate>
                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="lnkDeleteLang_Click">[Delete]</asp:LinkButton>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" UpdateMode="Always" ID="upnlAddEdit">
            <ContentTemplate>
                <asp:Button runat="server" ID="btShowAddEditPopup" Style="display: none;" />
                <act:ModalPopupExtender runat="server" TargetControlID="btShowAddEditPopup" PopupControlID="pnlAddEdit"
                    ID="mpeAddEdit" />
                <asp:Panel runat="server" ID="pnlAddEdit" Style="padding: 10px; font-family: Trebuchet MS, Verdana, Tahoma; background-color: white; font-size: 12px;">
                    <div>
                        <table>
                            <tr>
                                <td>System Known Languages:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSystemLanguages" DataTextField="DisplayName" DataValueField="Name" AutoPostBack="True" OnSelectedIndexChanged="ddlSystemLanguages_OnSelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label runat="server" ID="lblMessage" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Display Name:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbDisplayName" ReadOnly="True" Width="680px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>English Name:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbEngName" ReadOnly="True" Width="680px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Native Name:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbNativeName" ReadOnly="True" Width="680px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Pair:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbPair" ReadOnly="True" Width="680px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button runat="server" ID="btSave" Text="Save" OnClick="btSave_Click" />
                                    <asp:Button runat="server" ID="btCancel" Text="Cancel" OnClick="btCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
