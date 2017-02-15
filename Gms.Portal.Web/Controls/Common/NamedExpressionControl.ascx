<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NamedExpressionControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.NamedExpressionControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="lmis" TagName="HiddenFieldValueControl" %>

<lmis:HiddenFieldValueControl runat="server" ID="hdKey" Property="NamedExpressionModel.Key" />
<ul>
	<li>
		<ce:Label runat="server">Name:</ce:Label></li>
	<li>
		<asp:TextBox runat="server" ID="txtName" Property="NamedExpressionModel.Name" Width="180" />
	</li>
</ul>
<ul>
	<li>
		<ce:Label runat="server">Expression:</ce:Label></li>
	<li>
		<asp:TextBox runat="server" ID="txtExpression" Property="NamedExpressionModel.Expression" Width="180" />
	</li>
</ul>
<ul>
	<li>
		<ce:Label runat="server">Type:</ce:Label></li>
	<li>
		<asp:DropDownList runat="server" Width="180" ID="ddlType" Property="NamedExpressionModel.OutputType">
			<Items>
				<asp:ListItem Text="Unspecified" Value="Unspecified" />
				<asp:ListItem Text="Text" Value="Text" />
				<asp:ListItem Text="Number" Value="Number" />
				<asp:ListItem Text="DateTime" Value="DateTime" />
			</Items>
		</asp:DropDownList>
	</li>
</ul>
