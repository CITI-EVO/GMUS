<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NamedControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.NamedControl" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{NamedModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{NamedModel.ParentID=Value}" />

<div class="form-group">
    <label>Name</label>
    <asp:TextBox runat="server" Property="{NamedModel.Name=Text}" CssClass="form-control"></asp:TextBox>
</div>
