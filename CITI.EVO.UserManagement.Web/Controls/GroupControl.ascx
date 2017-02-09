<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.GroupControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{GroupModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{GroupModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdProjectID" Property="{GroupModel.ProjectID=Value}" />

<div class="box">
    <h3>
        <asp:Label runat="server">ჯგუფის სახელი</asp:Label>
    </h3>
    <div class="box_body">
        <asp:TextBox ID="tbxName" runat="server" Property="{GroupModel.Name=Text}"></asp:TextBox>
    </div>
</div>