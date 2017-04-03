<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ElementMoveControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.ElementMoveControl" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdfldMovingElementId" Property="{ElementMoveModel.ElementId=Value}" />
<ce:DropDownList runat="server" ID="cbxMovingElements" CssClass="chosen-select" DataTextField="Text" DataValueField="Value" Property="{ElementMoveModel.DestinationId=SelectedValue}" />