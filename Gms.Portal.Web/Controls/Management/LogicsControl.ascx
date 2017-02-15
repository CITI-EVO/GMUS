<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LogicsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.LogicsControl" %>
<dx:ASPxGridView ID="gvData"
	runat="server"
	AutoGenerateColumns="False"
	ClientInstanceName="gvData"
	KeyFieldName="ID"
	Width="100%"
	ClientIDMode="AutoID"
	EnableRowsCache="False"
	EnableViewState="False">
    <Settings ShowFilterRow="True"></Settings>
	<Columns>
		<dx:GridViewDataTextColumn Caption=" " FieldName="" VisibleIndex="0" Name="colViewCommand">
			<DataItemTemplate>
				<ce:ImageLinkButton runat="server" ToolTip="View" Target="_blank" CommandArgument='<%# Eval("ID") %>' DefaultImageUrl="~/App_Themes/Default/images/view.png" ID="btnView" OnCommand="btnView_OnCommand" />
				<ce:ImageLinkButton runat="server" ToolTip="Edit" Target="_blank" CommandArgument='<%# Eval("ID") %>' DefaultImageUrl="~/App_Themes/Default/images/edit.png" ID="btnEdit" OnCommand="btnEdit_OnCommand" />
				<ce:ImageLinkButton runat="server" ToolTip="Delete" Target="_blank" CommandArgument='<%# Eval("ID") %>' DefaultImageUrl="~/App_Themes/Default/images/delete.png" ID="btnDelete" OnCommand="btnDelete_OnCommand" OnClientClick="return confirm('Are you sure you want to delete?');" />
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Name" FieldName="Name"/>
		<dx:GridViewDataTextColumn Caption="Type" FieldName="Type"/>
		<dx:GridViewDataTextColumn Caption="SourceType" FieldName="SourceType"/>
	</Columns>
</dx:ASPxGridView>