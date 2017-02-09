<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TranslationsList.aspx.cs"
	Inherits="Pages_TranslationsList" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body style="font-family: Trebuchet MS, Verdana, Tahoma; font-size: 14px;">
	<form id="form1" runat="server">
		<asp:ScriptManager runat="server" ID="ScriptManager1" />
		<asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
			<ContentTemplate>
				<table width="100%">
					<tr>
						<td>
							<table>
								<tr>
									<td>ModuleName:
									</td>
									<td>
										<dx:ASPxComboBox runat="server" ID="cbxModules" AutoPostBack="True" OnSelectedIndexChanged="cbxModules_OnSelectedIndexChanged"></dx:ASPxComboBox>
									</td>
									<td>LanguagePair:
									</td>
									<td>
										<dx:ASPxComboBox runat="server" ID="cbxLanguagePairs" AutoPostBack="True" OnSelectedIndexChanged="cbxLanguagePairs_OnSelectedIndexChanged"></dx:ASPxComboBox>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxGridView ID="gvTrns" ClientInstanceName="gvTrns" runat="server" KeyFieldName="ID" Width="100%">
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
									<dx:GridViewDataTextColumn Caption="ModuleName" FieldName="ModuleName" Name="ModuleName"
										VisibleIndex="0" FixedStyle="Left">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="LanguagePair" FieldName="LanguagePair" Name="LanguagePair"
										VisibleIndex="0" FixedStyle="Left">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="DefaultText" FieldName="DefaultText" Name="DefaultText"
										VisibleIndex="0" FixedStyle="Left">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="TranslatedText" FieldName="TranslatedText" Name="TranslatedText"
										VisibleIndex="0" FixedStyle="Left">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn VisibleIndex="0" FixedStyle="Left">
										<DataItemTemplate>
											<asp:LinkButton runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="lnkEditTrn_Click">[Edit]</asp:LinkButton>
										</DataItemTemplate>
									</dx:GridViewDataTextColumn>
								</Columns>
							</dx:ASPxGridView>
						</td>
					</tr>
				</table>
			</ContentTemplate>
		</asp:UpdatePanel>
		<asp:UpdatePanel runat="server" UpdateMode="Always" ID="upnlEdit">
			<ContentTemplate>
				<asp:Button runat="server" ID="btShowEditPopup" Style="display: none;" />
				<act:ModalPopupExtender runat="server" TargetControlID="btShowEditPopup" PopupControlID="pnlEdit"
					ID="mpeEdit" />
				<asp:Panel runat="server" ID="pnlEdit" Style="padding: 10px; font-family: Trebuchet MS, Verdana, Tahoma; background-color: white; font-size: 12px;">
					<table>
						<tr>
							<td colspan="2">
								<asp:Label runat="server" ID="lblMessage"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>Module Name:
							</td>
							<td>
								<asp:TextBox runat="server" ID="tbModuleName" ReadOnly="True" Width="680px"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>Translation Key:
							</td>
							<td>
								<asp:TextBox runat="server" ID="tbKey" ReadOnly="True" Width="680px"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>Language Pair:
							</td>
							<td>
								<asp:TextBox runat="server" ID="tbLanguagePair" ReadOnly="True" Width="680px"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>Default Text:
							</td>
							<td>
								<asp:TextBox runat="server" ID="tbDefaultText" TextMode="MultiLine" Rows="50" Columns="150"
									Height="183px" Width="680px" ReadOnly="True"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>Translated Text:
							</td>
							<td>
								<asp:TextBox runat="server" ID="tbTranslatedText" TextMode="MultiLine" Rows="50"
									Columns="150" Height="183px" Width="680px"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<asp:Button runat="server" ID="btSave" Text="Save" OnClick="btSave_Click" />
								<asp:Button runat="server" ID="btCancel" Text="Cancel" OnClick="btCancel_Click" />
							</td>
						</tr>
					</table>
				</asp:Panel>
			</ContentTemplate>
		</asp:UpdatePanel>
	</form>
</body>
</html>
