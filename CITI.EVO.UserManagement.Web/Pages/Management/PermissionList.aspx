<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
	CodeFile="PermissionList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.PermissionList" %>
<%@ Register TagPrefix="mis" Namespace="CITI.EVO.Tools.Web.UI.Controls" Assembly="CITI.EVO.Tools" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
	<asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always" RenderMode="Block">
		<ContentTemplate>
			<h1>
				<asp:Label runat="server">უფლებების დაწესება</asp:Label>
			</h1>
			<div class="page_title_separator"></div>

			<div class="fieldset">
				<div class="box">
					<h3>
						<asp:Label runat="server">პროექტი/მოდული</asp:Label></h3>
					<div class="box_body_short">
						<ce:ASPxComboBox ID="cmbProject" TextField="Name" ValueField="ID"
							runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbProject_SelectedIndexChanged">
						</ce:ASPxComboBox>
					</div>
				</div>
				<div class="clear"></div>
				<div class="wrapper"></div>
				<div class="box_left">
					<h2>
						<asp:Label runat="server">მომხმარებლის ჯგუფები</asp:Label></h2>
					<div class="title_separator"></div>
					<div>
						<ce:ASPxTreeList ID="tlGroups" runat="server" AutoGenerateColumns="False" Width="100%"
							KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID"
							ViewStateMode="Disabled" OnFocusedNodeChanged="tlGroups_FocusedNodeChanged"
							EnableCallbacks="False" DataCacheMode="Disabled">
							<Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Horizontal" ShowTreeLines="false" />
							<%--<SettingsSelection Enabled="True" />--%>
							<%--<SettingsBehavior ProcessSelectionChangedOnServer="True" />--%>
							<SettingsBehavior ProcessFocusedNodeChangedOnServer="True" />
							<SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowSort="false" AllowFocusedNode="true" />
							<SettingsEditing Mode="EditFormAndDisplayNode" />
							<Styles>
								<AlternatingNode Enabled="true" />
								<Cell HorizontalAlign="Left" VerticalAlign="Middle" />
								<Header HorizontalAlign="Center">
									<Border BorderColor="#D7D7D7" />
								</Header>
							</Styles>
							<Columns>
								<dx:TreeListTextColumn VisibleIndex="0">
									<HeaderCaptionTemplate>
										<asp:Label runat="server" Text="სახელი" />
									</HeaderCaptionTemplate>
									<DataCellTemplate>
										<asp:Label runat="server" Text='<%#Eval("Name") %>' />
									</DataCellTemplate>
								</dx:TreeListTextColumn>
							</Columns>
							<Styles>
								<Header BackColor="#E9E9E9">
								</Header>
							</Styles>
							<Border BorderColor="#D7D7D7" />
						</ce:ASPxTreeList>
					</div>
				</div>
				<div class="box_right">
					<h2>
						<asp:Label runat="server">რესურსები</asp:Label></h2>
					<div class="title_separator"></div>
					<div>
						<asp:TextBox runat="server" ID="tbxKeyword"></asp:TextBox>
						<asp:LinkButton ID="btnSearch" runat="server" ToolTip="დამატება" CssClass="icon" Text="ძებნა" />
					</div>
					<ce:ASPxTreeList ID="tlResources" runat="server" AutoGenerateColumns="False" Width="100%"
						KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" ViewStateMode="Disabled"
						OnFocusedNodeChanged="tlResources_FocusedNodeChanged" EnableCallbacks="false"
						DataCacheMode="Disabled">
						<Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Horizontal" ShowTreeLines="false" />
						<SettingsBehavior ExpandCollapseAction="NodeDblClick"
							AllowSort="false"
							AllowFocusedNode="true"
							ProcessFocusedNodeChangedOnServer="true" />
						<SettingsEditing Mode="EditFormAndDisplayNode" />
						<Styles>
							<AlternatingNode Enabled="true" />
							<Cell HorizontalAlign="Left" VerticalAlign="Middle" />
							<Header HorizontalAlign="Center">
								<Border BorderColor="#D7D7D7" />
							</Header>
						</Styles>
						<Columns>
							<dx:TreeListTextColumn VisibleIndex="3">
								<HeaderCaptionTemplate>
									<asp:Label runat="server" Text="სახელი" />
								</HeaderCaptionTemplate>
								<DataCellTemplate>
									<asp:Label runat="server" Text='<%#Eval("Name") %>' />
								</DataCellTemplate>
							</dx:TreeListTextColumn>
							<dx:TreeListTextColumn VisibleIndex="4">
								<HeaderCaptionTemplate>
									<asp:Label runat="server" Text="მნიშვნელობა" />
								</HeaderCaptionTemplate>
								<DataCellTemplate>
									<asp:Label runat="server" Text='<%#Eval("Value") %>' />
								</DataCellTemplate>
							</dx:TreeListTextColumn>
							<dx:TreeListTextColumn VisibleIndex="5">
								<HeaderCaptionTemplate>
									<asp:Label runat="server" Text="აღწერილობა" />
								</HeaderCaptionTemplate>
								<DataCellTemplate>
									<asp:Label runat="server" Text='<%#Eval("Description") %>' />
								</DataCellTemplate>
							</dx:TreeListTextColumn>
							<dx:TreeListTextColumn VisibleIndex="6">
								<HeaderCaptionTemplate>
									<asp:Label runat="server" Text="ტიპი" />
								</HeaderCaptionTemplate>
								<DataCellTemplate>
									<asp:Label runat="server" Text='<%#Eval("Type") %>' />
								</DataCellTemplate>
							</dx:TreeListTextColumn>
						</Columns>
						<Styles>
							<Header BackColor="#E9E9E9">
							</Header>
						</Styles>
						<Border BorderColor="#D7D7D7" />
					</ce:ASPxTreeList>
				</div>
			</div>








			<ce:ImageLinkButton ID="btPermissionParameter" runat="server" DefaultImageUrl="~/App_Themes/default/images/add_icon.png"
				Text="დამატებითი ატრიბუტები" ToolTip="დაწესება" OnClick="btPermissionParameter_Click" />

			<ce:ImageLinkButton ID="btnSet" runat="server" DefaultImageUrl="~/App_Themes/default/images/establish.png"
				ToolTip="დაწესება" OnClick="btnSet_Click" />

			<ce:ImageLinkButton ID="btnRemove" runat="server" DefaultImageUrl="~/App_Themes/default/images/remove.png"
				ToolTip="მოხსნა" OnClick="btnRemove_Click" />





			<ce:CheckBoxList ID="chklRules" Font-Size="12px" runat="server" TextAlign="Left" RepeatDirection="Horizontal">
				<asp:ListItem Value="0">ნახვა</asp:ListItem>
				<asp:ListItem Value="1">დამატება</asp:ListItem>
				<asp:ListItem Value="2">რედაქტირება</asp:ListItem>
				<asp:ListItem Value="3">წაშლა</asp:ListItem>
			</ce:CheckBoxList>

			<asp:Label runat="server" ID="lbPermissionInfo" />

		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:Panel ID="PanelPermissionParameters" runat="server" Style="display: none" DefaultButton="btPermissionParameter">
		<asp:UpdatePanel ID="UpdatePanelPermissionParameters" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<asp:Button ID="btPermissionParameterPopup" runat="server" Style="display: none;" />
				<act:ModalPopupExtender ID="mpePermissionParametersForm" runat="server" PopupControlID="PanelPermissionParameters"
					BackgroundCssClass="modalBackground" TargetControlID="btPermissionParameterPopup" />
				<div class="ModalWindow" style="width: 500px;">
					<div class="popup_centerpanelcell">
						<div class="popup_center_top">
							<div class="tctpop">
								<div class="bctpop">
									<div class="lctpop">
										<div class="rctpop">
											<div class="blctpop">
												<div class="brctpop">
													<div class="tlctpop">
														<div class="trctpop">
															<div class="popup_top_title">
																<asp:Label ID="UserContext" runat="server">დამატებითი ატრიბუტები</asp:Label>
															</div>
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
						<div class="tpop">
							<div class="bpop">
								<div class="lpop">
									<div class="rpop">
										<div class="blpop">
											<div class="brpop">
												<div class="tlpop">
													<div class="trpop">
														<div class="stationary_popup">
															<div class="popup">
																<div style="float: left;">
																	<div style="float: left;">
																		<asp:HiddenField runat="server" ID="hdPermissionID" />
																		<div class="box_body_popup">
																			<div class="box_title_popup" style="color: #5D5D5D; font-size: 12px; width: 150px; padding: 5px;">
																				<asp:Label runat="server">სახელი</asp:Label>
																			</div>
																			<div class="box_title_popup" style="padding: 5px;">
																				<asp:TextBox ID="tbName" Width="173" runat="server" />
																			</div>
																		</div>
																		<div class="box_body_popup">
																			<div class="box_title_popup" style="color: #5D5D5D; font-size: 12px; width: 150px; padding: 5px;">
																				<asp:Label runat="server">მნიშვნელობა</asp:Label>
																			</div>
																			<div class="box_title_popup" style="padding: 5px;">
																				<asp:TextBox ID="tbValue" Width="173" runat="server" />
																			</div>
																		</div>
																	</div>
																	<div style="width: 100%; padding: 0px; margin: 0px; float: left;">
																		<ce:ASPxGridView ID="gvPermissionParameters" Width="100%" runat="server">
																			<Columns>
																				<dx:GridViewDataColumn FieldName="Name" VisibleIndex="0">
																					<HeaderCaptionTemplate>
																						<asp:Label runat="server">სახელი</asp:Label>
																					</HeaderCaptionTemplate>
																				</dx:GridViewDataColumn>
																				<dx:GridViewDataColumn FieldName="Value" VisibleIndex="1">
																					<HeaderCaptionTemplate>
																						<asp:Label runat="server">მნიშვნელიბა</asp:Label>
																					</HeaderCaptionTemplate>
																				</dx:GridViewDataColumn>
																				<dx:GridViewDataColumn VisibleIndex="1" Width="3px">
																					<DataItemTemplate>
																						<ce:ImageLinkButton ID="lnkDelete" runat="server" DefaultImageUrl="~/App_Themes/default/images/delete.png"
																							ToolTip="წაშლა" OnClientClick="return confirm('წავშალოთ?');" CommandArgument='<% #Eval("ID")%>'
																							OnClick="lnkDelete_Click" />
																					</DataItemTemplate>
																				</dx:GridViewDataColumn>
																			</Columns>
																		</ce:ASPxGridView>
																	</div>
																	<div style="text-align: left; padding: 13px 0 2px 3px; float: left;">
																		<ce:ImageLinkButton ID="btPermissionParameterOK" DefaultImageUrl="~/App_Themes/default/images/add_icon.png"
																			Text="შენახვა" ToolTip="შენახვა" runat="server" OnClick="btPermissionParameterOK_Click" />
																	</div>
																	<div style="text-align: left; padding: 13px 0 2px 15px; float: left;">
																		<ce:ImageLinkButton ID="btProjectCancel" DefaultImageUrl="~/App_Themes/default/images/close_icon.png"
																			Text="დახურვა" ToolTip="დახურვა" runat="server" />
																	</div>
																</div>
															</div>
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</asp:Panel>
</asp:Content>
